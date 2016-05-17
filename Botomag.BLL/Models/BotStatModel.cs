using System;

namespace Botomag.BLL.Models
{
    public class BotStatModel : BaseModel<Guid>
    {
        public BotModel Bot { get; set; }

        public long Requests { get; set; }
    }
}
