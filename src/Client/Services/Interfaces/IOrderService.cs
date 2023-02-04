using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//import
using Zalandu.Shared;

namespace Zalandu.Client.Services.Interfaces
{
    public interface IOrderService
    {
        public void AddProductInOrder(Product oneProduct, StockProduct oneStock);
        public Dictionary<OrderItem, int> GetOrderItems();
        public void RemoveProductFromOrder(OrderItem item);
        public void ModifyProducFromOrder(OrderItem item);
        public void CleanItemsFromObservable();
    }
}
