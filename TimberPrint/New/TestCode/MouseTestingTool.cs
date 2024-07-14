using System.Collections.Generic;
using Timberborn.AreaSelectionSystem;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.CursorToolSystem;
using Timberborn.DeconstructionSystemUI;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using TimberPrint.New.BlueprintSelectionSystem;
using UnityEngine;
using BlockObjectSelectionDrawer = TimberPrint.New.BlueprintSelectionSystem.BlockObjectSelectionDrawer;
using BlockObjectSelectionDrawerFactory = TimberPrint.New.BlueprintSelectionSystem.BlockObjectSelectionDrawerFactory;

namespace TimberPrint.New.TestCode;

public class MouseTestingTool(
    InputService inputService,
    CursorCoordinatesPicker cursorCoordinatesPicker,
    BlockObjectSelectionDrawerFactory blockObjectSelectionDrawerFactory,
    BlueprintAreaBlockObjectPickerFactory blueprintAreaBlockObjectPickerFactory)
    : Tool, IInputProcessor, ILoadableSingleton
{
    private BlueprintAreaBlockObjectPicker _blueprintAreaBlockObjectPicker = null!;
    
    private BlockObjectSelectionDrawer _blockObjectSelectionDrawer = null!;

    private Vector3Int? _pointA;

    private Vector3Int? _pointB;

    private int _height;

    public bool ProcessInput()
    {
        if (inputService.IsKeyDown("IncreaseHeight"))
        {
            _height += 1;
        }

        if (inputService.IsKeyDown("DecreaseHeight"))
        {
            _height -= 1;
        }
        
        return _blueprintAreaBlockObjectPicker.PickBlockObjects<Building>(PreviewCallback, ActionCallback, ShowNoneCallback, _height);
        
        if (inputService.IsKeyDown("PointA"))
        {
            _pointA = GetCursorCoordinates()?.TileCoordinates;
        }

        if (inputService.IsKeyDown("PointB"))
        {
            _pointB = GetCursorCoordinates()?.TileCoordinates;
        }

        if (inputService.IsKeyDown("IncreaseHeight"))
        {
            _height += 1;
        }

        if (inputService.IsKeyDown("DecreaseHeight"))
        {
            _height -= 1;
        }

        if (_pointA.HasValue && _pointB.HasValue)
        {
            _blockObjectSelectionDrawer.Draw(new List<BlockObject>(), _pointA.Value, _pointB.Value, _height, true);
        }

        Debug.LogWarning($"PointA: {_pointA}, PointB: {_pointB}, Height: {_height}");

        return false;
    }

    private void ShowNoneCallback()
    {
        _blockObjectSelectionDrawer.StopDrawing();
    }

    private void ActionCallback(IEnumerable<BlockObject> blockObjects, Vector3Int start, Vector3Int end, bool selectionstarted, bool selectingArea)
    {
        
    }

    private void PreviewCallback(IEnumerable<BlockObject> blockObjects, Vector3Int start, Vector3Int end, bool selectionstarted, bool selectingArea)
    {
        _blockObjectSelectionDrawer.Draw(blockObjects, start, end, _height, selectingArea);
    }

    private CursorCoordinates? GetCursorCoordinates()
    {
        return cursorCoordinatesPicker.CursorCoordinates();
    }

    public override void Enter()
    {
        inputService.AddInputProcessor(this);
    }

    public override void Exit()
    {
        inputService.RemoveInputProcessor(this);
    }

    public void Load()
    {
        _blueprintAreaBlockObjectPicker = blueprintAreaBlockObjectPickerFactory.CreatePickingUpwards();
        
        _blockObjectSelectionDrawer = blockObjectSelectionDrawerFactory.Create(
            new Color(0, 0.41f, 1),
            Color.blue,
            new Color(0.25f, 0.50f, 0.90f)
        );
    }
}