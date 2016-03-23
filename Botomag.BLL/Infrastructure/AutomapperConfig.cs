﻿using System;
using AutoMapper;

using Botomag.DAL.Model;
using Botomag.BLL.Model;

namespace Botomag.BLL.Infrastructure
{
    public class AutomapperConfig
    {
        public static void Configuration(IMapperConfiguration config)
        {
            config.CreateMap<BetType, BetTypeModel>();

            config.CreateMap<Organization, OrganizationModel>();

            config.CreateMap<Fight, FightModel>();
        }
    }
}
