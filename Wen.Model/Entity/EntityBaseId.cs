using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wen.Models.Entity;

/// <summary>
/// 框架实体基类Id
/// </summary>
public abstract class EntityBaseId
{
    /// <summary>
    /// 雪花Id
    /// </summary>
    [Key]
    public virtual long Id { get; set; }
}
