using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.ServiceProcess;

namespace MyWeb {

    public class Starter {
        public static IWebHost Start(String[] webHostcArgs) {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            var host = WebHost.CreateDefaultBuilder(webHostcArgs)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .Build();

            host.Run();
            return host;
        }
    }

    public class MyService : ServiceBase {
        private string[] args;
        private IWebHost host;

        public MyService(string[] args) => this.args = args;
        protected override void OnStart(String[] args) {
            Task.Run(() => {
                host = Starter.Start(args);
            });
            base.OnStart(args);
        }

        protected override void OnStop() {
            if (host != null) {
                host.StopAsync().GetAwaiter().GetResult();
            }
            base.OnStop();
        }

    }

    public class Program {
        public static void Main(string[] args) {
            var noConsole = args.Where(x => x != "--console").ToArray();
            if (args.Contains("--console")) {
                Starter.Start(noConsole);
            } else {
                ServiceBase[] servicesToRun = new ServiceBase[] { new MyService(noConsole) };
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}
