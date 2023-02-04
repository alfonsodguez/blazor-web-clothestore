using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zalandu.Shared
{
    public class StockProduct
    {
        public String   ProductId { get; set; }
        public String   Size      { get; set; } 
        public Decimal? SizePrice { get; set; } = null!;
        public int      Stock     { get; set; }
    }
}
