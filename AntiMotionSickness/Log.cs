using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiMotionSickness
{
    internal class Log
    {
        public static void Write(string message)
        {
            string currentDirectoryPath = Environment.CurrentDirectory.ToString();
            string DirPath = System.IO.Path.Combine(currentDirectoryPath, "Logs");
            string FilePath = DirPath + @"\Log_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);
            try
            {
                if (!di.Exists)
                {
                    di.Create();
                    using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
                    {
                        sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + message);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        sw.WriteLine("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + message);
                        sw.Close();
                    }
                }
            }
            catch
            {

            }
        }
    }
}
