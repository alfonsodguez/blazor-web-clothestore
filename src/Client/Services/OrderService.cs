using System;
using System.Collections.Generic;
using System.Linq;
using Zalandu.Client.Services.Interfaces;
using Zalandu.Shared;
using System.Reactive.Subjects;

namespace Zalandu.Client.Services
{
    public class OrderService : IOrderService
    {
        private BehaviorSubject<Dictionary<OrderItem,int>> _orderItemsInSubject            = new BehaviorSubject<Dictionary<OrderItem, int>>(new Dictionary<OrderItem, int>()); 
        private Dictionary<OrderItem,int>                  _itemsListRecoveredFromSubject  = new Dictionary<OrderItem, int>();
       
        public OrderService()
        {
            this._GetItemsFromSubject();
        }

        private void _GetItemsFromSubject()
        {
            this._orderItemsInSubject.Subscribe((Dictionary<OrderItem, int> items) => this._itemsListRecoveredFromSubject = items);
        }

        public void AddProductInOrder(Product oneProduct, StockProduct oneStock)
        {
            try
            {
                List<OrderItem> _itemsList = this._itemsListRecoveredFromSubject.Keys.ToList<OrderItem>();
                OrderItem       _itemOrder = _itemsList.Find((OrderItem item) => item.OrderProduct.ProductId == oneProduct.ProductId && item.Size == oneStock.Size);
                _itemOrder.OrderAmount += 1;

                this._orderItemsInSubject.OnNext(this._itemsListRecoveredFromSubject);
            }
            catch (Exception ex)
            {
                this._itemsListRecoveredFromSubject.Add(new OrderItem {
                    OrderAmount  = 1,
                    Size         = oneStock.Size,
                    SizePrice    = oneStock.SizePrice == null ? oneProduct.Price : (decimal)oneStock.SizePrice,
                    OrderProduct = oneProduct,
                    ProductId    = oneProduct.ProductId,
                    OrderId      = null 
                },oneStock.Stock);

                this._orderItemsInSubject.OnNext(this._itemsListRecoveredFromSubject);
            }
        }

        public void RemoveProductFromOrder(OrderItem item)
        {
            this._itemsListRecoveredFromSubject.Remove(item);
            this._orderItemsInSubject.OnNext(this._itemsListRecoveredFromSubject);
        }

        public void ModifyProducFromOrder(OrderItem item)
        {
            OrderItem _itemToModify = this._itemsListRecoveredFromSubject.FirstOrDefault( key => key.Key == item).Key;
            _itemToModify = item;

            this._orderItemsInSubject.OnNext(this._itemsListRecoveredFromSubject);
        }

        public Dictionary<OrderItem,int> GetOrderItems()
        {
            return this._itemsListRecoveredFromSubject;
        }

        public void CleanItemsFromObservable() 
        { 
            this._itemsListRecoveredFromSubject = new Dictionary<OrderItem, int>();
            this._orderItemsInSubject           = new BehaviorSubject<Dictionary<OrderItem, int>>(new Dictionary<OrderItem, int>());
        }
    }
}
