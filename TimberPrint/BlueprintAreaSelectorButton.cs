using Timberborn.BottomBarSystem;
using Timberborn.ToolSystem;

namespace TimberPrint;

public class BlueprintAreaSelectorButton : IBottomBarElementProvider
{
    private static readonly string ToolImageKey = "copy_icon";
    
    private readonly BlueprintAreaSelectorTool _blueprintAreaSelectorTool;
    
    private readonly ToolButtonFactory _toolButtonFactory;

    public BlueprintAreaSelectorButton(BlueprintAreaSelectorTool blueprintAreaSelectorTool, ToolButtonFactory toolButtonFactory)
    {
        _blueprintAreaSelectorTool = blueprintAreaSelectorTool;
        _toolButtonFactory = toolButtonFactory;
    }

    public BottomBarElement GetElement()
    {
        return BottomBarElement.CreateSingleLevel(_toolButtonFactory.CreateGrouplessRed(_blueprintAreaSelectorTool, ToolImageKey).Root);
    }
}