using System.Data;
using System.Runtime.InteropServices.JavaScript;
using Wen.Model;
using static System.Collections.Specialized.BitVector32;

namespace Wen.Common
{
    public class PubConstant
    {
        private static string configPath = string.Empty;
        private static string res = string.Empty;
        static PubConstant()
        {
            configPath = $"{Environment.CurrentDirectory}\\Config\\config.ini";
        }

        /// <summary>  
        /// 获取数据库连接字符串  
        /// </summary>  
        public static DbCoreDb ConnectionString()
        {
            string section = "Database";
            var keyValues = new Dictionary<string, string>();
            var config = new DbCoreDb();
            if (!File.Exists(configPath))
                throw new FileNotFoundException("INI 文件未找到！");
            string[] lines = File.ReadAllLines(configPath);
            bool isTargetSection = false;

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                {
                    // 检查是否为目标节  
                    isTargetSection = (trimmedLine.Substring(1, trimmedLine.Length - 2) == section);
                    continue;
                }
                if (isTargetSection && trimmedLine.Contains("="))
                {
                    string[] parts = trimmedLine.Split(new[] { '=' }, 2);
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    keyValues[key] = value;
                }
            }

            // 映射到实体  
            if (keyValues.TryGetValue("DatabaseType", out string? databaseType))
                config.DatabaseType = databaseType ?? string.Empty;

            if (keyValues.TryGetValue("DatabaseHost", out string? databaseHost))
                config.DatabaseHost = databaseHost ?? string.Empty;

            if (keyValues.TryGetValue("DatabasePort", out string? databasePort))
                config.DatabasePort = databasePort ?? string.Empty;

            if (keyValues.TryGetValue("DatabaseName", out string? databaseName))
                config.DatabaseName = databaseName ?? string.Empty;

            if (keyValues.TryGetValue("DatabaseUser", out string? databaseUser))
                config.DatabaseUser = databaseUser ?? string.Empty;

            if (keyValues.TryGetValue("DatabasePassword", out string? databasePassword))
                config.DatabasePassword = databasePassword ?? string.Empty;

            if (keyValues.TryGetValue("SshHost", out string? sshHost))
                config.SshHost = sshHost ?? string.Empty;

            if (keyValues.TryGetValue("SshPort", out string? sshPort))
                config.SshPort = sshPort ?? string.Empty;

            if (keyValues.TryGetValue("SshUser", out string? sshUser))
                config.SshUser = sshUser ?? string.Empty;

            if (keyValues.TryGetValue("SshPassword", out string? sshPassword))
                config.SshPassword = sshPassword ?? string.Empty;

            if (keyValues.TryGetValue("SshLocalPort", out string? sshLocalPort))
                config.SshLocalPort = sshLocalPort ?? string.Empty;

            return config;
        }


        public static string DalJk
        {

            get
            {


                return ReadIni.ReadIniData("Database", "DAL", "", configPath);

            }


        }

        public static int GetModelCache
        {
            get
            {
                string modelCache = ReadIni.ReadIniData("Database", "ModelCache", "", configPath);
                if (int.TryParse(modelCache, out int cacheDuration))
                {
                    return cacheDuration;
                }
                else
                {
                    return 0; // 或者返回一个默认值
                }
            }
        }
    }
}
