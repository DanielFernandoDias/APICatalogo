using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
        CreateMap<Produto, ProdutoDto>().ReverseMap();
        CreateMap<Categoria, CategoriaDto>().ReverseMap();
        }
    }
}
