using System;
using System.Collections.Generic;
using System.Linq;
using Timberborn.AreaSelectionSystem;
using Timberborn.BlockSystem;
using Timberborn.ConstructionMode;
using Timberborn.CoreUI;
using Timberborn.InputSystem;
using Timberborn.PrefabSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using UnityEngine;

namespace TimberPrint;

public class BlueprintAreaSelectorTool : Tool, IInputProcessor, ILoadableSingleton, IConstructionModeEnabler
{
    private readonly InputService _inputService;

    private readonly Colors _colors;

    private readonly BlockObjectSelectionDrawerFactory _blockObjectSelectionDrawerFactory;

    private readonly AreaBlockObjectPickerFactory _areaBlockObjectPickerFactory;

    private readonly PrefabNameRetriever _prefabNameRetriever;

    private readonly BlueprintRepository _blueprintRepository;

    private BlockObjectSelectionDrawer _blockObjectSelectionDrawer = null!;

    private AreaBlockObjectPicker _areaBlockObjectPicker = null!;

    public BlueprintAreaSelectorTool(InputService inputService,
        BlockObjectSelectionDrawerFactory blockObjectSelectionDrawerFactory, Colors colors,
        AreaBlockObjectPickerFactory areaBlockObjectPickerFactory, PrefabNameRetriever prefabNameRetriever, BlueprintRepository blueprintRepository)
    {
        _inputService = inputService;
        _blockObjectSelectionDrawerFactory = blockObjectSelectionDrawerFactory;
        _colors = colors;
        _areaBlockObjectPickerFactory = areaBlockObjectPickerFactory;
        _prefabNameRetriever = prefabNameRetriever;
        _blueprintRepository = blueprintRepository;
    }

    public void Load()
    {
        _blockObjectSelectionDrawer = _blockObjectSelectionDrawerFactory.Create(_colors.DeletedObjectHighlightColor,
            _colors.DeletedAreaTileColor, _colors.DeletedAreaSideColor);
    }

    public override void Enter()
    {
        _inputService.AddInputProcessor(this);
        _areaBlockObjectPicker = _areaBlockObjectPickerFactory.CreatePickingUpwards();
    }


    public override void Exit()
    {
        ShowNoneCallback();
        _inputService.RemoveInputProcessor(this);
        ShowNoneCallback();
    }

    public bool ProcessInput()
    {
        return _areaBlockObjectPicker.PickBlockObjects<PlaceableBlockObject>(PreviewCallback, ActionCallback,
            ShowNoneCallback);
    }

    private void PreviewCallback(
        IEnumerable<BlockObject> blockObjects,
        Vector3Int start,
        Vector3Int end,
        bool selectionStarted,
        bool selectingArea)
    {
        _blockObjectSelectionDrawer.Draw(blockObjects, start, end, selectingArea);
    }

    private void ActionCallback(
        IEnumerable<BlockObject> blockObjects,
        Vector3Int start,
        Vector3Int end,
        bool selectionStarted,
        bool selectingArea)
    {
        var orderedBlockPositions = blockObjects
            .OrderBy(o => o.Placement.Coordinates.x)
            .ThenBy(o => o.Placement.Coordinates.y)
            .ToArray();

        if (orderedBlockPositions.Length == 0)
        {
            return;
        }
        
        var firstBlockObjectPosition = orderedBlockPositions.First();
        
        var blueprintItems = orderedBlockPositions.Select(
            o => new BlueprintItem(
                _prefabNameRetriever.GetPrefabName(o),
                GetRelativeBlockCoordinates(firstBlockObjectPosition.Placement.Coordinates, o.Placement.Coordinates),
                o.Placement.Orientation,
                o.FlipMode
            )
        ).ToArray();

        var bp = new Blueprint(blueprintItems);
        
        _blueprintRepository.Add(bp);
    }

    private void ShowNoneCallback() => _blockObjectSelectionDrawer.StopDrawing();

    private static (Vector3Int, Vector3Int) NormalizeCoordinates(Vector3Int start, Vector3Int end)
    {
        var minX = Math.Min(start.x, end.x);
        var minY = Math.Min(start.y, end.y);

        var maxX = Math.Max(start.x, end.x);
        var maxY = Math.Max(start.y, end.y);

        return (new Vector3Int(minX, minY, start.z), new Vector3Int(maxX, maxY, start.z));
    }
    
    private static Vector3Int GetRelativeBlockCoordinates(Vector3Int startingCoordinates, Vector3Int blockObjectCoordinates)
    {
        return blockObjectCoordinates - startingCoordinates;
    }

    private static Vector3Int GetFirstBlockObjectPosition(IEnumerable<BlockObject> blockObjects)
    {
        return blockObjects
            .OrderBy(o => o.Placement.Coordinates.x)
            .ThenBy(o => o.Placement.Coordinates.y)
            .First().Coordinates;
    }
}