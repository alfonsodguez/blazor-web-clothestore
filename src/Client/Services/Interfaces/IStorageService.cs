using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zalandu.Shared;

namespace Zalandu.Client.Services.Interfaces
{
    public interface IStorageService
    {
        public event EventHandler<Customer>        GetCustomerFromIndexDBEvent;
        public event EventHandler<List<OrderItem>> ItemsRecuperadosIndexedDBEvent;

        Task DeleteStorage();
        Task<bool> IsCustomerLogged();
        Task GetCustomerFromStorage();
        Task GetOrderItemsFromStorage();
        void CalledFromJSCliente(Customer customerJS);
        void CalledFromJSOrderItems(List<OrderItem> listOrderItemsJS);
        Task InsertCustomerJWTInStorage(Customer customer, String jwt);
        Task InsertOrderItemsInStorage(List<OrderItem> orderItemsList);
    }
}
