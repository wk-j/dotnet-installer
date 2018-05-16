using System.Linq;
using WixSharp;
using System.Collections.Generic;
using System;

using SIO = System.IO;

class Config {
    public string ProjectDir { set; get; }
    public string TargetDir { set; get; }
    public string InstallerName { set; get; }
    public string ToolLocation { set; get; }
}

class FileFilter {
    private readonly System.IO.DirectoryInfo TopDir;

    public string Root => TopDir.FullName;

    public FileFilter(string dir) {
        TopDir = new System.IO.DirectoryInfo(dir);
    }

    System.IO.FileInfo[] GetFiles(string pattern) {
        return TopDir.GetFiles(pattern);
    }

    System.IO.FileInfo[] GetFiles(string pattern, string subDir) {
        return new System.IO.DirectoryInfo(System.IO.Path.Combine(TopDir.FullName, subDir)).GetFiles(pattern);
    }


    File ToWixFile(System.IO.FileInfo file) {
        return new File(file.FullName) {
            Permissions = new FilePermission[] {
                    new FilePermission("Everyone", GenericPermission.All)
                }
        };
    }

    public File GetFile(string path) {
        var full = System.IO.Path.Combine(TopDir.FullName, path);
        return new File(full);
    }

    public File[] GetWixFiles(string pattern) {
        return GetFiles(pattern).Select(ToWixFile).ToArray();
    }

    public File[] GetWixFiles(string pattern, string subDir) {
        return GetFiles(pattern, subDir).Select(ToWixFile).ToArray();
    }
}

class Utiltiy {
    public static void CreateShortcut(File file) {
        file.Shortcuts = new FileShortcut[] {
                new FileShortcut(file.Id, "INSTALLDIR"),
                new FileShortcut(file.Id, "%Desktop%")
            };
    }
}

class Program {
    static Dir GetTopDir(FileFilter filter) {
        var exe = filter.GetWixFiles("*.exe");
        var json = filter.GetWixFiles("*.json");
        var dll = filter.GetWixFiles("*.dll");
        var config = filter.GetWixFiles("*.config");
        // var bat = filter.GetWixFiles("*.bat");

        config.ForEach(x => x.SetComponentPermanent(true));

        var files = new List<File>();
        files.AddRange(exe);
        files.AddRange(config);
        files.AddRange(dll);
        files.AddRange(json);
        //files.AddRange(md5);

        var dir = new Dir(".", files.ToArray());
        return dir;
    }

    static WixEntity[] GetEasyCaptureStructures(string root) {
        var filter = new FileFilter(root);

        var dirs = new WixEntity[] {
                GetTopDir(filter),
                //new Dir("x86", filter.GetWixFiles("*.dll", "x86")),
                //new Dir("x64", filter.GetWixFiles("*.dll", "x64")),
                //new Dir("logs", new File [] { }),
                //new Dir("web", new Files(System.IO.Path.Combine(root, @"web\*.*")))
            };

        return dirs.ToArray();
    }

    static string GetProjectDir() {
        var projectDir = @"Z:\GitHub\DotNetInstaller\publish\MyWeb";
        return projectDir;
    }

    static string GetVertion() {
        var version = "0.2.0";
        return version;
    }

    static Config CreateConfig() {
        var projectDir = GetProjectDir();
        var version = GetVertion();
        var installerName = $"MyWeb.{version}";
        var config = new Config {
            ProjectDir = projectDir,
            InstallerName = installerName,
            TargetDir = $"%ProgramFiles%\\BCircle\\MyWeb",
            ToolLocation = @"C:\Program Files (x86)\WiX Toolset v3.11\bin"
        };
        return config;
    }

    static void Main(string[] args) {
        var config = CreateConfig();
        var version = GetVertion();

        Environment.SetEnvironmentVariable("WIXSHARP_WIXDIR", config.ToolLocation, EnvironmentVariableTarget.Process);

        var structure = GetEasyCaptureStructures(config.ProjectDir);
        var topDir = new Dir(config.TargetDir, structure);

        //var action = new SetPropertyAction("IDIR", "[INSTALLDIR]");
        var project = new Project(config.InstallerName, topDir);
        project.UI = WUI.WixUI_InstallDir;
        // project.LicenceFile = System.IO.Path.Combine(config.ProjectDir, "LICENSE.rtf");
        // project.BannerImage = "logo.jpg";

        project.UpgradeCode = Guid.Parse("a6ac098c-107f-4eb5-b485-48fbd83dee4e");
        project.ProductId = Guid.NewGuid();
        project.Version = new Version(version);
        project.MajorUpgrade = new MajorUpgrade { AllowSameVersionUpgrades = true, DowngradeErrorMessage = "Higher version already installed" };


        var exe = project.AllFiles.Where(x => x.Id == "MyWeb.exe").FirstOrDefault();
        exe.ServiceInstaller = new ServiceInstaller() {
            Id = config.InstallerName,
            Name = config.InstallerName,
            Description = config.InstallerName
        };

        Compiler.BuildMsi(project);
    }
}