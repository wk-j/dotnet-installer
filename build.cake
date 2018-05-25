#addin "wk.StartProcess";
#addin "wk.ProjectParser";

using PS = StartProcess.Processor;
using ProjectParser;

Task("Publish-Web").Does(() => {
        var webProject = $"src/console/MyWeb/MyWeb.csproj";
        DotNetCoreClean(webProject);
        DotNetCorePublish(webProject, new DotNetCorePublishSettings {
                OutputDirectory = "publish/MyWeb"
        });
});

Task("Publish-App").Does(() => {
        var appProject = $"src/console/MyApp/MyApp.csproj";
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

Task("Publish-Ubuntu-App").Does(() => {
        var root = "publish/ubuntu-app";
        CleanDirectory(root);
        DotNetCorePublish("src/ubuntu/UbuntuApp", new DotNetCorePublishSettings {
                OutputDirectory = $"{root}/opt/ubuntu-app",
                Runtime = "ubuntu-x64",
                Framework = "netcoreapp2.0"
        });
});

Task("Create-App-Deb")
        .IsDependentOn("Publish-Ubuntu-App")
        .Does(() => {
                var info = Parser.Parse("src/ubuntu/UbuntuApp/UbuntuApp.csproj");
                var root = "publish/ubuntu-app";
                CreateDirectory($"{root}/DEBIAN");
                CreateDirectory($"{root}/etc/systemd/system");
                CopyFile("control/control", $"{root}/DEBIAN/control");
                CopyFile("control/ubuntuapp.service", $"{root}/etc/systemd/system/ubuntuapp.service");
                PS.StartProcess($"dpkg-deb --build {root} publish/ubuntu-app.{info.Version}.deb");
        });

// --- WEB

Task("Publish-Ubuntu-Web").Does(() => {
        var root = "publish/UbuntuWeb";
        CleanDirectory(root);
        DotNetCorePublish("src/ubuntu/UbuntuWeb", new DotNetCorePublishSettings {
                OutputDirectory = $"{root}/opt/UbuntuWeb",
                Runtime = "ubuntu-x64",
                Framework = "netcoreapp2.1"
        });
});

Task("Create-Web-Deb")
        .IsDependentOn("Publish-Ubuntu-Web")
        .Does(() => {
                var info = Parser.Parse("src/ubuntu/UbuntuWeb/UbuntuWeb.csproj");
                var root = "publish/UbuntuWeb";
                CreateDirectory($"{root}/DEBIAN");
                CreateDirectory($"{root}/etc/systemd/system");
                CopyFile("control/web/control", $"{root}/DEBIAN/control");
                CopyFile("control/web/UbuntuWeb.service", $"{root}/etc/systemd/system/UbuntuWeb.service");
                PS.StartProcess($"dpkg-deb --build {root} publish/UbuntuWeb.{info.Version}.deb");
        });
var  target = Argument("target", "default");
RunTarget(target);