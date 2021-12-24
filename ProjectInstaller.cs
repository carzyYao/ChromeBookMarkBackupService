using ChromeBookMarkBackupWindowsService;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChromeBookMarkBackupService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void BackupServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            //showParameters();
            Dictionary<string, string> conditions = new Dictionary<string, string>();
            conditions.Add("SourceFilePath", Context.Parameters["SourceFilePath"]);
            conditions.Add("GoalFolderPath", Context.Parameters["GoalFolderPath"]);
            YaoHandle.AddConfigurationFileDetails(conditions, Context);
        }

        private void showParameters()
        {
            StringBuilder sb = new StringBuilder();
            StringDictionary myStringDictionary = this.Context.Parameters;
            if (this.Context.Parameters.Count > 0)
            {
                foreach (string myString in this.Context.Parameters.Keys)
                {
                    sb.AppendFormat("String={0} Value= {1}\n", myString,
                    this.Context.Parameters[myString]);
                }
            }
            MessageBox.Show(sb.ToString());
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
