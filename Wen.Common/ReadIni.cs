using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wen.Common
{
    public class ReadIni
    {
        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion
        public static string ReadIniData(string Section, string Key, string NoText, string iniFilePath)//读取INI文件
        {
            string str = System.Environment.CurrentDirectory;//获取当前文件目录
            //ini文件路径
            string str1 = "" + str + "\\Config\\config.ini";
            try
            {
                if (File.Exists("" + str1 + ""))
                {
                    StringBuilder temp = new StringBuilder(1024);
                    GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                    return temp.ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch //(Exception ex)
            {

                return "";
            }
        }



        /// <summary>
        /// 写入INI的方法section=，key=，value=，path=
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">返回键值</param>
        public static void INIWrite(string key, string value)
        {

            string str = System.Environment.CurrentDirectory;//获取当前文件目录
            //ini文件路径
            string path = "" + str + "\\config.ini";

            WritePrivateProfileString("Database", key, value, path);
        }
        public static void INIWrite(string Section, string key, string value, string path)
        {

            string str = System.Environment.CurrentDirectory;//获取当前文件目录



            WritePrivateProfileString(Section, key, value, path);
        }
    }
}
