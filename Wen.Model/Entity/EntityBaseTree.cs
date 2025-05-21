using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wen.Models.Entity;

/// <summary>
/// 树结构框架实体基类
/// </summary>
public abstract class EntityBaseTree : EntityBaseTreeId
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [Description("创建时间")]
    public virtual DateTime? CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Description("更新时间")]
    public virtual DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 创建者Id
    /// </summary>
    [Description("创建者用户名")]
    public virtual string? CreateUserName { get; set; }

    /// <summary>
    /// 修改者Id
    /// </summary>
    [Description("修改者用户名")]
    public virtual string? UpdateUserName { get; set; }
}