namespace Wen.Models.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ColAttribute : Attribute
{
    /// <summary>
    /// 名称
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 宽度
    /// </summary>
    public int Width { get; set; }
    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Show { get; set; } = true;
    /// <summary>
    /// 是支持列否筛选
    /// </summary>
    public bool IsFilter { get; set; }
    /// <summary>
    /// 是否固定列
    /// </summary>
    public bool IsFixed { get; set; }
    /// <summary>
    /// 是否生成编辑
    /// </summary>
    public bool IsEdit { get; set; } = true;
    /// <summary>
    /// 允许新增
    /// </summary>
    public bool IsAdd { get; set; } = true;
    /// <summary>
    /// 是否支持排序
    /// </summary>
    public bool IsSortOrder { get; set; }
    /// <summary>
    /// 注释
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// 数据编辑类型 暂时没想好如何使用，备注上
    /// </summary>
    public Type? ItemEditType { get; set; }
    /// <summary>
    /// 编辑窗口数据模板
    /// </summary>
    public Type? EditTemplate { get; set; }
}
