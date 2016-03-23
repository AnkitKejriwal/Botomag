using System;

namespace Botomag.BLL.Model
{
    public class FightModel : BaseModel<Guid>
    {
        public DateTime Date { get; set; }

        public Guid OrganizationId { get; set; }

        public OrganizationModel Organization { get; set; }

        public Guid BetTypeId { get; set; }

        public BetTypeModel BetType { get; set; }

        public string Bet { get; set; }

        public decimal Factor { get; set; }
    }
}
