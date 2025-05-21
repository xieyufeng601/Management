using System.ComponentModel;

namespace Wen.Models.Entity;

public class SysUserRole
{
    [DisplayName("用户名")]
    public string? UserName { get; set; }
    [DisplayName("角色名称")]
    public string? RoleName { get; set; }
}
