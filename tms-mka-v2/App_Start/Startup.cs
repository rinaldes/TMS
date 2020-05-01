﻿using System;
using System.Threading.Tasks;
using Microsoft.Owin.Cors;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(tms_mka_v2.App_Start.Startup))]

namespace tms_mka_v2.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}
