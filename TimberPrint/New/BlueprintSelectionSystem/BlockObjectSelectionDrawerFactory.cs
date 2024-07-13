using Timberborn.SelectionSystem;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlockObjectSelectionDrawerFactory
{
    private readonly Highlighter _highlighter;

    private readonly RectangleBoundsDrawerFactory _rectangleBoundsDrawerFactory;

    public BlockObjectSelectionDrawerFactory(
        Highlighter highlighter,
        RectangleBoundsDrawerFactory rectangleBoundsDrawerFactory)
    {
        _highlighter = highlighter;
        _rectangleBoundsDrawerFactory = rectangleBoundsDrawerFactory;
    }

    public BlockObjectSelectionDrawer Create(
        Color blockObjectHighlightColor,
        Color areaTileColor,
        Color areaSideColor)
    {
        return new BlockObjectSelectionDrawer(
            _rectangleBoundsDrawerFactory.Create(areaTileColor, areaSideColor),
            new RollingHighlighter(_highlighter), blockObjectHighlightColor
        );
    }
}