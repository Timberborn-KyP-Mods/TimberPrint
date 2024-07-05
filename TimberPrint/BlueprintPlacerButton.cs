using Timberborn.BottomBarSystem;
using Timberborn.ToolSystem;

namespace TimberPrint;

public class BlueprintPlacerButton : IBottomBarElementProvider
{
    private static readonly string ToolImageKey = "paste_icon";
    
    private readonly BlueprintPlacerTool _blueprintPlacerTool;
    
    private readonly ToolButtonFactory _toolButtonFactory;

    public BlueprintPlacerButton(BlueprintPlacerTool blueprintPlacerTool, ToolButtonFactory toolButtonFactory)
    {
        _blueprintPlacerTool = blueprintPlacerTool;
        _toolButtonFactory = toolButtonFactory;
    }

    public BottomBarElement GetElement()
    {
        return BottomBarElement.CreateSingleLevel(_toolButtonFactory.CreateGrouplessRed(_blueprintPlacerTool, ToolImageKey).Root);
    }
}