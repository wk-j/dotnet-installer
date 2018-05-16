// #addin "wk.StartProcess";
#addin nuget:?package=wk.StartProcess&version=18.5.0

using PS = StartProcess.Processor;

Task("Build-Windows")
        .Does(() => {
                var project = "MyWeb.WindowsService";
                PS.StartProcess($"dotnet restore src/{project}");
                PS.StartProcess($"dotnet build   src/{project}");
        });

var  target = Argument("target", "default");
RunTarget(target);