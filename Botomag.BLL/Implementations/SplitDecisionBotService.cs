using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Botomag.DAL;
using Botomag.DAL.Model;
using Botomag.BLL.Model;
using Botomag.BLL.Contracts;

namespace Botomag.BLL.Implementations
{
    public class SplitDecisionBotService : BaseService, ISplitDecisionBotService
    {
        public SplitDecisionBotService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

        public IEnumerable<FightModel> GetFights(decimal factor)
        {
            Repository<Fight, Guid> repo = _unitOfWork.GetRepository<Fight, Guid>();

            IEnumerable<Fight> fights = repo.Get();

            decimal delta = fights.Min(n => Math.Abs(n.Factor - factor));

            IEnumerable<Fight> result = from fight in fights
                                        where Math.Abs(fight.Factor - factor) == delta
                                        select fight;

            return _mapper.Map<IEnumerable<FightModel>>(result);
        }

        public async Task<IEnumerable<FightModel>> GetFightsAsync(decimal factor)
        {
            Repository<Fight, Guid> repo = _unitOfWork.GetRepository<Fight, Guid>();

            IEnumerable<Fight> fights = await Task<IEnumerable<Fight>>.Factory.StartNew(() =>
                {
                    return repo.Get();
                });

            decimal delta = await Task<decimal>.Factory.StartNew(() =>
                {
                    return fights.Min(n => Math.Abs(n.Factor - factor));
                });

            IEnumerable<Fight> result = await Task<IEnumerable<Fight>>.Factory.StartNew(() =>
                {
                    return from fight in fights
                           where Math.Abs(fight.Factor - factor) == delta
                           select fight;
                });

            return _mapper.Map<IEnumerable<FightModel>>(result);
        }
    }
}
