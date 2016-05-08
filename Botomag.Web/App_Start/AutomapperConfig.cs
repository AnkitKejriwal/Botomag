using System;
using AutoMapper;

using Botomag.Web.Models;
using Botomag.Web.Models.Account;
using Botomag.BLL.Models;

namespace Botomag.Web
{
    public class AutomapperConfig
    {
        public static void Configuration(IMapperConfiguration config)
        {
            config.CreateMap<RegisterViewModel, UserModel>().
                ForMember(trg => trg.PasswordHash, opt => opt.Ignore()).
                IncludeBase<BaseViewModel<Guid>, BaseModel<Guid>>();

            config.CreateMap<LoginViewModel, UserModel>().
                ForMember(trg => trg.PasswordHash, opt => opt.Ignore()).
                IncludeBase<BaseViewModel<Guid>, BaseModel<Guid>>();
        }
    }
}