using System.ComponentModel;

namespace Wen.Models.Entity;

public class EntityBaseDelFilter : EntityBase, IDeletedFilter
{
    /// <summary>
    /// 软删除
    /// </summary>
    [Description("软删除")]
    public virtual bool IsDelete { get; set; } = false;
}
