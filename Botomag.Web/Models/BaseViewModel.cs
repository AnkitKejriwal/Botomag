using System;

namespace Botomag.Web.Models
{
    public class BaseViewModel<T> where T : struct
    {
        T? Id { get; set; }
    }
}