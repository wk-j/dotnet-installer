// #addin "wk.StartProcess";
#addin nuget:?package=wk.StartProcess&version=18.5.0

using PS = StartProcess.Processor;

var name = "MyWeb";
var project = $"src/{name}/{name}.csproj";

Task("Restore").Does(() => {
        DotNetCoreRestore(project);
});

Task("Publish").Does(() => {
        DotNetCorePublish(project, new DotNetCorePublishSettings {
                OutputDirectory = "publish"
        });
});
Task("Build").Does(() => {
        DotNetCoreBuild(project);
});

Task("Run").Does(() => {

});

var  target = Argument("target", "default");
RunTarget(target);