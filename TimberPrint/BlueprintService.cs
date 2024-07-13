using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using Timberborn.PrefabSystem;
using Timberborn.UISound;
using UnityEngine;

namespace TimberPrint;

public class BlueprintService
{
    private const string BlockObjectPlacedSoundName = "UI.BlockObjectPlaced";

    private readonly BlueprintRepository _blueprintRepository;

    private readonly BlueprintPreviewPlacerFactory _blueprintPreviewPlacerFactory;

    private readonly BlockObjectPlacerService _blockObjectPlacerService;

    private readonly PrefabNameMapper _prefabNameMapper;

    private readonly UISoundController _uiSoundController;

    private readonly BlueprintManager _blueprintManager;

    public BlueprintService(BlueprintRepository blueprintRepository,
        BlueprintPreviewPlacerFactory blueprintPreviewPlacerFactory, BlockObjectPlacerService blockObjectPlacerService,
        PrefabNameMapper prefabNameMapper, UISoundController uiSoundController, BlueprintManager blueprintManager)
    {
        _blueprintRepository = blueprintRepository;
        _blueprintPreviewPlacerFactory = blueprintPreviewPlacerFactory;
        _blockObjectPlacerService = blockObjectPlacerService;
        _prefabNameMapper = prefabNameMapper;
        _uiSoundController = uiSoundController;
        _blueprintManager = blueprintManager;
    }

    public bool TryLoadBlueprint([NotNullWhen(true)] out BlueprintPreviewPlacer? blueprintPreviewPlacer)
    {
        if (_blueprintManager.ActiveBlueprint != null)
        {
            blueprintPreviewPlacer = _blueprintPreviewPlacerFactory.Create(_blueprintManager.ActiveBlueprint);
            return true;
        }

        blueprintPreviewPlacer = null;
        return false;
    }

    public void PlaceBlueprint(BlueprintPreviewPlacer blueprintPreviewPlacer, Vector3Int coordinate, Orientation orientation, bool flip)
    {
        var placements = blueprintPreviewPlacer.GetBuildableCoordinates(coordinate, orientation, flip).ToArray();

        if (placements.Length == 0)
        {
            _uiSoundController.PlayCantDoSound();
            return;
        }

        for (var i = 0; i < placements.Length; i++)
        {
            var prefab = _prefabNameMapper.GetPrefab(blueprintPreviewPlacer.Blueprint.BlueprintItems[i].TemplateName)
                .GetComponentFast<BlockObject>();

            var blockObjectPlacer = _blockObjectPlacerService.GetMatchingPlacer(prefab);

            blockObjectPlacer.Place(prefab, placements[i]);
        }
        
        _uiSoundController.PlaySound(BlockObjectPlacedSoundName);
    }
    
    public void AddBlueprint(Blueprint blueprint)
    {
        _blueprintRepository.Add(blueprint);
    }
}