using Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Arbetsprov.Startup))]
namespace Arbetsprov
{
    public class Startup
    {
        /// <summary>
        /// Initiate SignalR. 
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}