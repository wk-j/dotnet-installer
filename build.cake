// #addin "wk.StartProcess";
#addin nuget:?package=wk.StartProcess&version=18.5.0

using PS = StartProcess.Processor;

var webProject = $"src/MyWeb/MyWeb.csproj";
var appProject = $"src/MyApp/MyApp.csproj";

Task("Publish-Web").Does(() => {
        DotNetCoreClean(webProject);
        DotNetCorePublish(webProject, new DotNetCorePublishSettings {
                OutputDirectory = "publish/MyWeb"
        });
});

Task("Publish-App").Does(() => {
        DotNetCoreClean(appProject);
        DotNetCorePublish(appProject, new DotNetCorePublishSettings {
                OutputDirectory = "publish/MyApp"
        });
});

Task("Build-Web")
        .IsDependentOn("Publish-Web")
        .Does(() => {
                DotNetCoreBuild("src/MyWeb.Installer");
});

Task("Build-App")
        .IsDependentOn("Publish-App")
        .Does(() => {
                DotNetCoreBuild("src/MyApp.Installer");
});


var  target = Argument("target", "default");
RunTarget(target);