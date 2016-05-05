using System;
using AutoMapper;

using Botomag.DAL.Model;
using Botomag.BLL.Models;

namespace Botomag.BLL.Infrastructure
{
    public class AutomapperConfig
    {
        public static void Configuration(IMapperConfiguration config)
        {
            config.CreateMap<UserModel, User>().
                IncludeBase<BaseModel<Guid>, BaseEntity<Guid>>();
        }
    }
}
