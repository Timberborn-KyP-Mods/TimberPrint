using System.Collections.Generic;
using System.Linq;
using Timberborn.AreaSelectionSystem;
using Timberborn.BlockSystem;
using Timberborn.Common;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlueprintStackedBlockObjectPicker(
    AreaIterator areaIterator,
    BlockService blockService)
{
    private readonly HashSet<BlockObject> _blockObjects = [];

    private int _maxHeight;

    internal IEnumerable<BlockObject> GetBlockObjectAndStacked(BlockObject? startBlockObject, BlockObjectPickDirection pickDirection, BlockObjectPickerFilter selectionFilter)
    {
        _blockObjects.Clear();
        if (startBlockObject != null && selectionFilter.IsValid(startBlockObject))
        {
            AddBlockObjectsRecursively(startBlockObject, pickDirection);
        }

        return _blockObjects.AsReadOnlyEnumerable();
    }

    internal IEnumerable<BlockObject> GetBlockObjectsInAreaAndStacked(Vector3Int start, Vector3Int end, BlockObject? startBlockObject, BlockObjectPickDirection pickDirection, BlockObjectPickerFilter selectionFilter, int maxHeight)
    {
        _blockObjects.Clear();
        _maxHeight = maxHeight;
        if (startBlockObject != null && selectionFilter.IsValid(startBlockObject))
        {
            AddBlockObjectsRecursively(startBlockObject, pickDirection);
        }

        foreach (var item in GetBlockObjectsInCuboid(start, end).Where(selectionFilter.IsValid))
        {
            AddBlockObjectsRecursively(item, pickDirection);
        }

        return _blockObjects.AsReadOnlyEnumerable();
    }

    private IEnumerable<BlockObject> GetBlockObjectsInCuboid(Vector3Int start, Vector3Int end)
    {
        return (from coords in areaIterator.GetCuboid(start, end)
            where blockService.AnyObjectAt(coords)
            select coords).SelectMany(coords => blockService.GetObjectsAt(coords)).Distinct();
    }

    private void AddBlockObjectsRecursively(BlockObject blockObject, BlockObjectPickDirection pickDirection)
    {
        if (_blockObjects.Add(blockObject))
        {
            AddConnectedBlockObjects(blockObject, pickDirection);
        }
    }

    private void AddConnectedBlockObjects(BlockObject blockObject, BlockObjectPickDirection pickDirection)
    {
        foreach (var item in from block in blockObject.PositionedBlocks.GetAllBlocks()
                 where pickDirection != BlockObjectPickDirection.Downwards
                     ? block.Stackable.IsStackable()
                     : block.IsFoundationBlock
                 select block)
        {
            AddValidBlockObjectsStackedWithBlock(item, pickDirection);
        }
    }

    private void AddValidBlockObjectsStackedWithBlock(Block block, BlockObjectPickDirection pickDirection)
    {
        var z = pickDirection == BlockObjectPickDirection.Upwards ? 1 : -1;
        var coordinates = block.Coordinates + new Vector3Int(0, 0, z);

        if (coordinates.z >= _maxHeight)
        {
            return;
        }
        
        foreach (var item in blockService.GetObjectsAt(coordinates))
        {
            if (ShouldIncludeNearBlock(item.PositionedBlocks.GetBlock(coordinates), pickDirection))
            {
                AddBlockObjectsRecursively(item, pickDirection);
            }
        }
    }

    private static bool ShouldIncludeNearBlock(Block block, BlockObjectPickDirection direction)
    {
        if (direction != 0)
        {
            return block.Stackable.IsStackable();
        }

        return block.IsFoundationBlock;
    }
}