using System;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Represent response on appropriate command of bot
    /// </summary>
    public class Response : BaseEntity<Guid>
    {
        public string Text { get; set; }

        public ParseModes? ParseMode { get; set; }

        public string Keyboard { get; set; }
    }
}
