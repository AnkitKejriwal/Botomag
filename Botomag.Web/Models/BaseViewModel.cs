using System;

namespace Botomag.Web.Models
{
    public class BaseViewModel<T> where T : struct
    {
        public T? Id { get; set; }
    }
}