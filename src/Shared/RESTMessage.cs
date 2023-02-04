using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zalandu.Shared
{
    public class RESTMessage
    {
        public String       Message      { get; set; }
        public String       Token        { get; set; }
        public Customer     CustomerInfo { get; set; }
        public Object       Data         { get; set; }
        public List<String> Errors       { get; set; }
    }
}
