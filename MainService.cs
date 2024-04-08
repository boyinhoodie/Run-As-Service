using System;
using System.Globalization;
using System.IO;
using System.ServiceProcess;

namespace RunAsService
{
    partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            File.AppendAllText("D:\\log.txt", $"{DateTime.Now.ToString(CultureInfo.InvariantCulture)}服务已停止……\r\n**********************************************************************\r\n");
        }
    }
}
