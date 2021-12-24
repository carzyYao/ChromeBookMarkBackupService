using ChromeBookMarkBackupWindowsService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ChromeBookMarkBackupService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            int secNumber = 28800;//86400 一天, 28800 1/3天 
            var myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(TimerEvent);
            myTimer.Interval = secNumber * 1000; 
            myTimer.Start();

            YaoHandle.PrintLog("Run Backup: "+secNumber+"/times");
        }

        /// <summary>
        /// Timer要執行的事件
        /// </summary>
        public void TimerEvent(object sender, ElapsedEventArgs e)
        {
            string SourceFilePath = YaoHandle.GetSetting("SourceFilePath");
            string GoalFolderPath = YaoHandle.GetSetting("GoalFolderPath");

            //寫入現在時間測試服務結果
            YaoHandle.PrintLog("Run Backup ");
            //YaoHandle.PrintLog("SourceFilePath :" + SourceFilePath);
            //YaoHandle.PrintLog("GoalFolderPath :" + GoalFolderPath);

            string fileName = DateTime.Now.ToString("yyyy_MM_dd HH_mm_ss") + "_Bookmarks_back";
            File.Copy(SourceFilePath, Path.Combine(GoalFolderPath, fileName), true);

        }

        protected override void OnStop()
        {
            string path = YaoHandle.GetSetting("TestLogFilePath");
            //寫入現在時間測試服務結果
            File.AppendAllText(path, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
