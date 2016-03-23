using System.Threading.Tasks;
using System.Collections.Generic;
using Botomag.BLL.Model;

namespace Botomag.BLL.Contracts
{
    public interface ISplitDecisionBotService
    {
        Task<IEnumerable<FightModel>> GetFightsAsync(decimal factor);

        IEnumerable<FightModel> GetFights(decimal factor);
    }
}
