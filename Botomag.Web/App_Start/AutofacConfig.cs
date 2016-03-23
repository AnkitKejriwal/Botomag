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
            string botToken = AppConfigHelper.GetValue<string>(AppConfigKeys.BotToken, n => n);
            BaseBotService botsrv = new BaseBotService(botToken);
            builder.RegisterInstance(botsrv).As<IBaseBotService>();

            UnitOfWork unitOfWork = new UnitOfWork();
            builder.RegisterInstance(unitOfWork).As<IUnitOfWork>();

            builder.RegisterType<SplitDecisionBotService>().As<ISplitDecisionBotService>();

            MapperConfiguration config = new MapperConfiguration(cfg => Botomag.BLL.Infrastructure.AutomapperConfig.Configuration(cfg));
            config.AssertConfigurationIsValid();
            IMapper mapper = config.CreateMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            // Set resolver
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}