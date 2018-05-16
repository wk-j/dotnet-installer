// #addin "wk.StartProcess";
#addin nuget:?package=wk.StartProcess&version=18.5.0

using PS = StartProcess.Processor;

var name = "MyWeb";
var project = $"src/{name}/{name}.csproj";

Task("Publish").Does(() => {
        DotNetCorePublish(project, new DotNetCorePublishSettings {
                OutputDirectory = "publish"
        });
});

Task("Build-Windows")
        .Does(() => {
                var project = "MyWeb.WindowsService";
                PS.StartProcess($"dotnet restore src/{project}");
                PS.StartProcess($"dotnet build   src/{project}");
        });

var  target = Argument("target", "default");
RunTarget(target);