using System;
using System.IO;

namespace Botomag.BLL.Contracts
{
    public interface IBotService
    {
        object ProcessUpdate(string token, Stream stream);
    }
}
