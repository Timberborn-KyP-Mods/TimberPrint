using System.Collections.Generic;
using System.Linq;
using Bindito.Unity;
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

public class BlueprintAreaSelectorTool(
    InputService inputService,
    BlockObjectSelectionDrawerFactory blockObjectSelectionDrawerFactory,
    Colors colors,
    AreaBlockObjectPickerFactory areaBlockObjectPickerFactory,
    PrefabNameRetriever prefabNameRetriever,
    BlueprintRepository blueprintRepository,
    ToolManager toolManager,
    ConstructionModeService constructionModeService,
    BlueprintManager blueprintManager)
    : Tool, IInputProcessor, ILoadableSingleton
{
    private BlockObjectSelectionDrawer _blockObjectSelectionDrawer = null!;

    private AreaBlockObjectPicker _areaBlockObjectPicker = null!;

    public void Load()
    {
        _blockObjectSelectionDrawer = blockObjectSelectionDrawerFactory.Create(
            colors.PriorityActionColor,
            colors.PriorityTileColor,
            colors.PrioritySideColor
            );
        
        inputService.AddInputProcessor(this);
    }

    public override void Enter()
    {
        constructionModeService.EnterConstructionMode();
        _areaBlockObjectPicker = areaBlockObjectPickerFactory.CreatePickingUpwards();
    }

    public override void Exit()
    {
        ShowNoneCallback();
        constructionModeService.ExitConstructionMode();
    }

    public bool ProcessInput()
    {
        HandleToolActivator();

        if (toolManager.ActiveTool != this)
        {
            return false;
        }
        
        return _areaBlockObjectPicker.PickBlockObjects<PlaceableBlockObject>(PreviewCallback, ActionCallback,
            ShowNoneCallback);
    }
    
    private void HandleToolActivator()
    {
        if(toolManager.ActiveTool != this && inputService.IsKeyDown("Blueprint.CopyTool"))
        {
            toolManager.SwitchTool(this);
        }
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
                prefabNameRetriever.GetPrefabName(o),
                GetRelativeBlockCoordinates(firstBlockObjectPosition.Placement.Coordinates, o.Placement.Coordinates),
                o.Placement.Orientation,
                o.FlipMode
            )
        ).ToArray();

        var bp = new Blueprint("", blueprintItems);
        
        blueprintRepository.Add(bp);
        blueprintManager.SwitchBlueprint(bp);
    }

    private void ShowNoneCallback() => _blockObjectSelectionDrawer.StopDrawing();
    
    private static Vector3Int GetRelativeBlockCoordinates(Vector3Int startingCoordinates, Vector3Int blockObjectCoordinates)
    {
        return blockObjectCoordinates - startingCoordinates;
    }
}