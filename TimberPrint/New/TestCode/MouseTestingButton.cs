using Timberborn.BottomBarSystem;
using Timberborn.ToolSystem;

namespace TimberPrint.New.TestCode;

public class MouseTestingButton(ToolButtonFactory toolButtonFactory, MouseTestingTool mouseTestingTool)
    : IBottomBarElementProvider
{
    private static readonly string ToolImageKey = "copy_icon";

    public BottomBarElement GetElement()
    {
        return BottomBarElement.CreateSingleLevel(toolButtonFactory.CreateGrouplessRed(mouseTestingTool, ToolImageKey).Root);
    }
}