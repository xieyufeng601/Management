
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Wen.Models.Attributes;

namespace Wen.Models.Entity;

/// <summary>
/// Role 实体类
/// </summary>
//[Index($"index_{nameof(SysRole)}_{nameof(RoleName)}", nameof(RoleName), true)]
public class SysRole
{
    /// <summary>
    /// 获得/设置 角色名称
    /// </summary>
    [DisplayName("角色名称")]
    [Col]
    [Key]
    public string? RoleName { get; set; }

    /// <summary>
    /// 获得/设置 角色描述
    /// </summary>
    [DisplayName("角色描述")]
    [Col]
    public string? Description { get; set; }
}
