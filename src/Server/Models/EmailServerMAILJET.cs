using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zalandu.Server.Models
{
    public class EmailServerMAILJET
    {
        public string ServerName { get; internal set; }
        public string APIKey     { get; internal set; }
        public string SecretKey  { get; internal set; }
    }
}
