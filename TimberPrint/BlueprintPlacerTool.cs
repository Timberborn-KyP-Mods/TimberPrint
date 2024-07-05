using System.Diagnostics.CodeAnalysis;
using Timberborn.AreaSelectionSystem;
using Timberborn.CursorToolSystem;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;
using UnityEngine;

namespace TimberPrint;

public class BlueprintPlacer : IInputProcessor, ILoadableSingleton
{
    private readonly InputService _inputService;

    private readonly BlueprintService _blueprintService;

    private readonly AreaPickerFactory _areaPickerFactory;

    private readonly CursorCoordinatesPicker _cursorCoordinatesPicker;

    private BlueprintPreviewPlacer? _blueprintPreviewPlacer;

    private AreaPicker _areaPicker = null!;

    public BlueprintPlacer(InputService inputService, BlueprintService blueprintService, AreaPickerFactory areaPickerFactory, CursorCoordinatesPicker cursorCoordinatesPicker)
    {
        _inputService = inputService;
        _blueprintService = blueprintService;
        _areaPickerFactory = areaPickerFactory;
        _cursorCoordinatesPicker = cursorCoordinatesPicker;
    }

    public void Load()
    {
        _inputService.AddInputProcessor(this);
        _areaPicker = _areaPickerFactory.Create();
    }

    public bool ProcessInput()
    {
        HandleToolStatus();

        if (_blueprintPreviewPlacer == null)
        {
            return false;
        }

        if (TryGetCursorTile(out var tileCoordinates))
        {
            _blueprintPreviewPlacer.Show(tileCoordinates.Value);
        }
        else
        {
            _blueprintPreviewPlacer.HideAllPreviews();
        }

        return false;
    }

    private void HandleToolStatus()
    {
        if(_blueprintPreviewPlacer == null && _inputService.IsKeyDown("Blueprint.PlacerTool"))
        {
            Enter();
        }
        
        if(_blueprintPreviewPlacer != null && (_inputService.MouseCancel || _inputService.KeyboardCancel))
        {
            Exit();
        }
    }
    
    
    private bool TryGetCursorTile([NotNullWhen(true)] out Vector3Int? tileCoordinates)
    {
        var cursorCoordinates = _cursorCoordinatesPicker.CursorCoordinates();

        if (cursorCoordinates.HasValue)
        {
            tileCoordinates = cursorCoordinates.Value.TileCoordinates;
            return true;
        }
        
        tileCoordinates = null;
        return false;
    }

    private void Enter()
    {
        _blueprintPreviewPlacer = _blueprintService.LoadBlueprint();
    }

    private void Exit()
    {
        _blueprintPreviewPlacer?.HideAllPreviews();
        _blueprintPreviewPlacer = null;
    }
}