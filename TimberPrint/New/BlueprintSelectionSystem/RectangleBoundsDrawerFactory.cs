using Mods.TimberPrint.Scripts;
using Timberborn.Rendering;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class RectangleBoundsDrawerFactory
{
    private readonly Material _blockSideMaterial;
    
    private readonly Mesh _blockBottomMesh;
    
    private readonly Material _blockBottomMaterial;

    private readonly MeshDrawerFactory _meshDrawerFactory;

    private readonly TopRectangleBoundsMeshes _topRectangleBoundsMeshes;

    public RectangleBoundsDrawerFactory(
        Timberborn.AreaSelectionSystem.RectangleBoundsDrawerFactory rectangleBoundsDrawerFactory,
        MeshDrawerFactory meshDrawerFactory, TopRectangleBoundsMeshes topRectangleBoundsMeshes)
    {
        _blockSideMaterial = rectangleBoundsDrawerFactory._blockSideMaterial;
        _blockBottomMesh = rectangleBoundsDrawerFactory._blockBottomMesh;
        _blockBottomMaterial = rectangleBoundsDrawerFactory._blockBottomMaterial;
            
        _meshDrawerFactory = meshDrawerFactory;
        _topRectangleBoundsMeshes = topRectangleBoundsMeshes;
    }

    public RectangleBoundsDrawer Create(Color tileColor, Color blockSideColor)
    {
        return new RectangleBoundsDrawer(
            _meshDrawerFactory.Create(_topRectangleBoundsMeshes.blockSideMesh0010, _blockSideMaterial, Color.magenta),
            _meshDrawerFactory.Create(_topRectangleBoundsMeshes.blockSideMesh0011, _blockSideMaterial, Color.blue),
            
            _meshDrawerFactory.Create(_topRectangleBoundsMeshes.blockSideMesh0111, _blockSideMaterial, blockSideColor),
            _meshDrawerFactory.Create(_topRectangleBoundsMeshes.blockSideMesh1010, _blockSideMaterial, blockSideColor),
            
            _meshDrawerFactory.Create(_topRectangleBoundsMeshes.blockSideMesh1111, _blockSideMaterial, blockSideColor),
            _meshDrawerFactory.Create(_blockBottomMesh, _blockBottomMaterial, tileColor)
        );
    }
}