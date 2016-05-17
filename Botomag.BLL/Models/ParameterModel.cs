using System;

using Botomag.DAL.Model;

namespace Botomag.BLL.Models
{
    public class ParameterModel : BaseModel<Guid>
    {
        public string Expression { get; set; }

        public ParameterTypes Type { get; set; }

        public ResponseModel Response { get; set; }

        public Guid CommandId { get; set; }

        public CommandModel Command { get; set; }
    }
}
