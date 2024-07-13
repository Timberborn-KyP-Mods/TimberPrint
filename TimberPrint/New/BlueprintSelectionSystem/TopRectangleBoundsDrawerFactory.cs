using Mods.TimberPrint.Scripts;
using Timberborn.AreaSelectionSystem;
using Timberborn.Rendering;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class TopRectangleBoundsDrawerFactory(
    RectangleBoundsDrawerFactory rectangleBoundsDrawerFactory,
    TopRectangleBoundsMeshes topRectangleBoundsMeshes,
    MeshDrawerFactory meshDrawerFactory)
{
    private readonly Material _blockSideMaterial = rectangleBoundsDrawerFactory._blockSideMaterial;
    
    private readonly Mesh _blockBottomMesh = rectangleBoundsDrawerFactory._blockBottomMesh;
    
    private readonly Material _blockBottomMaterial = rectangleBoundsDrawerFactory._blockBottomMaterial;

    public RectangleBoundsDrawer Create(Color tileColor, Color blockSideColor)
    {
        return new RectangleBoundsDrawer(
            meshDrawerFactory.Create(topRectangleBoundsMeshes.blockSideMesh0010, _blockSideMaterial, blockSideColor),
            meshDrawerFactory.Create(topRectangleBoundsMeshes.blockSideMesh0011, _blockSideMaterial, blockSideColor),
            
            meshDrawerFactory.Create(topRectangleBoundsMeshes.blockSideMesh0111, _blockSideMaterial, blockSideColor),
            meshDrawerFactory.Create(topRectangleBoundsMeshes.blockSideMesh1010, _blockSideMaterial, blockSideColor),
            
            meshDrawerFactory.Create(topRectangleBoundsMeshes.blockSideMesh1111, _blockSideMaterial, blockSideColor),
            meshDrawerFactory.Create(_blockBottomMesh, _blockBottomMaterial, tileColor)
        );
    }
}