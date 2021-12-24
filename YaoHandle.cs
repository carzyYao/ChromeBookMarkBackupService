using ChromeBookMarkBackupService;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;


namespace ChromeBookMarkBackupWindowsService
{
    public class YaoHandle
    {
        public static void PrintLog(string message)
        {
            string LogFilePath = GetSetting("TestLogFilePath");
            using (StreamWriter sw = File.AppendText(LogFilePath))
            {
                //WriteLine為換行 
                sw.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " ----> ");
                sw.WriteLine(message);
                sw.WriteLine("");
            }
        }


        /// <summary>
        /// 取得app.config AppSettings paramaters.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSetting(string key)
        {
            string value;
            try
            {
                Assembly executingAssembly = Assembly.GetAssembly(typeof(ProjectInstaller));
                string targetDir = executingAssembly.Location;
                Configuration config = ConfigurationManager.OpenExeConfiguration(targetDir);
                value = config.AppSettings.Settings[key].Value.ToString();
            }
            catch (Exception ex)
            {
                PrintLog(ex.ToString());
                return "";
            }

            return value;
        }

        public static void AddConfigurationFileDetails(Dictionary<string, string> Conditions, InstallContext Context)
        {
            try
            {
                // Get the path to the executable file that is being installed on the target computer  
                string assemblypath = Context.Parameters["assemblypath"];
                string appConfigPath = assemblypath + ".config";

                // Write the path to the app.config file  
                XmlDocument doc = new XmlDocument();
                doc.Load(appConfigPath);

                XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                    if (node.Name == "configuration")
                        configuration = node;

                if (configuration != null)
                {
                    //MessageBox.Show("configuration != null");
                    // Get the ‘appSettings’ node  
                    XmlNode settingNode = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                    {
                        if (node.Name == "appSettings")
                            settingNode = node;
                    }

                    if (settingNode != null)
                    {
                        //MessageBox.Show("settingNode != null");
                        //Reassign values in the config file  
                        foreach (XmlNode node in settingNode.ChildNodes)
                        {
                            //MessageBox.Show("node.Value = " + node.Value);  
                            if (node.Attributes == null)
                                continue;
                            XmlAttribute attribute = node.Attributes["value"];
                            //MessageBox.Show("attribute != null ");  
                            //MessageBox.Show("node.Attributes['value'] = " + node.Attributes["value"].Value);  
                            if (node.Attributes["key"] != null)
                            {
                                //MessageBox.Show("node.Attributes['key'] != null ");  
                                //MessageBox.Show("node.Attributes['key'] = " + node.Attributes["key"].Value);
                                string nodeAttrKeyValue = node.Attributes["key"].Value;
                                if (Conditions.ContainsKey(nodeAttrKeyValue))
                                {
                                    attribute.Value = Conditions[nodeAttrKeyValue];
                                    ////如果有路徑字眼 做路徑處理
                                    //if (nodeAttrKeyValue.ToLower().Trim().Contains("path"))
                                    //{
                                    //    attribute.Value = GetPathValue(Conditions[nodeAttrKeyValue]);
                                    //}
                                    //else
                                    //{
                                    //    attribute.Value = Conditions[nodeAttrKeyValue];
                                    //}

                                }
                            }
                        }
                    }
                    doc.Save(appConfigPath);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 路徑資料加斜線
        /// </summary>
        /// <param name="rPath"></param>
        /// <returns></returns>
        public static string GetPathValue(string rPath)
        {
            return rPath.Replace("\\", "\\\\");
        }
    }

}
