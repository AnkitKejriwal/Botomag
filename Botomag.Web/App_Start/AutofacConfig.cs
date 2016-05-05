using System;
using System.Configuration;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;

using Botomag.BLL.Contracts;
using Botomag.BLL.Implementations;
using Botomag.DAL;
using Botomag.Web.Infrastructure;
using TelegramBot.Core.Services.Implementations;
using TelegramBot.Core.Services.Contracts;

namespace Botomag.Web
{
    public class AutofacConfig
    {
        public static void Configuration()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register services

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<BotService>().As<IBotService>();
            builder.RegisterType<MailService>().As<IMailService>();
            builder.RegisterType<TelegramBotService>().As<ITelegramBotService>();
            builder.RegisterType<UserService>().As<IUserService>();

            MapperConfiguration config = new MapperConfiguration(cfg => 
                {
                    Botomag.BLL.Infrastructure.AutomapperConfig.Configuration(cfg);
                    AutomapperConfig.Configuration(cfg);
                });
            config.AssertConfigurationIsValid();
            IMapper mapper = config.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            // Set resolver
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}