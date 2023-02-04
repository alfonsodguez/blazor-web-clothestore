using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zalandu.Shared;

namespace Zalandu.Shared
{
    public class Order
    {
        public String          OrderId       { get; set; }  
        public String          CustomerId    { get; set; } 
        public String          StatusOrder   { get; set; }            
        public DateTime        DateOrder     { get; set; } 
        public Decimal         DeliveryCosts { get; set; }
        public Decimal         SubtotalOrder { get; set; }
        public Decimal         TotalOrder    { get; set; } 
        public List<OrderItem> OrderList     { get; set; }  

        public Order()
        {
            this.OrderId       = System.Guid.NewGuid().ToString();
            this.OrderList     = new List<OrderItem>();
            this.DeliveryCosts = (decimal) 5.05;  
        }
    }
}
