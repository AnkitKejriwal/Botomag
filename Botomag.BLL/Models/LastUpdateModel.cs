using System;

namespace Botomag.BLL.Models
{
    public class LastUpdateModel : BaseModel<Guid>
    {
        public int ChatId { get; set; }

        public int UpdateId { get; set; }

        public Guid BotId { get; set; }

        public BotModel Bot { get; set; }

        public int CurrentState { get; set; }
    }
}
