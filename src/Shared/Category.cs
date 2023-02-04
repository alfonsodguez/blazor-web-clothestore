using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zalandu.Shared
{
    public class Category
    {
        public int    CategoryId       { get; set; }
        public String CategoryName     { get; set; }
        public int    CategoryParentId { get; set; }
    }
}
