using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Botomag.BLL.Models;

namespace Botomag.BLL.Contracts
{
    public interface IBotService
    {
        Task<string> ProcessUpdateAsync(Guid botId, Stream stream);

        Task<IEnumerable<BotModel>> GetBotsByUserIdAsync(Guid userId, int? skip = null, int? take = null);

        Task<int> GetBotsCountByUserIdAsync(Guid userId);
    }
}
