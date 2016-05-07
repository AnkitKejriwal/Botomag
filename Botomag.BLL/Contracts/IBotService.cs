using System;
using System.IO;
using System.Threading.Tasks;

namespace Botomag.BLL.Contracts
{
    public interface IBotService
    {
        Task<string> ProcessUpdateAsync(Guid botId, Stream stream);
    }
}
