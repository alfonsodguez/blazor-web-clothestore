using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zalandu.Client.Services.Interfaces;
using Zalandu.Shared;
using Microsoft.JSInterop;

namespace Zalandu.Client.Services
{
    public class StorageService : IStorageService
    {
        private IJSRuntime _js;
        public DotNetObjectReference<StorageService> StorageSrvReference; 
        public event EventHandler<Customer>          GetCustomerFromIndexDBEvent;
        public event EventHandler<List<OrderItem>>   ItemsRecuperadosIndexedDBEvent;

        public StorageService(IJSRuntime jsInyect)
        {
            this.StorageSrvReference = DotNetObjectReference.Create(this);
            this._js = jsInyect;
        }


        public async Task GetCustomerFromStorage()
        {
            await this._js.InvokeAsync<Customer>("manageIndexedDB.devuelveCliente", this.StorageSrvReference);
        }

        public async Task GetOrderItemsFromStorage()
        {
            await this._js.InvokeAsync<List<OrderItem>>("manageIndexedDB.devuelveItemsPedido", this.StorageSrvReference);
        }

        [JSInvokable("BlazorDBCallback")]
        public void CalledFromJSCliente(Customer customerResoveJS)
        {
            this.GetCustomerFromIndexDBEvent.Invoke(this, customerResoveJS);
        }

        [JSInvokable("BlazorDBCallbackItemsPedido")]
        public void CalledFromJSOrderItems(List<OrderItem> listOrderItemsJS)
        {
            this.ItemsRecuperadosIndexedDBEvent.Invoke(this, listOrderItemsJS);
        }
        
        public async Task DeleteStorage()
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.borrarDB");
        }

        public async Task<bool> IsCustomerLogged()
        {
            return await this._js.InvokeAsync<bool>("manageIndexedDB.checkIsCustomerLogged");
        }

        public async Task InsertCustomerJWTInStorage(Customer customer, string jwt)
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.almacenaClienteJWT", customer, jwt);
        }

        public async Task InsertOrderItemsInStorage(List<OrderItem> orderItemsList)
        {
            await this._js.InvokeVoidAsync("manageIndexedDB.almacenarItemsPedido", orderItemsList);
        }
    }
}
