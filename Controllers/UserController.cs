using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using ShoppingAssistantServer.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ShoppingAssistantServer.Services;
using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Models.Users;
using System.Net.Mail;
using ShoppingAssistantServer.Models.Store;
using System.Net;

namespace ShoppingAssistantServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IStoreService _storeService;
        private IProductService _productService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IStoreService storeService,
            IProductService productService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _storeService = storeService;
            _productService = productService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate/client")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Incorrect username or password!" });

            /*
            if (user.Verified == "NO")
                return BadRequest(new { message = "The user did not activate its account" });*/

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost("register/client")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            // map model to entity
            var user = _mapper.Map<User>(model);

            try
            {
                // create user
                User created_user = _userService.Create(user, model.Password);

                if (model.Type.Equals("admin"))
                {
                    _userService.CreateAdmin(new Admin(created_user.Id));
                }


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("email");
                mail.To.Add(user.Email);
                mail.Subject = "Registration";
                mail.Body = $"Please select the link below to verify your Web Map email address.\n\n http://localhost:4000/users/validate/{user.Email}";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("email", "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);


                /*
                 Now perform authentication
                 */

                var user_auth = _userService.Authenticate(model.Email, model.Password);

                if (user_auth == null)
                    return BadRequest(new { message = "Incorrect username or password!" });

                /*
                if (user.Verified == "NO")
                    return BadRequest(new { message = "The user did not activate its account" });*/

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                return Ok(new
                {
                    Id = user_auth.Id,
                    Username = user_auth.Username,
                    Token = tokenString
                });

                //return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("validate/{email}")]
        public IActionResult Validate(string email)
        {
            var user = _userService.GetByEmail(email);
            user.Verified = "YES";
            _userService.Update(user);
            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("forgotpass")]
        public IActionResult ForgotPassword([FromBody] ForgotPassModel model)
        {
            //find the user
            try
            {
                User user = _userService.GetByEmail(model.Email);
                //generate a random password for him
                String newPassword = _userService.RandomPassword();
                //update user info
                _userService.Update(user, newPassword);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("email");
                mail.To.Add(user.Email);
                mail.Subject = "New password";
                mail.Body = $"Here is your new password: {newPassword}\n\n Please change it as soon as you log in!\n\n";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("email", "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var model = _mapper.Map<IList<UserModel>>(users);
            return Ok(model);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var model = _mapper.Map<UserModel>(user);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UpdateModel model)
        {
            // map model to entity and set id
            var user = _mapper.Map<User>(model);
            user.Id = id;

            try
            {
                // update user 
                _userService.Update(user, model.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _userService.Delete(id);
            return Ok();
        }


        [HttpPost("shops")]
        public IActionResult AddStore([FromBody] StoreModel model)
        {

            Stores store = new Stores();
            store.Name = model.Name;
            store.Geographic_coordinates = model.Geographic_coordinates;
            store.Address = model.Address;
            Storeschedules schedule = new Storeschedules();
            schedule.Monday = model.Monday;
            schedule.Tuesday = model.Tuesday;
            schedule.Wednesday = model.Wednesday;
            schedule.Thursday = model.Thursday;
            schedule.Friday = model.Friday;
            schedule.Saturday = model.Saturday;
            schedule.Sunday = model.Sunday;
            try
            {
                var createdStore = _storeService.CreateStore(store, model.Id_user);
                schedule.Id_store = createdStore.Id;
                _storeService.CreateSchedule(schedule);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
            //throw new NotImplementedException();
        }


        [HttpPut("shops")]
        public IActionResult UpdateStoreSchedule([FromBody] ScheduleModel model)
        {

            Storeschedules schedule = new Storeschedules();
            schedule.Id_store = model.Id_store;
            schedule.Monday = model.Monday;
            schedule.Tuesday = model.Tuesday;
            schedule.Wednesday = model.Wednesday;
            schedule.Thursday = model.Thursday;
            schedule.Friday = model.Friday;
            schedule.Saturday = model.Saturday;
            schedule.Sunday = model.Sunday;
            if (schedule.Monday.Equals(""))
                schedule.Monday = "closed";
            if (schedule.Tuesday.Equals(""))
                schedule.Tuesday = "closed";
            if (schedule.Wednesday.Equals(""))
                schedule.Wednesday = "closed";
            if (schedule.Thursday.Equals(""))
                schedule.Thursday = "closed";
            if (schedule.Friday.Equals(""))
                schedule.Friday = "closed";
            if (schedule.Saturday.Equals(""))
                schedule.Saturday = "closed";
            if (schedule.Sunday.Equals(""))
                schedule.Sunday = "closed";
            try
            {
                _storeService.UpdateSchedule(schedule);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
            //throw new NotImplementedException();
        }

        [HttpGet("shops/{user_id}")]
        public IActionResult GetStoresByAdmin(int user_id)
        {
            IEnumerable<Stores> stores = _storeService.GetStoresByAdmin(user_id);
            List<StoreScheduleModel> result = new List<StoreScheduleModel>();
            var iter = stores.GetEnumerator();
            iter.Reset();
            iter.MoveNext();
            while(iter.Current != null)
            {
                StoreScheduleModel temp = new StoreScheduleModel();
                temp.Id = iter.Current.Id;
                temp.Name = iter.Current.Name;
                temp.Address = iter.Current.Address;
                temp.Geographic_coordinates = iter.Current.Geographic_coordinates;
                Storeschedules temp_sch = _storeService.GetStoreSchedule(iter.Current.Id);
                temp.Monday = temp_sch.Monday; temp.Tuesday = temp_sch.Tuesday; temp.Wednesday = temp_sch.Wednesday;
                temp.Thursday = temp_sch.Thursday; temp.Friday = temp_sch.Friday; temp.Saturday = temp_sch.Saturday;
                temp.Sunday = temp_sch.Sunday;
                result.Add(temp);
                iter.MoveNext();
            }
           return Ok( result );
        }

        [HttpPost("products")]
        public IActionResult CreateProduct([FromBody] CreateProductModel model)
        {
            Products product = new Products();
            product.Name = model.Name; product.Producer = model.Producer; product.Weight = model.Weight;
            product.Category = model.Category; product.Description = model.Description; product.Bulk_Product = model.Bulk_product;
            try
            {
                var createdProduct = _productService.CreateProduct(product);
                Productstore productstore = new Productstore();
                productstore.Id_product = createdProduct.Id;
                productstore.Id_store = model.Id_store;
                productstore.Price = model.Price;
                productstore.Availability = model.Availability;
                _storeService.CreateProductStore(productstore);
            }
            catch(AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }

        [HttpPut("products")]
        public IActionResult UpdateProduct([FromBody] ProductStoreModel model)
        {
            Productstore productStore = new Productstore();
            productStore.Id_product = model.Id_product; productStore.Id_store = model.Id_store;
            productStore.Price = model.Price; productStore.Availability = model.Availability;
            try
            {
                _storeService.UpdateProductStore(productStore);
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            return Ok();
        }


    }
}
