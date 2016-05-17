using System;
using TelegramBot.Core.Types.RequestTypes;

namespace Botomag.BLL.Models
{
    public class ResponseModel : BaseModel<Guid>
    {
        public string Text { get; set; }

        public ParseModes? ParseMode { get; set; }

        public string Keyboard { get; set; }
    }
}
