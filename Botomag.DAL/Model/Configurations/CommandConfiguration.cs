﻿using System;
using System.Data.Entity.ModelConfiguration;

namespace Botomag.DAL.Model.Configurations
{
    public class CommandConfiguration : BaseConfiguration<Command, Guid>
    {
        public CommandConfiguration()
        {
            Property(n => n.Name).IsRequired();
            HasRequired(n => n.Bot).WithMany(n => n.Commands).HasForeignKey(n => n.BotId);
            HasMany(n => n.Parameters).WithRequired(n => n.Command).HasForeignKey(n => n.CommandId);
            HasOptional(n => n.InvalidParameterResponse).WithOptionalPrincipal();
        }
    }
}
