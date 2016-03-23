using System;

namespace Botomag.DAL.Model
{
    public class Organization : BaseEntity<Guid>
    {
        public string Title { get; set; }
    }
}
