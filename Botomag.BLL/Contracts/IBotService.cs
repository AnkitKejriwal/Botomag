using System;
using System.IO;

namespace Botomag.BLL.Contracts
{
    public interface IBotService
    {
        string ProcessUpdate(Guid botId, Stream stream);
    }
}
