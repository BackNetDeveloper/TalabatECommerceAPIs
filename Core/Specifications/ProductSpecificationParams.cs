using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ProductSpecificationParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }
       
        private const int MAXPAGESIZE= 50;
        private int _PageSize = 6;
        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MAXPAGESIZE) ? MAXPAGESIZE : value;
        }
        public int PageIndex { get; set; } = 1;

        private string? _Search;
        public string? Search
        {
            get => _Search;
            set => _Search = value.ToLower();
        }
    }
}
