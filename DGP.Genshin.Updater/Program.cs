using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace DGP.Genshin.Updater
{
    /// <summary>
    /// 主入口点
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            // fix unable to find file issue
            if (Path.GetDirectoryName(AppContext.BaseDirectory) is string exePath)
            {
                Environment.CurrentDirectory = exePath;
            }

            bool hasArgs = args.Length > 0 && args[0] == "UpdateInstall";
            bool hasPackage = File.Exists("../Package.zip");

            switch ((hasArgs, hasPackage))
            {
                case (true, true):
                    AutoUpdate();
                    break;
                case (false, true):
                    ManualUpdate();
                    break;
                case (_, false):
                    Console.WriteLine($"未在 {Directory.GetParent(AppContext.BaseDirectory)} 找到更新文件 'Package.zip'");
                    Console.ReadKey();
                    break;
            }
        }

        private static void ManualUpdate()
        {
            Console.WriteLine("检测到安装包存在，是否手动安装？(Y/其他任何按键)");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key is ConsoleKey.Y)
            {
                AutoUpdate();
            }
        }

        private static void AutoUpdate()
        {
            Console.WriteLine("准备开始安装...");
            WaitForProcessExit("DGP.Genshin");
            Console.Clear();
            using (ZipArchive archive = ZipFile.OpenRead("../Package.zip"))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    try
                    {
                        if (entry.FullName.EndsWith("/"))
                        {
                            string directoryName = entry.FullName[0..^1];
                            Console.WriteLine($"创建目录:{directoryName}");
                            Directory.CreateDirectory($"../{directoryName}");
                        }
                        else
                        {
                            Console.WriteLine($"提取文件:{entry.FullName}");
                            entry.ExtractToFile($"../{entry.FullName}", true);
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            Console.WriteLine("正在启动Snap Genshin");
            Process.Start("../DGP.Genshin.exe");
        }

        private static void WaitForProcessExit(string processName)
        {
            Process[] ps = Process.GetProcessesByName(processName);
            if (ps.Length > 0)
            {
                Process p = ps[0];
                p.CloseMainWindow();

                Console.WriteLine($"等待 {processName} 退出中...(可能需要手动退出)");
                p.WaitForExit();
            }
        }
    }
}