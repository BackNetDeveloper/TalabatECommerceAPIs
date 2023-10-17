using APIsMainProject.Dtos;
using AutoMapper;
using Core.Entities;

namespace APIsMainProject.Helper
{
    public class ProductUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration configuration;

        public ProductUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return configuration["ApiUrl"] + source.PictureUrl;
            return null;

        }
    }
}
