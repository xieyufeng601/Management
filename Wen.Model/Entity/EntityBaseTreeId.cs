using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wen.Models.Entity;

/// <summary>
/// 树结构框架实体基类Id
/// </summary>
public abstract class EntityBaseTreeId
{
    /// <summary>
    /// 雪花Id
    /// </summary>
    [Key]
    public virtual long Id { get; set; }

    /// <summary>
    /// 雪花Id
    /// </summary>
    [DisplayName("父层ID")]
    public virtual long ParentId { get; set; }
}
