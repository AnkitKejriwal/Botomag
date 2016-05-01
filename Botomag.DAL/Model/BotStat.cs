using System;
using System.Collections.Generic;

namespace Botomag.DAL.Model
{
    /// <summary>
    /// Represent bot statistic
    /// </summary>
    public class BotStat : BaseEntity<Guid>
    {
        public virtual Bot Bot { get; set; }

        public long Requests { get; set; }
    }
}
