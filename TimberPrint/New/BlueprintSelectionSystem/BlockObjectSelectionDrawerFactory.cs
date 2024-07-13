using Timberborn.AreaSelectionSystem;
using Timberborn.SelectionSystem;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlockObjectSelectionDrawerFactory(
    Highlighter highlighter,
    TopRectangleBoundsDrawerFactory topRectangleBoundsDrawerFactory,
    RectangleBoundsDrawerFactory rectangleBoundsDrawerFactory)
{
    public BlockObjectSelectionDrawer Create(
        Color blockObjectHighlightColor,
        Color areaTileColor,
        Color areaSideColor)
    {
        return new BlockObjectSelectionDrawer(
            rectangleBoundsDrawerFactory.Create(areaTileColor, areaSideColor),
            topRectangleBoundsDrawerFactory.Create(areaTileColor, areaSideColor),
            new RollingHighlighter(highlighter), blockObjectHighlightColor
        );
    }
}