using System;

namespace Botomag.BLL.Models
{
    public class BaseModel<T> where T : struct
    {
        public T Id { get; set; }
    }
}
