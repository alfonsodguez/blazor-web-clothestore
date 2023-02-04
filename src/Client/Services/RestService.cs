using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zalandu.Shared;
using System.Net.Http;
using System.Text.Json;
using System.Net.Http.Json;

namespace Zalandu.Client.Services
{
    public class RestService : IRestService
    {
        private readonly HttpClient _http;

        public RestService(HttpClient serviceHttpInject)
        {
            this._http = serviceHttpInject;
        }

        #region =========ZONE CUSTOMER=========
        public async Task<RESTMessage> CustomerSignUp(Customer newCustomer)
        {
            HttpResponseMessage _response = await this._http.PostAsJsonAsync("api/RESTCustomer/SignUp", newCustomer); 

            return JsonSerializer.Deserialize<RESTMessage>(await _response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });
        }

        public async Task<RESTMessage> CustomerLogin(Customer.Credentials creds)
        {
            HttpResponseMessage _response = await this._http.PostAsJsonAsync("api/RESTCustomer/Login", creds);
          
            return  JsonSerializer.Deserialize<RESTMessage>(await _response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });
        }

        public async Task<RESTMessage> ModifyCustomerAddress(Address deliveryAddress)
        {
            HttpResponseMessage _response = await this._http.PostAsJsonAsync<Address>("api/RESTCustomer/UpdateCustomerAddress", deliveryAddress);
            
            return JsonSerializer.Deserialize<RESTMessage>(await _response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });
        }

        public async Task<RESTMessage> ModifyCustomerInfo(Zalandu.Shared.Customer updateCustomer, bool? isAddressChange)
        {
            HttpResponseMessage _response = await this._http.PostAsJsonAsync("api/RESTCustomer/UpdateCustomerInfo?isAddressChange=" + isAddressChange, updateCustomer);

            return await _response.Content.ReadFromJsonAsync<RESTMessage>();  
        }

        public async Task<RESTMessage> SaveCustomerOrder(Order customerOrder)
        {
            HttpResponseMessage _response = await this._http.PostAsJsonAsync<Order>("api/RESTCustomer/SaveCustomerOrder", customerOrder);

            return JsonSerializer.Deserialize<RESTMessage>(await _response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });
        }
        #endregion

        #region =========ZONE SHOP=========
        public async Task<List<Product>> GetProducts(String categoryName)
        {
            HttpResponseMessage _response = await this._http.GetAsync("api/RESTShop/GetProducts?categoryName=" + categoryName);
            RESTMessage _resultado = JsonSerializer.Deserialize<RESTMessage>(await _response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });

            if(_resultado.Data != null)
            {
                return JsonSerializer.Deserialize<List<Product>>(((JsonElement)_resultado.Data).GetRawText());                
            }
            else
            {
                throw new Exception("error en el server...");
            }            
        }

        public async Task<List<Category>> GetCategories()
        {
            HttpResponseMessage _response = await this._http.GetAsync("api/RESTShop/GetCategories");
            RESTMessage _resultado = JsonSerializer.Deserialize<RESTMessage>(await _response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNamingPolicy = null });

            if(_resultado.Data != null)
            {
                return JsonSerializer.Deserialize<List<Category>>(((JsonElement)_resultado.Data).GetRawText());
            }
            else
            {
                throw new Exception("error en el server...");                
            }
        }
        #endregion
    }
}
