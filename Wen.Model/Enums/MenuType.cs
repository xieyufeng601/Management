using System.ComponentModel;

namespace Wen.Models.Enums;

public enum MenuType
{
    [Description("无")]
    None = 0,
    [Description("菜单")]
    Menu = 1,
    [Description("功能")]
    Block = 2,
}
