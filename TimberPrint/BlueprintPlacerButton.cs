using Timberborn.BottomBarSystem;
using Timberborn.ToolSystem;

namespace TimberPrint;

public class BlueprintPlacerButton(BlueprintPlacerTool blueprintPlacerTool, ToolButtonFactory toolButtonFactory)
    : IBottomBarElementProvider
{
    private static readonly string ToolImageKey = "paste_icon";

    public BottomBarElement GetElement()
    {
        return BottomBarElement.CreateSingleLevel(toolButtonFactory.CreateGrouplessRed(blueprintPlacerTool, ToolImageKey).Root);
    }
}