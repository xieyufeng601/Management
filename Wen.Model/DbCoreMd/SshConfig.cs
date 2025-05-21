using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wen.Model
{
    /// <summary>
    /// SSH配置类
    /// </summary>
    public class SshConfig
    {
        public required string Host { get; set; }
        public int Port { get; set; } = 22;
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? PrivateKeyPath { get; set; }
        public string? PrivateKeyPassphrase { get; set; }
        public required string DatabaseHost { get; set; }
        public int DatabasePort { get; set; } = 3306;
        public int LocalPort { get; set; } = 3307;
    }
}
