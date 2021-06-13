using AutoMapper;
using RSS.Business.Models;
using RSS.CompleteApp.ViewModels;

namespace RSS.CompleteApp.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //Verificando se o objeto aninhado é nulo, caso não seja mapeia
            AllowNullCollections = true;
            CreateMap<Supplier, SupplierViewModel>().ForMember(s => s.Address, opt => 
            { 
                opt.Condition(src => src.Adress != null);
                opt.MapFrom(src => src.Adress);
            })
            .ForMember( s=> s.Products, opt => 
            {
                opt.Condition(src => src.Products != null);
                opt.MapFrom(src => src.Products);
            })
            .ReverseMap();
            CreateMap<Address, AddressViewModel>().ReverseMap();
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
