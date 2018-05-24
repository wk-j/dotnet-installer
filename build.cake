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

Task("Publish-Ubuntu").Does(() => {
        var root = "publish/Ubuntu";
        CleanDirectory(root);
        DotNetCorePublish("ubuntu/UbuntuApp", new DotNetCorePublishSettings {
                OutputDirectory = $"{root}/opt/ubuntu-app",
                Runtime = "ubuntu-x64",
                Framework = "netcoreapp2.0"
        });
});

Task("Create-Deb")
        .IsDependentOn("Publish-Ubuntu")
        .Does(() => {
                var root = "publish/Ubuntu";
                CreateDirectory($"{root}/DEBIAN");
                CreateDirectory($"{root}/etc/systemd/system");
                CopyFile("control/control", $"{root}/DEBIAN/control");
                CopyFile("control/ubuntuapp.service", $"{root}/etc/systemd/system/ubuntuapp.service");
                PS.StartProcess($"dpkg-deb --build {root} ubuntu-app.deb");
        });

var  target = Argument("target", "default");
RunTarget(target);