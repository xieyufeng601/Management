using System.ComponentModel;

namespace Wen.Models.Enums;

/// <summary>
/// 数据变化类型
/// </summary>
public enum ItemChangedType
{
    /// <summary>
    /// 新建
    /// </summary>
    [Description("新增")]
    Add,

    /// <summary>
    /// 更新
    /// </summary>
    [Description("更新")]
    Update,
}
