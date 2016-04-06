using System;

namespace TelegramBot.Core.Types.ResponseTypes
{
    public class UpdateResponse : BaseResponse
    {
        public int? Update_Id { get; set; }

        public MessageResponse Message { get; set; }
    }
}
