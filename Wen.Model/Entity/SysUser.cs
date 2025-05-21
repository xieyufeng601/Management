using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wen.Models.Attributes;

namespace Wen.Models;
[SugarTable("SysUser")]
/// <summary>
/// 
/// </summary>
public class SysUser
{
    /// <summary>
    /// 获得/设置 用户主键ID
    /// </summary>
    [Key]
    public string ? Id { get; set; }

    /// <summary>
    /// 获得/设置 系统登录用户名
    /// </summary>
    [Key]
    [DisplayName("登录名称")]
    [Col(IsEdit = true, IsFilter = true, IsFixed = true)]
    public string? UserName { get; set; }

    /// <summary>
    /// 获得/设置 用户显示名称
    /// </summary>
    [DisplayName("显示名称")]
    [Col(IsEdit = true, IsFilter = true)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// 获得/设置 用户头像图标路径
    /// </summary>
    [DisplayName("用户头像")]
    [Col(Show = false, IsAdd = false, IsEdit = false)]
    public string? Icon { get; set; }

    /// <summary>
    /// 获取/设置 密码
    /// </summary>
    [DisplayName("密码")]
    public string? Password { get; set; }

    /// <summary>
    /// 获取/设置 密码盐
    /// </summary>
    [DisplayName("密码盐")]
    public string? PasswordSalt { get; set; }

    /// <summary>
    /// 获得/设置 说明
    /// </summary>
    [DisplayName("说明")]
    [Col(IsEdit = true)]
    public string? Description { get; set; }
}