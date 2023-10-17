using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductsWithBrandAndTypeSpecification:BaseSpecification<Product>
    {
        public ProductsWithBrandAndTypeSpecification
            (ProductSpecificationParams productParams)
            :base(product =>
            (string.IsNullOrEmpty(productParams.Search) || product.Name.Contains(productParams.Search)) &&
            (!productParams.BrandId.HasValue ||  product.ProductBrandId==productParams.BrandId)&&
            (!productParams.TypeId.HasValue || product.ProductTypeId == productParams.TypeId)
            )
        {
            AddInclude(product => product.ProductType);
            AddInclude(product => product.ProductBrand);
            AddOrderBy(product => product.Name);
            ApplyPaging(productParams.PageSize *(productParams.PageIndex - 1),productParams.PageSize);
            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "PriceAsc":
                             AddOrderBy(product => product.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesending(product => product.Price);
                        break;
                    default:
                        AddOrderBy(product => product.Name);
                        break;
                }
            }
        }
        public ProductsWithBrandAndTypeSpecification( int id)
            :base(product => product.Id==id)
        {
            AddInclude(product => product.ProductType);
            AddInclude(product => product.ProductBrand);
        }
    }
}
