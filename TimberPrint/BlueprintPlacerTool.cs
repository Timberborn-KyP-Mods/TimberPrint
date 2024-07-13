using System.Collections.Generic;
using System.Linq;
using Timberborn.AreaSelectionSystem;
using Timberborn.ConstructionMode;
using Timberborn.Coordinates;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;

namespace TimberPrint;

public class BlueprintPlacerTool(
    InputService inputService,
    BlueprintService blueprintService,
    AreaPickerFactory areaPickerFactory,
    ToolManager toolManager,
    ConstructionModeService constructionModeService)
    : Tool, IInputProcessor, ILoadableSingleton
{
    private BlueprintPreviewPlacer? _blueprintPreviewPlacer;

    private AreaPicker _areaPicker = null!;

    private Orientation _orientation = Orientation.Cw0;

    private bool _flip;

    public void Load()
    {
        inputService.AddInputProcessor(this);
        _areaPicker = areaPickerFactory.Create();
    }
    
    public bool ProcessInput()
    {
        HandleToolActivator();

        if (_blueprintPreviewPlacer == null)
        {
            return false;
        }

        HandleOrientation();

        return _areaPicker.PickBlockObjectArea(_blueprintPreviewPlacer.PreviewHandler, Orientation.Cw0, FlipMode.Unflipped, PreviewCallback, ActionCallback);
    }

    private void ActionCallback(IEnumerable<Placement> placements)
    {
        if (_blueprintPreviewPlacer != null)
        {
            blueprintService.PlaceBlueprint(_blueprintPreviewPlacer, placements.LastOrDefault().Coordinates, _orientation, _flip);
        }
    }

    private void PreviewCallback(IEnumerable<Placement> placements)
    {
        var placement = placements.LastOrDefault();
        
        if (placement != null)
        {
            _blueprintPreviewPlacer?.Show(placement.Coordinates, _orientation, _flip);
        }
    }

    private void HandleToolActivator()
    {
        if(_blueprintPreviewPlacer == null && inputService.IsKeyDown("Blueprint.PlacerTool"))
        {
            toolManager.SwitchTool(this);
        }
    }
    
    private void HandleOrientation()
    {
        if(inputService.IsKeyDown("RotateClockwise"))
        {
            _orientation = _orientation.RotateClockwise();
        }
        
        if(inputService.IsKeyDown("RotateCounterclockwise"))
        {
            _orientation = _orientation.RotateCounterclockwise();
        }
        
        if(inputService.IsKeyDown("Flip"))
        {
            _flip = !_flip;
        }
    }

    public override void Enter()
    {
        _orientation = Orientation.Cw0;
        _flip = false;
        constructionModeService.EnterConstructionMode();
        blueprintService.TryLoadBlueprint(out _blueprintPreviewPlacer);
    }

    public override  void Exit()
    {
        _blueprintPreviewPlacer?.HideAllPreviews();
        _blueprintPreviewPlacer = null;
        constructionModeService.ExitConstructionMode();
    }
}