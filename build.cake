// #addin "wk.StartProcess";
#addin nuget:?package=wk.StartProcess&version=18.5.0

using PS = StartProcess.Processor;

var webProject = $"src/MyWeb/MyWeb.csproj";
var appProject = $"src/MyApp/MyApp.csproj";

Task("Publish-Web").Does(() => {
        DotNetCorePublish(webProject, new DotNetCorePublishSettings {
                OutputDirectory = "publish/MyWeb"
        });
});

Task("Publish-App").Does(() => {
        DotNetCorePublish(appProject, new DotNetCorePublishSettings {
                OutputDirectory = "publish/MyApp"
        });
});

var  target = Argument("target", "default");
RunTarget(target);