using System;
using System.ServiceProcess;
using System.Configuration;
using System.IO;

namespace MyApp {

    class Util {
        public static void Write(string value) {
            File.WriteAllText(@"Z:\wk\Hello.txt", value);
        }
    }

    class MyService : ServiceBase {
        protected override void OnStart(string[] args) {
            Util.Write("OnStart");
            base.OnStart(args);
        }
        protected override void OnStop() {
            Util.Write("OnStop");
            base.OnStop();
        }
    }

    class Program {

        static void Main(string[] args) {
            Util.Write("Main");
        }
    }
}
