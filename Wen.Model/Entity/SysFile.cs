using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wen.Models.Entity
{
    public class SysFile
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Description("名称")]
        public string Name { get; set; } = "";

        /// <summary>
        /// 文件后缀
        /// </summary>
        [Description("文件后缀")]
        public string Ext { get; set; } = "";

        /// <summary>
        /// 注释
        /// </summary>
        [Description("注释")]
        public string Description { get; set; } = "";

        /// <summary>
        /// 文件大小(kb)
        /// </summary>
        [Description("文件大小(kb)")]
        public long SizeKb { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Description("文件类型")]
        public string ContentType { get; set; } = "application/octet-stream";

        /// <summary>
        /// 目录
        /// </summary>
        [Description("目录")]
        public string Folder { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建用户名
        /// </summary>
        [Description("创建用户名")]
        public string? CreateUserName { get; set; }
    }
}
