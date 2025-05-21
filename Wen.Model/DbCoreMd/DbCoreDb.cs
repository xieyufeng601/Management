using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wen.Model
{
    /// <summary>
    /// 数据库连接信息
    /// </summary>
    [SugarTable("DbCoreDb")]
    public class DbCoreDb
    {
        public DbCoreDb()
        {
            
            DatabaseType = string.Empty;
            DatabaseHost = string.Empty;
            DatabasePort = string.Empty;
            DatabaseName = string.Empty;
            DatabaseUser = string.Empty;
            DatabasePassword = string.Empty;
            SshHost = string.Empty;
            SshPort = string.Empty;
            SshUser = string.Empty;
            SshPassword = string.Empty;
            SshLocalPort = string.Empty;
        }
        /// <summary>
        /// mysql |sqlserver| oracle | sqlite | postgresql 
        /// </summary>
        public string DatabaseType { get; set; }
        /// <summary>
        /// localhost
        /// </summary>
        public string DatabaseHost { get; set; }
        /// <summary>
        ///  3306
        /// </summary>
        public string DatabasePort { get; set; }
        /// <summary>
        /// yourdb
        /// </summary>
        public string DatabaseName { get; set; } 
        /// <summary>
        ///  username
        /// </summary>
        public string DatabaseUser { get; set; } 
        /// <summary>
        ///  password
        /// </summary>
        public string DatabasePassword { get; set; } 
        /// <summary>
        /// SSH隧道连接 null (default: no SSH)
        /// </summary>
        public string SshHost { get; set; } 
        /// <summary>
        ///  SSH隧道连接端口 22
        /// </summary>
        public string SshPort { get; set; }
        /// <summary>
        ///  SSH隧道连接用户名 （sshuser）
        /// </summary>
        public string SshUser { get; set; } 
        /// <summary>
        /// SSH隧道连接密码（sshpassword）
        /// </summary>
        public string SshPassword { get; set; } 
        /// <summary>
        ///  默认端口 3307
        /// </summary>
        public string SshLocalPort { get; set; } 
    }
}
