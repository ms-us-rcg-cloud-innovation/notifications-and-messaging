using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(NotificationFunctions.Startup))]

namespace NotificationFunctions
{
    public class Startup
        : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            throw new NotImplementedException();
        }
    }
}
