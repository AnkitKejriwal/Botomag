using System;
using System.Threading.Tasks;

namespace Botomag.BLL.Contracts
{
    public interface IMailService
    {
        Task SendMessageAsync(string to, string subject, string body);
    }
}
