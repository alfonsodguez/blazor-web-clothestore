using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Zalandu.Shared;
using Zalandu.Server.Models;
using Zalandu.Server.Models.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Storage;

namespace Zalandu.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RESTCustomerController : ControllerBase
    {
        private readonly UserManager<IdentityCustomer>   _userManager;
        private readonly SignInManager<IdentityCustomer> _signinManager;
        private readonly ApplicationDBContext            _db;
        private readonly IEmailClient                    _emailClient;
        private readonly IConfiguration                  _configFile;  

        public RESTCustomerController(UserManager<IdentityCustomer> userManagerInjection, SignInManager<IdentityCustomer> signinManagerInjection, ApplicationDBContext dbcontextInjection, IEmailClient mailjetClienteInjection, IConfiguration accessAppsetting)
        {
            this._userManager   = userManagerInjection;
            this._signinManager = signinManagerInjection;
            this._db            = dbcontextInjection;
            this._emailClient   = mailjetClienteInjection;
            this._configFile    = accessAppsetting;
        }

        #region ==========SIGNUP==========
        [HttpPost]
        public async Task<RESTMessage> SignUp([FromBody] Customer newCustomer )
        {
            try
            {
                IdentityCustomer _customerToSign = new IdentityCustomer {
                    Name     = newCustomer.Name,
                    Surname  = newCustomer.Surname,
                    Birth    = newCustomer.Birth,
                    Email    = newCustomer.CustomerCredentials.Email,
                    UserName = newCustomer.Name
                };

                IdentityResult _createCustomerInStorage = await this._userManager.CreateAsync(_customerToSign, newCustomer.CustomerCredentials.Password);

                if (_createCustomerInStorage.Succeeded)
                {
                    SendEmailActivationAccount(newCustomer);

                    return new RESTMessage {
                        Message      = "Registro realizado ok, a la espera de confirmacion email",
                        Errors       = null,
                        CustomerInfo = null,
                        Token        = null,
                        Data         = null
                    };
                }
                else
                {
                    List<String> _errorsList               = new List<String>();
                    List<IdentityError> _identityErrorList = _createCustomerInStorage.Errors.ToList();

                    _identityErrorList.ForEach(item => _errorsList.Add(item.Description));

                    return new RESTMessage {
                        Message      = "*Error en los datos enviados, vuelva a completar el formulario de registro",
                        Errors       = _errorsList,
                        CustomerInfo = newCustomer,
                        Token        = null,
                        Data         = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new RESTMessage {
                    Message = ex.Message,
                    Errors = new List<String> { "*Fallo al registrar cliente" },
                    CustomerInfo = newCustomer,
                    Token = null,
                    Data = null
                };
            }
        }

        private async void SendEmailActivationAccount(Customer newCustomer)
        {
            IdentityCustomer _customerRegistered = await this._userManager.FindByEmailAsync(newCustomer.CustomerCredentials.Email);
            String _token = await this._userManager.GenerateEmailConfirmationTokenAsync(_customerRegistered);

            String _urlEmail = Url.RouteUrl("ActivateAccount", new { customerId = _customerRegistered.Id, token = _token }, "https", "localhost:44344");
            String _emailMessage = $@"<h3><strong>Se ha registrado correctamente en www.Zalandú.com</strong></h3>.<br>Pulsa en el siguiente enlace: <a href='{_urlEmail}'>'{_urlEmail.Split('?')[0]}'</a> para activar tu cuenta";

            this._emailClient.SendEmail(_customerRegistered.Email, "Confirmación de registro en www.Zalandú.com", _emailMessage, null);
        }

        [HttpGet(Name = "ActivateAccount")]
        public async Task<IActionResult> ConfirmationSignUp([FromQuery] String customerId, [FromQuery] String token) 
        {
            try
            {
                IdentityCustomer _customerIdentity = await this._userManager.FindByIdAsync(customerId);
                IdentityResult _validationToken    = await this._userManager.ConfirmEmailAsync(_customerIdentity, token);

                if (_validationToken.Succeeded)
                {
                    return Redirect("https://localhost:44344/Cliente/Login");
                }
                else
                {
                    return Ok(new RESTMessage {
                        Message      = "Confirmacion email fallida",
                        Errors       = new List<string> { "*Activacion del email por identity fallida" },
                        CustomerInfo = null,
                        Token        = null,
                        Data         = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new RESTMessage {
                    Message      = "Fallo al recuperar cliente identity",
                    Errors       = new List<string> { "no existe cliente", ex.Message },
                    CustomerInfo = null,
                    Token        = null,
                    Data         = null
                });
            }
        }       
        #endregion

        #region ==========LOGIN===========
        [HttpPost]
        public async Task<RESTMessage> Login([FromBody] Customer.Credentials creds)
        {
            try
            {
                IdentityCustomer _customerIdentity = await this._userManager.FindByEmailAsync(creds.Email);

                if (_customerIdentity == null)
                {
                    throw new Exception("*Email incorrecto");
                }

                if (!_customerIdentity.EmailConfirmed)
                {
                    throw new Exception("*Email sin confirmar");
                }

                Microsoft.AspNetCore.Identity.SignInResult _singinResult = await this._signinManager.PasswordSignInAsync(_customerIdentity, creds.Password, false, true); //<--4to argumento: bloqueo de cuenta con 5 fallos

                if (_singinResult.Succeeded)
                {
                    return new RESTMessage
                    {
                        Message      = "Login ok",
                        Errors       = null,
                        CustomerInfo = new Customer {
                            CustomerId          = _customerIdentity.Id,
                            Name                = _customerIdentity.Name,
                            Surname             = _customerIdentity.Surname,
                            Birth               = _customerIdentity.Birth,
                            Dni                 = _customerIdentity.Dni,
                            CustomerCredentials = new Customer.Credentials { Email = _customerIdentity.Email },
                            CurrentOrder        = new Order { CustomerId = _customerIdentity.Id},
                            DeliveryAddress     = this._db.Addresses.Where((Address address) => address.CustomerId == _customerIdentity.Id).SingleOrDefault() ?? new Address { AddressId = System.Guid.NewGuid().ToString(), CustomerId = _customerIdentity.Id },
                            HistoryOrder        = this._db.CustomerOrders.Where((Order pedido) => pedido.CustomerId == _customerIdentity.Id).ToList() ?? null                          
                        },
                        Token = this.GenerarJWTsession(_customerIdentity, creds),
                        Data = null
                    }; 
                }
                else
                {
                    throw new Exception("*Password incorrecta");
                }
            }
            catch (Exception ex)
            {
                return new RESTMessage {
                    Message      = ex.Message,
                    Errors       = new List<string> { "*Email invalido", "*Email sin confirmar", "*Credentials incorrectas" },
                    CustomerInfo = null,
                    Token        = null,
                    Data         = null
                };               
            }
        }

        private string GenerarJWTsession(IdentityCustomer customerIdentity, Customer.Credentials creds)
        {
            SecurityKey _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(this._configFile.GetValue<string>("JWT:Key")) );
            
            JwtSecurityToken _tokenSession = new JwtSecurityToken(
                issuer:   this._configFile.GetValue<string>("JWT:Issuer"),
                audience: null,
                expires:  DateTime.Now.AddHours(2),
                claims:   new List<Claim> {
                    new Claim("Name",       customerIdentity.Name),
                    new Claim("Surname",    customerIdentity.Surname),
                    new Claim("CustomerId", customerIdentity.Id),
                    new Claim("Email",      creds.Email)
                },
                signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(_tokenSession);              
        }
        #endregion

        #region ==========CUSTOMER PANEL==========
        [HttpPost]
        public async Task<RESTMessage> UpdateCustomerAddress([FromBody]Address updateAddress)
        {
            try
            {
                Address _address = this._db.Addresses.Where((Address address) => address.CustomerId == updateAddress.CustomerId).FirstOrDefault();

                if (_address != null)
                {
                    _address.PhoneNumber  = updateAddress.PhoneNumber;
                    _address.RoadName     = updateAddress.RoadName;
                    _address.RoadNumber   = updateAddress.RoadNumber;
                    _address.Block        = updateAddress.Block;
                    _address.Floor        = updateAddress.Floor;
                    _address.Door         = updateAddress.Door;
                    _address.Province     = updateAddress.Province;
                    _address.Municipality = updateAddress.Municipality;
                    _address.CP           = updateAddress.CP;
                    
                    await this._db.SaveChangesAsync();
                }
                else
                {
                    _address = new Address {
                        AddressId    = Guid.NewGuid().ToString(),
                        CustomerId   = updateAddress.CustomerId,
                        RoadName     = updateAddress.RoadName,
                        RoadNumber   = updateAddress.RoadNumber,
                        Block        = updateAddress.Block,
                        Floor        = updateAddress.Floor,
                        Door         = updateAddress.Door,
                        Province     = updateAddress.Province,
                        Municipality = updateAddress.Municipality,
                        CP           = updateAddress.CP
                    };

                    this._db.Addresses.Add(_address);
                    this._db.SaveChanges();
                }


                return new RESTMessage
                {
                    Message = "Data del cliente actualizados ok",
                    Errors = null,
                    CustomerInfo = null,
                    Token = null,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new RESTMessage {
                    Message      = ex.Message,
                    Errors       = new List<String> { "*Fallo al actualizar direccion de envio del cliente" },
                    CustomerInfo = null,
                    Token        = null,
                    Data         = null
                };
            }                
        }

        [HttpPost]
        public async Task<RESTMessage> UpdateCustomerInfo([FromQuery]bool isAddressChange, [FromBody] Customer updateCustomer)
        {
            try
            {
                IdentityCustomer _customerIdentity = await  this._userManager.FindByIdAsync(updateCustomer.CustomerId);

                _customerIdentity.Name    = updateCustomer.Name;
                _customerIdentity.Surname = updateCustomer.Surname;
                _customerIdentity.Dni     = updateCustomer.Dni;
                _customerIdentity.Birth   = updateCustomer.Birth;
                _customerIdentity.Email   = updateCustomer.CustomerCredentials.Email;

                IdentityResult _updateResult = await this._userManager.UpdateAsync(_customerIdentity);
                await this._signinManager.RefreshSignInAsync(_customerIdentity);

                if (_updateResult.Succeeded) 
                {
                    if (isAddressChange)
                    {
                        Address _address =  this._db.Addresses.Where((Address address) => address.CustomerId == updateCustomer.DeliveryAddress.CustomerId).ElementAt(0);

                        if(_address != null)
                        {
                            _address.RoadName     = updateCustomer.DeliveryAddress.RoadName;
                            _address.RoadNumber   = updateCustomer.DeliveryAddress.RoadNumber;
                            _address.Block        = updateCustomer.DeliveryAddress.Block;
                            _address.Floor        = updateCustomer.DeliveryAddress.Floor;
                            _address.Door         = updateCustomer.DeliveryAddress.Door;
                            _address.Province     = updateCustomer.DeliveryAddress.Province;
                            _address.Municipality = updateCustomer.DeliveryAddress.Municipality;
                            _address.CP           = updateCustomer.DeliveryAddress.CP;                            
                        }
                        else
                        {
                            _address = new Address {
                                AddressId    = Guid.NewGuid().ToString(),
                                CustomerId   = updateCustomer.CustomerId,
                                RoadName     = updateCustomer.DeliveryAddress.RoadName,
                                RoadNumber   = updateCustomer.DeliveryAddress.RoadNumber,
                                Block        = updateCustomer.DeliveryAddress.Block,
                                Floor        = updateCustomer.DeliveryAddress.Floor,
                                Door         = updateCustomer.DeliveryAddress.Door,
                                Province     = updateCustomer.DeliveryAddress.Province,
                                Municipality = updateCustomer.DeliveryAddress.Municipality,
                                CP           = updateCustomer.DeliveryAddress.CP
                            };

                            this._db.Addresses.Add(_address);                
                        }
                        await this._db.SaveChangesAsync();                        
                    }

                    return new RESTMessage {
                        Message      = "Data del cliente actualizados ok",
                        Errors       = null,
                        CustomerInfo = updateCustomer,
                        Token        = null,
                        Data         = null
                    };                  
                }
                else
                {
                    return new RESTMessage {
                        Message      = "*Fallo al actualizar los datos personales del cliente",
                        Errors       = new List<String> { "*Fallo en servidor cuando modificamos datos cliente en bd" },
                        CustomerInfo = null,
                        Token        = null,
                        Data         = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new RESTMessage {
                    Message      = "*Fallo al modificar datos del cliente",
                    Errors       = new List<String> { "*Fallo en direccion", "*Fallo en cliente" },
                    CustomerInfo = null,
                    Token        = null,
                    Data         = null
                };
            }
        }
        #endregion

        #region ==========FINALIZE ORDER==========
        [HttpPost]
        public RESTMessage SaveCustomerOrder([FromBody] Order customerOrder)
        {
            IDbContextTransaction transaction = this._db.Database.BeginTransaction();

            try
            {
                this._db.CustomerOrders.Add(customerOrder);

                customerOrder.OrderList.ForEach((OrderItem item) => {
                    //------save in ordersitems table------
                    item.OrderId = customerOrder.OrderId;
                    this._db.OrderItems.Add(item);

                    //------trigger: update stock------
                    StockProduct _stockItem = this._db.StockProducts.Where(row => row.ProductId == item.ProductId && row.Size == item.Size).FirstOrDefault();
                    _stockItem.Stock -= 1;

                    this._db.SaveChanges();
                });

                this._db.SaveChanges();

                transaction.Commit();

                return new RESTMessage() {
                    Message = "Pedido almacenado en bd ok",
                    Errors = null,
                    CustomerInfo = null,
                    Token = null,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new RESTMessage() {
                    Message = ex.Message,
                    Errors = new List<string> { "*Fallo al almacenar el pedido en la bd" },
                    CustomerInfo = null,
                    Token = null,
                    Data = customerOrder
                };
            }
        }
        #endregion
    }
}
