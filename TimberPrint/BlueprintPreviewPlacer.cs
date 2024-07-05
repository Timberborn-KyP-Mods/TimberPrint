using System.Collections.Generic;
using System.Linq;
using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using Timberborn.PrefabSystem;
using Timberborn.PreviewSystem;
using UnityEngine;

namespace TimberPrint;

public class BlueprintPreviewPlacer
{
    private readonly PreviewShower _previewShower;

    private readonly BlockValidator _blockValidator;

    private readonly BlockService _blockService;

    private readonly PreviewValidationService _previewValidationService;

    private readonly bool _treatPreviewsAsSingle;

    private readonly Preview[] _previews;

    private readonly string _prefabName;

    private readonly List<Placement> _coordinatesCache = new();

    public string WarningText { get; private set; } = "";

    public Blueprint Blueprint { get; }

    public PlaceableBlockObject PreviewHandler { get; }

    private readonly Placement[] _previewPlacements;
    
    private Preview SinglePreview => _previews[0];

    private int _bottomHeight;
    
    public BlueprintPreviewPlacer(PreviewShower previewShower, BlockValidator blockValidator, BlockService blockService,
        PreviewValidationService previewValidationService, bool treatPreviewsAsSingle, Preview[] previews,
        Placement[] previewPlacements,
        Blueprint blueprint, PlaceableBlockObject previewHandler)
    {
        _previewShower = previewShower;
        _blockValidator = blockValidator;
        _blockService = blockService;
        _previewValidationService = previewValidationService;
        _previews = previews;
        _treatPreviewsAsSingle = treatPreviewsAsSingle;
        _previewPlacements = previewPlacements;
        Blueprint = blueprint;
        _prefabName = SinglePreview.GetComponentFast<Prefab>().PrefabName;
        PreviewHandler = previewHandler;
    }

    #region PersonalCode

    public void Show(Vector3Int anchorCoordinate, Orientation orientation = Orientation.Cw0)
    {
        _bottomHeight = anchorCoordinate.z;
        
        var placements = _previewPlacements
            .Select(previewPlacement => new Placement(anchorCoordinate + previewPlacement.Coordinates,
                previewPlacement.Orientation, previewPlacement.FlipMode));
        
        ShowPreviews(placements);
    }

    public IEnumerable<Placement> GetBuildableCoordinates(Vector3Int anchorCoordinate,
        Orientation orientation = Orientation.Cw0)
    {
        var placements = _previewPlacements
            .Select(previewPlacement => new Placement(anchorCoordinate + previewPlacement.Coordinates,
                previewPlacement.Orientation, previewPlacement.FlipMode));

        return GetBuildableCoordinates(placements);
    }

    #endregion

    public void ShowPreviews(IEnumerable<Placement> placements)
    {
        WarningText = "";
        _previewShower.UnhighlightAllPreviews();
        _coordinatesCache.AddRange(placements);
        if (_coordinatesCache.Count > 0)
        {
            var buildablePreviewsAndAddToBlockServices = GetBuildablePreviewsAndAddToBlockServices(_coordinatesCache, out var blocksCount);
            
            if (_treatPreviewsAsSingle)
            {
                ShowAllPreviews(buildablePreviewsAndAddToBlockServices);
            }
            else if (blocksCount == 1)
            {
                HideRemainingPreviews(buildablePreviewsAndAddToBlockServices, exceptSinglePreview: true);
                ShowSinglePreview(buildablePreviewsAndAddToBlockServices);
            }
            else
            {
                HideRemainingPreviews(buildablePreviewsAndAddToBlockServices, exceptSinglePreview: false);
                ShowBuildablePreviews(buildablePreviewsAndAddToBlockServices);
            }
        }
        else
        {
            HideAllPreviews();
        }

        _coordinatesCache.Clear();
    }

    public IEnumerable<Placement> GetBuildableCoordinates(IEnumerable<Placement> placements)
    {
        var buildablePreviewsAndAddToBlockServices =
            GetBuildablePreviewsAndAddToBlockServices(placements, out _);
        var flag = PreviewsAreValid(buildablePreviewsAndAddToBlockServices);
        RemovePreviewsFromServices();
        if (!flag)
        {
            yield break;
        }

        foreach (var item in buildablePreviewsAndAddToBlockServices)
        {
            yield return item.BlockObject.Placement;
        }
    }

    public void HideAllPreviews()
    {
        RemovePreviewsFromServices();
        _previewShower.HidePreviews(_previews);
    }

    private bool PreviewsAreValid(IReadOnlyList<Preview> buildablePreviews)
    {
        if (_previewValidationService.PreviewsAreValid(buildablePreviews))
        {
            if (_treatPreviewsAsSingle)
            {
                return buildablePreviews.Count == _previews.Length;
            }

            return true;
        }

        return false;
    }

    private List<Preview> GetBuildablePreviewsAndAddToBlockServices(IEnumerable<Placement> placements, out int blocksCount)
    {
        RemovePreviewsFromServices();
        var positionedBuildablePreviews = GetPositionedBuildablePreviews(placements, out blocksCount);
        foreach (var item in positionedBuildablePreviews)
        {
            item.AddToPreviewServices();
        }

        return positionedBuildablePreviews;
    }

    private List<Preview> GetPositionedBuildablePreviews(IEnumerable<Placement> placements, out int blocksCount)
    {
        var positionedPreviews = GetPositionedPreviews(placements, out blocksCount);
        return BuildablePreviews(positionedPreviews).ToList();
    }

    private IEnumerable<Preview> GetPositionedPreviews(IEnumerable<Placement> placements, out int blocksCount)
    {
        blocksCount = 0;
        foreach (var placement in placements)
        {
            _previews[blocksCount++].Reposition(placement);
        }

        return _previews.Take(blocksCount);
    }

    private IEnumerable<Preview> BuildablePreviews(IEnumerable<Preview> previews)
    {
        var enumerable = previews.Where(preview =>
            preview.BlockObject.Coordinates.z == _bottomHeight
                ? BottomBlocksValid(preview.BlockObject.PositionedBlocks)
                : BlocksValid(preview.BlockObject.PositionedBlocks));
        
        if (!SinglePreview.BlockObject.Overridable)
        {
            return enumerable;
        }

        return ExceptOverridingSame(enumerable);
    }
    
    public bool BottomBlocksValid(PositionedBlocks positionedBlocks)
    {
        return positionedBlocks.GetAllBlocks().All(block => _blockValidator.BlockValid(block, false, false));
    }
    
    public bool BlocksValid(PositionedBlocks positionedBlocks)
    {
        return positionedBlocks.GetAllBlocks().All(block => _blockValidator.FitsInMap(block, true) && ! _blockValidator.BlockConflictsWithExistingObject(block) && !_blockValidator.BlockConflictsWithTerrain(block));
    }

    private IEnumerable<Preview> ExceptOverridingSame(IEnumerable<Preview> buildablePreviews)
    {
        foreach (var buildablePreview in buildablePreviews)
        {
            var occupiedCoordinates = buildablePreview.BlockObject.PositionedBlocks.GetOccupiedCoordinates();
            foreach (var item in occupiedCoordinates)
            {
                if (_blockService.GetObjectsWithComponentAt<Prefab>(item).All(prefab => prefab.PrefabName != _prefabName))
                {
                    yield return buildablePreview;
                }
            }
        }
    }

    private void RemovePreviewsFromServices()
    {
        var previews = _previews;
        for (var i = 0; i < previews.Length; i++)
        {
            previews[i].RemoveFromPreviewServices();
        }
    }

    private void ShowAllPreviews(IReadOnlyCollection<Preview> buildablePreviews)
    {
        if (buildablePreviews.Count == _previews.Length)
        {
            ShowBuildablePreviews(_previews);
        }
        else if (AnyPreviewIsAlmostValid())
        {
            ShowUnbuildablePreviews(_previews);
            WarningText = GetWarningText(_previews);
            UpdateModels(_previews);
        }
        else
        {
            HideAllPreviews();
        }
    }

    private bool AnyPreviewIsAlmostValid()
    {
        foreach (var preview in _previews)
        {
            if (PreviewAlmostValid(preview))
            {
                return true;
            }
        }

        return false;
    }

    private bool PreviewAlmostValid(Preview preview)
    {
        return _blockValidator.BlocksAlmostValid(preview.BlockObject.PositionedBlocks);
    }

    private void ShowSinglePreview(IReadOnlyCollection<Preview> buildablePreviews)
    {
        if (buildablePreviews.Count > 0)
        {
            ShowBuildablePreviews(new Preview[1] { SinglePreview });
        }
        else if (PreviewAlmostValid(SinglePreview))
        {
            WarningText = GetWarningText(new Preview[1] { SinglePreview });
            _previewShower.ShowUnbuildablePreview(SinglePreview);
            SinglePreview.UpdateModel();
        }
        else
        {
            HideAndRemoveFromServices(SinglePreview);
        }
    }

    private void HideRemainingPreviews(IReadOnlyCollection<Preview> buildablePreviews, bool exceptSinglePreview)
    {
        foreach (var item in _previews.Except(buildablePreviews))
        {
            if (item != SinglePreview || !exceptSinglePreview)
            {
                HideAndRemoveFromServices(item);
            }
        }
    }

    private void ShowBuildablePreviews(IReadOnlyList<Preview> previews)
    {
        if (_previewValidationService.PreviewsAreValid(previews, out var errorMessage))
        {
            if (_treatPreviewsAsSingle)
            {
                _previewShower.ShowBuildablePreviewsAsSingle(previews, out errorMessage);
            }
            else
            {
                _previewShower.ShowBuildablePreviews(previews, out errorMessage);
            }
        }
        else
        {
            ShowUnbuildablePreviews(previews);
        }

        UpdateModels(previews);
        WarningText = errorMessage;
    }

    private string GetWarningText(IReadOnlyList<Preview> previews)
    {
        _previewValidationService.PreviewsAreValid(previews, out var errorMessage);
        return errorMessage;
    }

    private void ShowUnbuildablePreviews(IReadOnlyList<Preview> previews)
    {
        if (_treatPreviewsAsSingle)
        {
            _previewShower.ShowUnbuildablePreviewsAsSingle(previews);
        }
        else
        {
            _previewShower.ShowUnbuildablePreviews(previews);
        }
    }

    private static void UpdateModels(IReadOnlyList<Preview> previews)
    {
        foreach (var preview in previews)
        {
            preview.UpdateModel();
        }
    }

    private static void HideAndRemoveFromServices(Preview preview)
    {
        preview.RemoveFromPreviewServices();
        preview.Hide();
    }
}