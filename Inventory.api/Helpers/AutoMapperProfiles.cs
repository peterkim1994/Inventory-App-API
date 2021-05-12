using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryPOS.Core.Dtos;
using InventoryPOS.DataStore.Models;
using InventoryPOSApp.Core.Dtos;

namespace InventoryPOS.api.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductDto>()
              .ForMember
              (
                dest => dest.Price,
                opt => opt.MapFrom(src => src.Price/100.0)
              );
            CreateMap<ProductDto, Product>()
                .ForMember
                (
                    d => d.Price, 
                    o => o.MapFrom(s => Convert.ToInt32(s.Price * 100.0))
                );           
            CreateMap<Colour, ColourDto>();
            CreateMap<Size, SizeDto>();
            CreateMap<Brand, BrandDto>();
            CreateMap<Promotion, PromotionDto>()
                .ForMember
                (
                    d => d.Products,
                    o => o.MapFrom(s => s.ProductPromotions.Select(pm => pm.ProductId).ToList())
                )
                .ForMember
                (
                    d=> d.Start,
                    o => o.MapFrom(s => s.Start.ToString())
                )
                .ForMember
                (
                    d => d.End,
                    o => o.MapFrom(s => s.End.ToString())
                );
            CreateMap<PromotionDto, Promotion>()
                .ForMember
                (
                    d => d.Start,
                    o => o.MapFrom(s => DateTime.Parse(s.Start))
                )
                .ForMember
                (
                    d => d.End,
                    o => o.MapFrom(s => DateTime.Parse(s.End))
                ); ;
        }
    }
}
