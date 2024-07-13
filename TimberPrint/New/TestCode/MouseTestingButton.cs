using Timberborn.BottomBarSystem;
using Timberborn.ToolSystem;

namespace TimberPrint.New.TestCode;

public class MouseTestingButton : IBottomBarElementProvider
{
    private static readonly string ToolImageKey = "copy_icon";
    
    private readonly MouseTestingTool _mouseTestingTool;
    
    private readonly ToolButtonFactory _toolButtonFactory;

    public MouseTestingButton(ToolButtonFactory toolButtonFactory, MouseTestingTool mouseTestingTool)
    {
        _toolButtonFactory = toolButtonFactory;
        _mouseTestingTool = mouseTestingTool;
    }

    public BottomBarElement GetElement()
    {
        return BottomBarElement.CreateSingleLevel(_toolButtonFactory.CreateGrouplessRed(_mouseTestingTool, ToolImageKey).Root);
    }
}