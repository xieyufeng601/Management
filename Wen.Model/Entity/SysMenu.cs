
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wen.Models.Attributes;
using Wen.Models.Enums;



namespace Wen.Models.Entity;

/// <summary>
/// Bootstrap Admin 后台管理菜单相关操作实体类
/// </summary>
public class SysMenu
{
    /// <summary>
    /// 获得/设置 主键 ID
    /// </summary>
    [Key]
    [Col]
    [DisplayName("代码")]
    public string? Code { get; set; }

    /// <summary>
    /// 获得/设置 父层ID
    /// </summary>
    [DisplayName("父层")]
    [Col]
    public string? ParentCode { get; set; }

    /// <summary>
    /// 获得/设置 菜单名称
    /// </summary>
    [DisplayName("名称")]
    [Col]
    public string? Name { get; set; }

    /// <summary>
    /// 获得/设置 菜单序号
    /// </summary>
    [DisplayName("序号")]
    [Col]
    public int OrderNo { get; set; }

    /// <summary>
    /// 获得/设置 菜单图标
    /// </summary>
    [DisplayName("图标")]
    [Col]
    public string? Icon { get; set; }

    /// <summary>
    /// 获得/设置 菜单URL地址
    /// </summary>

    [DisplayName("地址")]
    public string? Url { get; set; }

    /// <summary>
    /// 获得/设置 是否为资源文件, 1 表示菜单 2 表示按钮
    /// </summary>
    [DisplayName("类型")]
    [Col]
    public MenuType MenuType { get; set; }

    /// <summary>
    /// 获取/设置 注释描述
    /// </summary>
    [DisplayName("注释")]
    [Col]
    public string? Description { get; set; }

    /// <summary>
    /// 页面控件类型
    /// </summary>
    [DisplayName("页面控件")]
    public string? ControlType { get; set; }

    /// <summary>
    /// 树结构子数据
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<SysMenu>? Child { get; set; }
}
