using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.ServiceProcess;

namespace RunAsService
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "s")
            {
                var servicesToRun = new ServiceBase[]
                {
                    new MainService(),
                };
                ServiceBase.Run(servicesToRun);
            }
            else
            {
                Console.WriteLine("这是Windows应用程序");
                Console.WriteLine("请选择，[1]安装服务 [2]卸载服务 [3]退出");
                var rs = int.Parse(Console.ReadLine() ?? string.Empty);
                switch (rs)
                {
                    case 1:
                        ////取当前可执行文件路径，加上"s"参数，证明是从windows服务启动该程序
                        var processModule = Process.GetCurrentProcess().MainModule;
                        if (processModule != null)
                        {
                            var path = processModule.FileName + " s";
                            Process.Start("sc", "create myserver binpath= \"" + path + "\" displayName= A我的服务 start= auto");
                        }
                        Process.Start("sc", "Start myserver");

                        #region 启动cmd

                        // TODO：读取外部配置
                        var exePaths = new[]
                        {
                            //@"C:\Windows\system32\cmd.exe",
                            //@"C:\Windows\system32\cmd.exe"
                            @"D:\Codes\RunAsService\bin\Debug\run.bat"
                        };
                        foreach (var exePath in exePaths)
                        {
                            var startInfo = new ProcessStartInfo
                            {
                                FileName = exePath,
                                Verb = "runas", // 以管理员权限运行
                                UseShellExecute = true, // 使用操作系统的 Shell 来运行进程（默认true，要重定向 IO流，Process对象必须将 UseShellExecute属性设置为false）
                                //RedirectStandardInput = true, // 标准输入流的重定向，重定向至Process，我们可以通过Process.StandardInput.WriteLine将数据写入标准流
                                //RedirectStandardOutput = true, // 与RedirectStandardInput相反，这是标准输出流的重定向，我们可以通过Process.RedirectStandardOutput.ReadLine等方法读取标准流数据
                                //RedirectStandardError = true,
                                CreateNoWindow = false, // 如果需要隐藏窗口，设置为 true 就不显示窗口
                                //StandardOutputEncoding = Encoding.UTF8,
                                //Arguments = "/K " + str + " &exit",
                            };

                            try
                            {
                                Process.Start(startInfo);
                                File.AppendAllText("D:\\log.txt", $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)}【{exePath}】已启动……\r\n");
                            }
                            catch (Exception ex)
                            {
                                File.AppendAllText("D:\\log.txt", $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)}【{exePath}】启动失败。{ex.Message}……\r\n");
                            }
                        }

                        #endregion

                        Console.WriteLine("安装成功");
                        Console.Read();
                        break;
                    case 2:
                        Process.Start("sc", "Stop myserver");
                        Process.Start("sc", "delete myserver");
                        Console.WriteLine("卸载成功");
                        Console.Read();
                        break;
                    case 3: break;
                }
            }
        }
    }
}
