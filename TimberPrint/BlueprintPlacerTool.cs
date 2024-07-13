using System.Collections.Generic;
using System.Linq;
using Timberborn.AreaSelectionSystem;
using Timberborn.ConstructionMode;
using Timberborn.Coordinates;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;

namespace TimberPrint;

public class BlueprintPlacerTool : Tool, IInputProcessor, ILoadableSingleton
{
    private readonly InputService _inputService;

    private readonly BlueprintService _blueprintService;

    private readonly AreaPickerFactory _areaPickerFactory;

    private readonly ToolManager _toolManager;

    private readonly ConstructionModeService _constructionModeService;

    private BlueprintPreviewPlacer? _blueprintPreviewPlacer;

    private AreaPicker _areaPicker = null!;

    private Orientation _orientation = Orientation.Cw0;

    private bool _flip;

    public BlueprintPlacerTool(InputService inputService, BlueprintService blueprintService, AreaPickerFactory areaPickerFactory, ToolManager toolManager, ConstructionModeService constructionModeService)
    {
        _inputService = inputService;
        _blueprintService = blueprintService;
        _areaPickerFactory = areaPickerFactory;
        _toolManager = toolManager;
        _constructionModeService = constructionModeService;
    }

    public void Load()
    {
        _inputService.AddInputProcessor(this);
        _areaPicker = _areaPickerFactory.Create();
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
            _blueprintService.PlaceBlueprint(_blueprintPreviewPlacer, placements.LastOrDefault().Coordinates, _orientation, _flip);
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
        if(_blueprintPreviewPlacer == null && _inputService.IsKeyDown("Blueprint.PlacerTool"))
        {
            _toolManager.SwitchTool(this);
        }
    }
    
    private void HandleOrientation()
    {
        if(_inputService.IsKeyDown("RotateClockwise"))
        {
            _orientation = _orientation.RotateClockwise();
        }
        
        if(_inputService.IsKeyDown("RotateCounterclockwise"))
        {
            _orientation = _orientation.RotateCounterclockwise();
        }
        
        if(_inputService.IsKeyDown("Flip"))
        {
            _flip = !_flip;
        }
    }

    public override void Enter()
    {
        _orientation = Orientation.Cw0;
        _flip = false;
        _constructionModeService.EnterConstructionMode();
        _blueprintService.TryLoadBlueprint(out _blueprintPreviewPlacer);
    }

    public override  void Exit()
    {
        _blueprintPreviewPlacer?.HideAllPreviews();
        _blueprintPreviewPlacer = null;
        _constructionModeService.ExitConstructionMode();
    }
}