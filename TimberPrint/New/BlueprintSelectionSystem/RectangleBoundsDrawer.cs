using Timberborn.Common;
using Timberborn.Coordinates;
using Timberborn.Rendering;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class RectangleBoundsDrawer
{
    private readonly MeshDrawer _blockBottomMeshDrawer;
    private readonly NeighboredValues4<MeshDrawer> _blockSideMeshDrawers = new();

    public RectangleBoundsDrawer(
      MeshDrawer blockSideMeshDrawer0010,
      MeshDrawer blockSideMeshDrawer0011,
      MeshDrawer blockSideMeshDrawer0111,
      MeshDrawer blockSideMeshDrawer1010,
      MeshDrawer blockSideMeshDrawer1111,
      MeshDrawer blockBottomMeshDrawer)
    {
      
      
      _blockSideMeshDrawers.AddVariants(blockSideMeshDrawer0010, false, false, true, false);
      _blockSideMeshDrawers.AddVariants(blockSideMeshDrawer0011, false, false, true, true);
      _blockSideMeshDrawers.AddVariants(blockSideMeshDrawer0111, false, true, true, true);
      _blockSideMeshDrawers.AddVariants(blockSideMeshDrawer1010, true, false, true, false);
      _blockSideMeshDrawers.AddVariants(blockSideMeshDrawer1111, true, true, true, true);
      

      _blockBottomMeshDrawer = blockBottomMeshDrawer;
    }

    public void DrawOnLevel(Vector2Int start, Vector2Int end, int level)
    {
      var (min, max) = Vectors.MinMax(start, end);
      for (var x = min.x; x <= max.x; ++x)
      {
        for (var y = min.y; y <= max.y; ++y)
        {
          var block = ProjectOnLevel(new Vector2Int(x, y), level);
          DrawBottom(block);
          DrawSides(block, min, max, level);
        }
      }
    }

    private void DrawBottom(Vector3Int block)
    {
      _blockBottomMeshDrawer.DrawAtCoordinates(block, 0.02f);
    }

    private void DrawSides(Vector3Int block, Vector2Int min, Vector2Int max, int minLevel)
    {
      var down = VisibleSide(block, Vector2Int.down, min, max, minLevel);
      var left = VisibleSide(block, Vector2Int.left, min, max, minLevel);
      var up = VisibleSide(block, Vector2Int.up, min, max, minLevel);
      var right = VisibleSide(block, Vector2Int.right, min, max, minLevel);
      if (!(down | left | up | right))
        return;
      
      var (meshDrawer, orientation) = _blockSideMeshDrawers.GetMatch(down, left, up, right);
      var quaternion = Quaternion.AngleAxis(orientation.ToAngle(), Vector3.up);
      meshDrawer.DrawAtCoordinates(block, 0.02f, quaternion);
    }
    
    private static bool VisibleSide(
      Vector3Int block,
      Vector2Int neighborOffset,
      Vector2Int min,
      Vector2Int max,
      int minLevel)
    {
      var vector2Int = block.XY() + neighborOffset;
      var vector3Int = ProjectOnLevel(vector2Int, minLevel);
      return !InBounds(vector2Int, min, max) || block.z != vector3Int.z;
    }
    
    private static Vector3Int ProjectOnLevel(Vector2Int block, int level)
    {
      return new Vector3Int(block.x, block.y, level);
    }
    
    private static bool InBounds(Vector2Int coordinates, Vector2Int min, Vector2Int max)
    {
      return coordinates.x >= min.x && coordinates.x <= max.x && coordinates.y >= min.y && coordinates.y <= max.y;
    }
}