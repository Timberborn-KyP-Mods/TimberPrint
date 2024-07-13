using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Timberborn.BlockObjectTools;
using Timberborn.BlockSystem;
using Timberborn.Coordinates;
using Timberborn.PrefabSystem;
using Timberborn.UISound;
using UnityEngine;

namespace TimberPrint;

public class BlueprintService(
    BlueprintRepository blueprintRepository,
    BlueprintPreviewPlacerFactory blueprintPreviewPlacerFactory,
    BlockObjectPlacerService blockObjectPlacerService,
    PrefabNameMapper prefabNameMapper,
    UISoundController uiSoundController,
    BlueprintManager blueprintManager)
{
    private const string BlockObjectPlacedSoundName = "UI.BlockObjectPlaced";

    public bool TryLoadBlueprint([NotNullWhen(true)] out BlueprintPreviewPlacer? blueprintPreviewPlacer)
    {
        if (blueprintManager.ActiveBlueprint != null)
        {
            blueprintPreviewPlacer = blueprintPreviewPlacerFactory.Create(blueprintManager.ActiveBlueprint);
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
            uiSoundController.PlayCantDoSound();
            return;
        }

        for (var i = 0; i < placements.Length; i++)
        {
            var prefab = prefabNameMapper.GetPrefab(blueprintPreviewPlacer.Blueprint.BlueprintItems[i].TemplateName)
                .GetComponentFast<BlockObject>();

            var blockObjectPlacer = blockObjectPlacerService.GetMatchingPlacer(prefab);

            blockObjectPlacer.Place(prefab, placements[i]);
        }
        
        uiSoundController.PlaySound(BlockObjectPlacedSoundName);
    }
    
    public void AddBlueprint(Blueprint blueprint)
    {
        blueprintRepository.Add(blueprint);
    }
}