using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Helpers;
using ShoppingAssistantServer.Models.Filters;
using ShoppingAssistantServer.Models.ShoppingList;
using ShoppingAssistantServer.Services;
using System.Collections.Generic;

namespace ShoppingAssistantServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FiltersController : ControllerBase
    {

        private IStoreService _storeService;
        private IProductService _productService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public FiltersController(
            IStoreService storeService,
            IProductService productService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _productService = productService;
            _storeService = storeService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [HttpPost("/filters/products/name")]
        public IActionResult GetProductNameSuggestions([FromBody] ProductNameModel model)
        {
            var response = _mapper.Map<IList<ProductNameModel>>(_productService.GetProductsByNameSubstring(model.Name));
            return Ok(response);
        }

        [HttpPost("/filters/products/description")]
        public IActionResult GetProductDescriptionSuggestions([FromBody] ProductNameModel model)
        {
            var response = _mapper.Map<IList<ProductDescriptionModel>>(_productService.GetProductsByFullName(model.Name));
            return Ok(response);
        }

        [HttpGet("/filters/products/{store_id}")]
        public IActionResult FilterProductsByStoreId(int store_id)
        {
            var products = _productService.GetProductsByStoreId(store_id);
            List<ProductPriceAvailModel> result = new List<ProductPriceAvailModel>();
            var iter = products.GetEnumerator();
            iter.Reset();
            iter.MoveNext();
            while(iter.Current != null)
            {
                Products prod = iter.Current;
                ProductPriceAvailModel temp = new ProductPriceAvailModel();
                temp.Id = prod.Id; temp.Name = prod.Name; temp.Description = prod.Description; temp.Producer = prod.Producer;
                temp.Weight = prod.Weight; temp.Category = prod.Category;
                Productstore ps = _productService.GetProductPriceAvail(prod.Id, store_id);
                temp.Price = ps.Price; temp.Availability = ps.Availability;
                result.Add(temp);
                iter.MoveNext();
            }
            return Ok(result);
        }

        [HttpPost("/filters/stores")]
        public IActionResult FilterStores([FromBody] FilterModel model)
        {
            var response = _mapper
                .Map<List<StorePriceModel>>
                (_storeService.Filter(model.UserCoordinates, model.Radius, model.UserDateTime, model.ShoppingListContent));
            return Ok(response);
        }




    }
}
