using System.Collections.Generic;
using System.Linq;
using Timberborn.AreaSelectionSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using Timberborn.LevelVisibilitySystem;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlueprintBlockObjectPicker(
    BlueprintStackedBlockObjectPicker blueprintStackedBlockObjectPicker,
    ILevelVisibilityService levelVisibilityService)
{
    internal IEnumerable<BlockObject> PickBlockObjects(SelectionStart selectionStart, Vector3Int endCoords, BlockObjectPickDirection pickDirection, bool selectingArea, int maxHeight)
    {
        var coordinates = selectionStart.Coordinates;
        var blockObjectHit = selectionStart.GetBlockObjectHit();
        var blockObject = blockObjectHit?.BlockObject;
        if (blockObject != null)
        {
            Debug.LogWarning(blockObject.BaseZ);
            if (!selectingArea && !IsStackable(blockObject) && pickDirection == BlockObjectPickDirection.Upwards)
            {
                return Enumerables.One(blockObject);
            }
            if (blockObject.BaseZ > 0)
            {
                coordinates.z = blockObject.CoordinatesAtBaseZ.z;
            }
        }
        
        var selectionFilter = blockObjectHit.HasValue ? BlockObjectPickerFilter.CreateWithConstraints(blockObject, coordinates, levelVisibilityService.MaxVisibleLevel) : BlockObjectPickerFilter.Create(coordinates.z);
        
        if (selectingArea)
        {
            return blueprintStackedBlockObjectPicker.GetBlockObjectsInAreaAndStacked(coordinates, endCoords, blockObject, pickDirection, selectionFilter, maxHeight);
        }
        
        return blueprintStackedBlockObjectPicker.GetBlockObjectAndStacked(blockObject, pickDirection, selectionFilter);
    }

    private static bool IsStackable(BlockObject blockObject)
    {
        return blockObject.PositionedBlocks.GetAllBlocks().Any(block => block.Stackable.IsStackable());
    }
}