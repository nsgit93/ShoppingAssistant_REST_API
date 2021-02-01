using AutoMapper;
using ShoppingAssistantServer.Entities;
using ShoppingAssistantServer.Models.Filters;
using ShoppingAssistantServer.Models.ShoppingList;
using ShoppingAssistantServer.Models.Store;
using ShoppingAssistantServer.Models.Users;
using System;

namespace ShoppingAssistantServer.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
            CreateMap<Tuple<string, string, double>, StorePriceModel>()
                .ForMember(d => d.storename, opt => opt.MapFrom(s => s.Item1))
                .ForMember(d => d.storeaddress, opt => opt.MapFrom(s => s.Item2))
                .ForMember(d => d.price, opt => opt.MapFrom(s => s.Item3));
            CreateMap<Products, ProductNameModel>()
                .ForMember(pn => pn.Name, opt => opt.MapFrom(p => p.Name));
            CreateMap<Products, ProductDescriptionModel>()
                .ForMember(pn => pn.Description, opt => opt.MapFrom(p => p.Description));
            CreateMap<Stores, StoreModel>();
            CreateMap<Storeschedules, StoreModel>();

        }
    }
}