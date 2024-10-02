using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Catalog
{
    public class CatalogItemFilter
    {
        public List<string> Brands { get; set; } = new List<string>();
        public List<string> Types { get; set; } = new List<string>();
    }
}
