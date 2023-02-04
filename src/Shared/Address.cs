using System;
using System.ComponentModel.DataAnnotations;

namespace Zalandu.Shared
{
    public class Address
    {
        #region ----------properties class------------
#nullable enable
        public String AddressId  { get; set; } 
        public String CustomerId { get; set; }

        [Required(ErrorMessage = "*Telefono obligatorio")]
        [Phone(ErrorMessage    = "*Formato de tlfno invalido: 000 11 22 33")]
        [RegularExpression("^[0-9]{3} [0-9]{2} [0-9]{2} [0-9]{2}$", ErrorMessage = "*Formato de tlfno invalido: 000 11 22 33")]
        public String? PhoneNumber { get; set; }

        [Required(ErrorMessage       = "*Campo requerido")]
        [MaxLength(100, ErrorMessage = "El nombre de vía no puede exceder los 100 caracteres")]
        public String RoadName { get; set; } 

        [Required(ErrorMessage = "*Campo requerido")]
        [StringLength(6, MinimumLength = 1)]
        public String RoadNumber { get; set; } 

        [StringLength(6, MinimumLength = 1)]
        public String? Block { get; set; } 

        [StringLength(6, MinimumLength = 1)]
        public String? Floor { get; set; }

        [MaxLength(6, ErrorMessage = "No puede exceder de un max de 6 caracteres")]
        public String? Door { get; set; }

        [Required(ErrorMessage = "*Campo requerido")]
        [StringLength(50, MinimumLength = 1)]
        public String Province { get; set; }

        [Required(ErrorMessage = "*Campo requerido")]
        [StringLength(50, MinimumLength = 1)]
        public String Municipality { get; set; }

        [Required(ErrorMessage = "*Campo requerido")]
        [DataType(DataType.PostalCode)]
        public String CP { get; set; }
#nullable disable
        #endregion

        #region ----------constructor----------
        public Address()
        {
            this.RoadName    = "";
            this.RoadNumber  = "";
            this.PhoneNumber = "";
        }
        #endregion
    }
}