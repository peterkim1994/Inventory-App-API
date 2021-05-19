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
                opt => opt.MapFrom(src => src.Price / 100.0)
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
                  d => d.ProductIds,
                  o => o.MapFrom(s => s.ProductPromotions.Select(pm => pm.ProductId).ToList())
                )
                .ForMember
                (
                    d => d.Start,
                    o => o.MapFrom(s => s.Start.ToString("yyyy-MM-dd"))
                )
                .ForMember
                (
                    d => d.End,
                    o => o.MapFrom(s => s.End.ToString("yyyy-MM-dd"))
                )
                .ForMember
                (
                    d => d.PromotionPrice,
                    o => o.MapFrom(s => Convert.ToInt32(s.PromotionPrice / 100.0))
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
                )
                .ForMember
                (
                    d => d.PromotionPrice,
                    o => o.MapFrom(s => Convert.ToInt32(s.PromotionPrice * 100.0))
                );

            CreateMap<ProductSale, ProductSaleDto>()
                .ForMember
                (
                    d => d.Product,
                    o => o.MapFrom(s => String.Format("{0} {1} {2} {3}", s.Product.Brand.Value, s.Product.ItemCategory.Value, s.Product.Colour.Value, s.Product.Size.Value))
                )           
                .ForMember
                (
                    d => d.PromotionName,
                    o => o.MapFrom(s => s.Promotion.PromotionName)
                );
        }
    }
}
