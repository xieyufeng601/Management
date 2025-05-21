using System.ComponentModel;

namespace Wen.Models.Entity;

public class SysRoleMenu
{
    /// <summary>
    /// 角色名称
    /// </summary>
    [DisplayName("角色名称")]
    public string? RoleName { get; set; }

    /// <summary>
    /// 菜单代码
    /// </summary>
    [DisplayName("菜单代码")]
    public string ?MenuCode { get; set; }
}
