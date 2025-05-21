using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using Wen.Models.Attributes;

namespace Wen.Models.Entity;

/// <summary>
/// 字典配置项
/// </summary>
public class SysDict
{
    /// <summary>
    /// 获得/设置 ID
    /// </summary>
    [Key]
    public string?  Id { get; set; }
    /// <summary>
    /// 获得/设置 字典标签
    /// </summary>
    [DisplayName("字典标签")]
    [Col]
    public string? Category { get; set; }

    /// <summary>
    /// 获得/设置 字典名称
    /// </summary>
    [DisplayName("字典名称")]
    [Col]
    public string? Name { get; set; }

    /// <summary>
    /// 获得/设置 字典字典值
    /// </summary>
    [DisplayName("字典代码")]
    [Col]
    public string? Code { get; set; }

    [DisplayName("字典颜色")]
    public string? ColorString { get; set; }
    /// <summary>
    /// 获得/设置 字典颜色
    /// </summary>
    [DisplayName("字典颜色")]
    [Col]
    public string?  ColorStr 
    {
        get; set;
       
    }

  

}
