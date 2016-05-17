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
                ForMember(trg => trg.Bots, opt => opt.Ignore()).
                IncludeBase<BaseModel<Guid>, BaseEntity<Guid>>().
                ReverseMap().
                ForMember(trg => trg.Password, opt => opt.Ignore()).
                IncludeBase<BaseEntity<Guid>, BaseModel<Guid>>();

            config.CreateMap<Bot, BotModel>().
                IncludeBase<BaseEntity<Guid>, BaseModel<Guid>>();

            config.CreateMap<LastUpdate, LastUpdateModel>().
                IncludeBase<BaseEntity<Guid>, BaseModel<Guid>>();

            config.CreateMap<BotStat, BotStatModel>().
                IncludeBase<BaseEntity<Guid>, BaseModel<Guid>>();

            config.CreateMap<Response, ResponseModel>().
                IncludeBase<BaseEntity<Guid>, BaseModel<Guid>>();

            config.CreateMap<Parameter, ParameterModel>().
                IncludeBase<BaseEntity<Guid>, BaseModel<Guid>>();

            config.CreateMap<Command, CommandModel>().
                IncludeBase<BaseEntity<Guid>, BaseModel<Guid>>();
        }
    }
}
