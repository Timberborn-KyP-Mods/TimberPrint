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

public class BlueprintAreaSelectorTool : Tool, IInputProcessor, ILoadableSingleton
{
    private readonly InputService _inputService;

    private readonly Colors _colors;

    private readonly BlockObjectSelectionDrawerFactory _blockObjectSelectionDrawerFactory;

    private readonly AreaBlockObjectPickerFactory _areaBlockObjectPickerFactory;

    private readonly PrefabNameRetriever _prefabNameRetriever;

    private readonly BlueprintRepository _blueprintRepository;

    private readonly ToolManager _toolManager;
    
    private readonly ConstructionModeService _constructionModeService;

    private readonly BlueprintManager _blueprintManager;

    private BlockObjectSelectionDrawer _blockObjectSelectionDrawer = null!;

    private AreaBlockObjectPicker _areaBlockObjectPicker = null!;

    public BlueprintAreaSelectorTool(InputService inputService,
        BlockObjectSelectionDrawerFactory blockObjectSelectionDrawerFactory, Colors colors,
        AreaBlockObjectPickerFactory areaBlockObjectPickerFactory, PrefabNameRetriever prefabNameRetriever, BlueprintRepository blueprintRepository, ToolManager toolManager, ConstructionModeService constructionModeService, BlueprintManager blueprintManager)
    {
        _inputService = inputService;
        _blockObjectSelectionDrawerFactory = blockObjectSelectionDrawerFactory;
        _colors = colors;
        _areaBlockObjectPickerFactory = areaBlockObjectPickerFactory;
        _prefabNameRetriever = prefabNameRetriever;
        _blueprintRepository = blueprintRepository;
        _toolManager = toolManager;
        _constructionModeService = constructionModeService;
        _blueprintManager = blueprintManager;
    }

    public void Load()
    {
        _blockObjectSelectionDrawer = _blockObjectSelectionDrawerFactory.Create(
            _colors.PriorityActionColor,
            _colors.PriorityTileColor,
            _colors.PrioritySideColor
            );
        
        _inputService.AddInputProcessor(this);
    }

    public override void Enter()
    {
        _constructionModeService.EnterConstructionMode();
        _areaBlockObjectPicker = _areaBlockObjectPickerFactory.CreatePickingUpwards();
    }

    public override void Exit()
    {
        ShowNoneCallback();
        _constructionModeService.ExitConstructionMode();
    }

    public bool ProcessInput()
    {
        HandleToolActivator();

        if (_toolManager.ActiveTool != this)
        {
            return false;
        }
        
        return _areaBlockObjectPicker.PickBlockObjects<PlaceableBlockObject>(PreviewCallback, ActionCallback,
            ShowNoneCallback);
    }
    
    private void HandleToolActivator()
    {
        if(_toolManager.ActiveTool != this && _inputService.IsKeyDown("Blueprint.CopyTool"))
        {
            _toolManager.SwitchTool(this);
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
                _prefabNameRetriever.GetPrefabName(o),
                GetRelativeBlockCoordinates(firstBlockObjectPosition.Placement.Coordinates, o.Placement.Coordinates),
                o.Placement.Orientation,
                o.FlipMode
            )
        ).ToArray();

        var bp = new Blueprint("", blueprintItems);
        
        _blueprintRepository.Add(bp);
        _blueprintManager.SwitchBlueprint(bp);
    }

    private void ShowNoneCallback() => _blockObjectSelectionDrawer.StopDrawing();
    
    private static Vector3Int GetRelativeBlockCoordinates(Vector3Int startingCoordinates, Vector3Int blockObjectCoordinates)
    {
        return blockObjectCoordinates - startingCoordinates;
    }
}