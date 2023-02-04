using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zalandu.Shared
{
    public class OrderItem
    {
        public String  ProductId    { get; set; } 
        public String  Size         { get; set; }
        public Decimal SizePrice    { get; set; }
        public int     OrderAmount  { get; set; }
        public Product OrderProduct { get; set; }
#nullable enable
        public String? OrderId      { get; set; } 
    }
}
