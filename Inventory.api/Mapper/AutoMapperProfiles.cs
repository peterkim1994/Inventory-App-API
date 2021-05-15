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
                  d => d.ProductIds,
                  o => o.MapFrom(s => s.ProductPromotions.Select(pm => pm.ProductId).ToList())
                //      o => o.MapFrom(s => s.ProductPromotions.ToList())
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
                //.AfterMap((dao, dto) => {
                //    if (dto.Products == null)
                //        dto.Products = new List<ProductDto>();
                //    dto.Products.Clear();      
                //    foreach (var p in dao.ProductPromotions)
                //    {                       
                //        dto.Products.Add(new ProductDto { 
                //            Id = p.ProductId,
                //            //BrandId = p.Product.BrandId,
                //            //BrandValue = p.Product.Brand.Value,
                //            //ColourId = p.Product.ColourId,
                //            //ColourValue = p.Product.Colour.Value,
                //            //SizeId = p.Product.SizeId,
                //            //SizeValue = p.Product.Size.Value,
                //            //ItemCategoryId = p.Product.ItemCategoryId,
                //            //ItemCategoryValue = p.Product.ItemCategory.Value,
                //            //ManufactureCode = p.Product.ManufactureCode,
                //            //Description = p.Product.Description,
                //            //Price = p.Product.Price/100.00,
                //            //Qty = p.Product.Qty
                //        });              
                //    }
                //}); 
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
        }
    }
}
