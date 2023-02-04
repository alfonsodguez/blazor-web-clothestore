using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zalandu.Server.Models.Interfaces
{
    public interface IEmailClient
    {
        #nullable enable
        void SendEmail(String toEmailClient, String subject, String body, String? attachedName);
        #nullable disable
    }
}
