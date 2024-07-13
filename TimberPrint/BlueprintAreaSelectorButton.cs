using Timberborn.BottomBarSystem;
using Timberborn.ToolSystem;

namespace TimberPrint;

public class BlueprintAreaSelectorButton(
    BlueprintAreaSelectorTool blueprintAreaSelectorTool,
    ToolButtonFactory toolButtonFactory)
    : IBottomBarElementProvider
{
    private static readonly string ToolImageKey = "copy_icon";

    public BottomBarElement GetElement()
    {
        return BottomBarElement.CreateSingleLevel(toolButtonFactory.CreateGrouplessRed(blueprintAreaSelectorTool, ToolImageKey).Root);
    }
}