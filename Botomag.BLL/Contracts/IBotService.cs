using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Botomag.BLL.Contracts
{
    public interface IBotService
    {
        Task<string> ProcessUpdateAsync(Guid botId, Stream stream);

        Task<IEnumerable<string>> GetBotNamesByUserIdAsync(Guid userId);
    }
}
