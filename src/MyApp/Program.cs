﻿using System;
using System.ServiceProcess;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MyApp {

    public class Util {
        public static void Write(string value) {
            File.WriteAllText(@"C:\wk\Hello.txt", value);
        }
    }

    public class MyService : ServiceBase {
        protected override void OnStart(string[] args) {
            Util.Write("OnStart");

            Task.Run(() => {
                while (true) {
                    Thread.Sleep(1000);
                }
            });

            base.OnStart(args);

        }
        protected override void OnStop() {
            Util.Write("OnStop");
            base.OnStop();
        }
    }

    public class Program {

        static void Main(string[] args) {
            ServiceBase[] servicesToRun = new ServiceBase[] { new MyService() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
