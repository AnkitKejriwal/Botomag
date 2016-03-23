using System;

namespace Botomag.DAL.Model
{
    public class Fight : BaseEntity<Guid>
    {
        public DateTime Date { get; set; }

        public Guid OrganizationId { get; set; }

        public virtual Organization Organization { get; set; }

        public Guid BetTypeId { get; set; }

        public virtual BetType BetType { get; set; }

        public string Bet { get; set; }

        public decimal Factor { get; set; }

    }
}
