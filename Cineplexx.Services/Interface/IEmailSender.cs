using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cineplexx.Services.Interface
{
    public interface IEmailSender
    {
        Task SendEmail();
    }
}
