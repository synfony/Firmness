using AutoMapper;
using Firmness.Core.Models;
using Firmness.ViewModels;
using System.Linq;

namespace Firmness.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Product Mappings
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();

            // Client Mappings
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            CreateMap<Client, ClientDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User != null && src.User.Email != null ? src.User.Email! : string.Empty));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            CreateMap<ClientDto, Client>();

            // Sale Mappings
            CreateMap<Sale, SaleDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => $"{src.Client.FirstName} {src.Client.LastName}"))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.SaleDetails != null ? src.SaleDetails.Sum(sd => sd.Quantity * sd.UnitPrice) : 0m));
            
            CreateMap<SaleDetail, SaleDetailDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name! : "N/A"));

            // Reverse mappings for creation/update if needed
            CreateMap<SaleDto, Sale>();
            CreateMap<SaleDetailDto, SaleDetail>();
        }
    }
}