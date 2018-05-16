// #addin "wk.StartProcess";
#addin nuget:?package=wk.StartProcess&version=18.5.0

using PS = StartProcess.Processor;

var name = "MyApp";
var project = $"src/{name}/{name}.csproj";

Task("Restore").Does(() => {
        DotNetCoreRestore(project);
});

Task("Publish-Web").Does(() => {
        DotNetCorePublish(project, new DotNetCorePublishSettings {
                OutputDirectory = "publish/MyWeb"
        });
});
Task("Build").Does(() => {
        DotNetCoreBuild(project);
});

Task("Publish-App").Does(() => {
        DotNetCorePublish(project, new DotNetCorePublishSettings {
                OutputDirectory = "publish/MyApp"
        });
});

var  target = Argument("target", "default");
RunTarget(target);