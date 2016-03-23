using System;

namespace Botomag.BLL.Model
{
    public class BaseModel<T> where T : struct
    {
        T Id { get; set; }
    }
}
