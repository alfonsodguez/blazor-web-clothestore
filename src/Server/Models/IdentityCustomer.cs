using System;
using Microsoft.AspNetCore.Identity;

namespace Zalandu.Server.Models
{
    public class IdentityCustomer : IdentityUser
    {
        public String   Name    { get; set; }
        public String   Surname { get; set; }
        public String   Dni     { get; set; }
        public DateTime Birth   { get; set; }
    }
}
