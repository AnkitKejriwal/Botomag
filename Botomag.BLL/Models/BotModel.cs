using System;
using System.Collections.Generic;

namespace Botomag.BLL.Models
{
    public class BotModel : BaseModel<Guid>
    {
        public string Token { get; set; }

        public IEnumerable<CommandModel> Commands { get; set; }

        public IEnumerable<LastUpdateModel> LastUpdates { get; set; }

        public BotStatModel BotStat { get; set; }

        public ResponseModel InvalidCommandResponse { get; set; }

        public string Name { get; set; }

        public DateTime? LastUpdate { get; set; }

        public Guid UserId { get; set; }

        public UserModel User { get; set; }
    }
}
