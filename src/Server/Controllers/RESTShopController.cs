using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Zalandu.Server.Models;
using Zalandu.Shared;

namespace Zalandu.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RESTShopController : ControllerBase
    {
        private readonly ApplicationDBContext _db;

        public RESTShopController(ApplicationDBContext dbcontextInjection)
        {
            this._db = dbcontextInjection;
        }
       
        #region --------ZONE SHOP-------
        [HttpGet]
        public RESTMessage GetCategories()
        {
            try
            {
                List<Category> _listaCategorias =  this._db.Categories.Select(row => row).ToList<Category>();

                return new RESTMessage {
                    Data         = _listaCategorias,
                    CustomerInfo = null,
                    Errors       = null,
                    Message      = "categorias recuperadas ok"
                };
            }
            catch (Exception ex)
            {
                return new RESTMessage {
                    Data         = null,
                    CustomerInfo = null,
                    Errors       = new List<String> { ex.Message },
                    Message      = "fallo al recuperar lista con la categorias"
                };
            }
        }
        
        [HttpGet]
        public RESTMessage GetProducts([FromQuery]String categoryName)  
        {
            try
            {
                List<Product> _productsList           = this._db.Products.Where(row => row.CategoryProduct == categoryName || row.CategoryChild == categoryName || row.CategoryParent == categoryName).ToList<Product>();
                List<StockProduct> _stockProductsList = this._db.StockProducts.Select(row => row).ToList<StockProduct>();

                //----add stock producto to each product corresponding----
                _productsList.ForEach(product => product.Stock = _stockProductsList.Where(stock => stock.ProductId == product.ProductId).ToList());                              

                return new RESTMessage {
                    Data         = _productsList,
                    CustomerInfo = null,
                    Errors       = null,
                    Message      = "productos recuperado ok"
                };
            }
            catch (Exception ex)
            {
                return new RESTMessage {
                    Data         = null,
                    CustomerInfo = null,
                    Errors       = new List<string> { ex.Message },
                    Message      = "error al recuperar productos"
                };
            }
        }       
        #endregion
    }
}
