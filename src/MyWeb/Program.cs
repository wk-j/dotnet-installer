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
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace MyWeb {
    public class Program {
        public static void Main(string[] args) {

            bool isService = true;
            if (Debugger.IsAttached || args.Contains("--console")) {
                isService = false;
            }

            var pathToContentRoot = Directory.GetCurrentDirectory();
            if (isService) {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            var webHostcArgs = args.Where(arg => arg != "--console").ToArray();

            var host = WebHost.CreateDefaultBuilder(webHostcArgs)
                .UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>()
                .Build();

            if (isService) {
                host.RunAsService();
            } else {
                host.Run();
            }
        }
    }
}
