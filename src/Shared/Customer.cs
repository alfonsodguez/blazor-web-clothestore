using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Zalandu.Shared
{
    public class Customer
    {
        #region ---------properties class----------
#nullable enable        
        public String      CustomerId          { get; set; }
        public Credentials CustomerCredentials { get; set; }
        
        [Required(ErrorMessage      = "*Nombre obligatorio")]
        [MaxLength(50, ErrorMessage = "*El nombre no puede exceder los 50 caracteres")]
        [DataType(DataType.Text)]
        public String Name { get; set; }

        [Required(ErrorMessage      = "*Apellidos obligatorios")]
        [MaxLength(50, ErrorMessage = "*Los apellidos no pueden exceder los 50 caracteres")]
        [DataType(DataType.Text)]
        public String Surname { get; set; }

        [Required(ErrorMessage                    = "*Fecha de nacimiento obligatoria")]
        [DataType(DataType.DateTime, ErrorMessage = "*Formato fecha invalido")]
        public DateTime Birth { get; set; }

        [MaxLength(10, ErrorMessage = "*DNI invalido: 12345678-W")]
        [MinLength(9,  ErrorMessage = "*DNI invalido: 12345678W")]
        public String? Dni { get; set; } 

        [NotMapped]
        public Address? DeliveryAddress { get; set; }  
        [NotMapped]
        public Order? CurrentOrder { get; set; }
        [NotMapped]
        public List<Order>? HistoryOrder { get; set; }
#nullable disable
        #endregion

        #region ---------constructor----------
        public Customer()
        {
            this.CustomerId          = Guid.NewGuid().ToString();
            this.CustomerCredentials = new Credentials();
            this.CurrentOrder        = new Order
            {
                StatusOrder = "en curso"                
            };
            this.HistoryOrder = new List<Order>();
        }
        #endregion

        #region ---------internal class-----------
        public class Credentials
        {
            [Required(ErrorMessage        = "*Email obligatorio")]
            [EmailAddress(ErrorMessage    = "*Formato de email invalido")]
            [StringLength(50,ErrorMessage = "*No se puede exceder un max de 50 caracteres")]
            public String Email { get; set; }

            [Required(ErrorMessage                            = "*Password obligatoria")]            
            [StringLength(50, MinimumLength = 8, ErrorMessage = "*Se requieren min 8 caracteres y como max 50")]             
            [DataType(DataType.Password, ErrorMessage         = "*Formato de Password invalido")]
            public String Password { get; set; }

            [Required(ErrorMessage                            = "*ConfirmPassword obligatoria")]
            [StringLength(50, MinimumLength = 8, ErrorMessage = "*Se requieren min 8 caracteres y como max 50")]
            [Compare("Password",ErrorMessage                  = "*La Password y ConfirmPassword no coinciden")]
            [DataType(DataType.Password)]       
            public String ConfirmPassword { get; set; }
#nullable enable            
            public String? HashPassword { get; set; }
        }
        #endregion
    }
}
