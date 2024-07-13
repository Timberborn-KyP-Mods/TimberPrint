using System.Collections.Generic;
using System.Linq;
using Timberborn.BlockSystem;
using Timberborn.PrefabSystem;
using Timberborn.PreviewSystem;
using UnityEngine;

namespace TimberPrint;

public class BlueprintPreviewPlacerFactory(
    PreviewFactory previewFactory,
    PrefabNameMapper prefabNameMapper,
    PreviewShower previewShower,
    BlockValidator blockValidator,
    BlockService blockService,
    PreviewValidationService previewValidationService)
{
    public BlueprintPreviewPlacer Create(Blueprint blueprint)
    {
        var previews = CreatePreviews(blueprint.BlueprintItems);

        return new BlueprintPreviewPlacer(
            previewShower,
            blockValidator,
            blockService,
            previewValidationService,
            true,
            previews,
            blueprint.BlueprintItems.Select(item => item.Placement).ToArray(),
            blueprint,
            GetPlaceableBlockObjectPreviewHandler(blueprint.BlueprintItems)
        );
    }

    private Preview[] CreatePreviews(IReadOnlyList<BlueprintItem> blueprintItems)
    {
        var previews = new Preview[blueprintItems.Count];

        for (var index = 0; index < blueprintItems.Count; index++)
        {
            var prefab = prefabNameMapper.GetPrefab(blueprintItems[index].TemplateName);
            var preview = previewFactory.Create(prefab.GetComponentFast<PlaceableBlockObject>());
            preview.GetComponentFast<BlockObject>().MarkAsPreview();
            previews[index] = preview;
        }

        return previews;
    }
    
    private PlaceableBlockObject GetPlaceableBlockObjectPreviewHandler(IReadOnlyList<BlueprintItem> blueprintItems)
    {
        return prefabNameMapper.GetPrefab(blueprintItems[0].TemplateName).GetComponentFast<PlaceableBlockObject>();
    }
}