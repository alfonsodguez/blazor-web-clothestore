using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zalandu.Shared;

namespace Zalandu.Client.Services
{
    public interface IRestService
    {
        #region //ZONE CLIENT//
        Task<RESTMessage> CustomerSignUp(Customer newCustomer);
        Task<RESTMessage> CustomerLogin(Customer.Credentials customerLogin);
        Task<RESTMessage> ModifyCustomerAddress(Address deliveryAddress);
        Task<RESTMessage> ModifyCustomerInfo(Customer updateCustomer, bool? isAddressChange);
        Task<RESTMessage> SaveCustomerOrder(Order customerOrder);
        #endregion

        #region //ZONE SHOP//
        Task<List<Product>>  GetProducts(String categoryName);
        Task<List<Category>> GetCategories();
        #endregion
    }
}
