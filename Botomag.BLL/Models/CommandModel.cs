using System;
using System.Collections.Generic;

namespace Botomag.BLL.Models
{
    public class CommandModel : BaseModel<Guid>
    {
        public Guid BotId { get; set; }

        public BotModel Bot { get; set; }

        public string Name { get; set; }

        public int CurrentState { get; set; }

        public int NextState { get; set; }

        public IEnumerable<ParameterModel> Parameters { get; set; }

        public ResponseModel InvalidParameterResponse { get; set; }
    }
}
