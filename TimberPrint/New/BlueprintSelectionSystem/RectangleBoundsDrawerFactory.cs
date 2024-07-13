using Timberborn.AssetSystem;
using Timberborn.PrefabSystem;
using Timberborn.Rendering;
using UnityEngine;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class RectangleBoundsDrawerFactory
{
    private readonly Mesh _blockSideMesh0010;
    
    private readonly Mesh _blockSideMesh0011;
    
    private readonly Mesh _blockSideMesh0111;
    
    private readonly Mesh _blockSideMesh1010;
    
    private readonly Mesh _blockSideMesh1111;
    
    private readonly Material _blockSideMaterial;
    
    private readonly Mesh _blockBottomMesh;
    
    private readonly Material _blockBottomMaterial;

    private readonly MeshDrawerFactory _meshDrawerFactory;

    private readonly IAssetLoader _assetLoader;

    public RectangleBoundsDrawerFactory(
        Timberborn.AreaSelectionSystem.RectangleBoundsDrawerFactory rectangleBoundsDrawerFactory,
        MeshDrawerFactory meshDrawerFactory, 
        IAssetLoader assetLoader
        )
    {
        _blockSideMesh0010 = rectangleBoundsDrawerFactory._blockSideMesh0010;
        _blockSideMesh0011 = rectangleBoundsDrawerFactory._blockSideMesh0011;
        _blockSideMesh0111 = rectangleBoundsDrawerFactory._blockSideMesh0111;
        _blockSideMesh1010 = rectangleBoundsDrawerFactory._blockSideMesh1010;
        _blockSideMesh1111 = rectangleBoundsDrawerFactory._blockSideMesh1111;
        _blockSideMaterial = rectangleBoundsDrawerFactory._blockSideMaterial;
        _blockBottomMesh = rectangleBoundsDrawerFactory._blockBottomMesh;
        _blockBottomMaterial = rectangleBoundsDrawerFactory._blockBottomMaterial;
            
        _meshDrawerFactory = meshDrawerFactory;
        _assetLoader = assetLoader;
    }

    public RectangleBoundsDrawer Create(Color tileColor, Color blockSideColor)
    {
        var blockSide0010 = _assetLoader.Load<GameObject>("Mesh/BlockSide0010").GetComponent<MeshFilter>().mesh;
        var blockSide0011 = _assetLoader.Load<GameObject>("Mesh/BlockSide0011").GetComponent<MeshFilter>().mesh;
        var blockSide0111 = _assetLoader.Load<GameObject>("Mesh/BlockSide0111").GetComponent<MeshFilter>().mesh;

        
        return new RectangleBoundsDrawer(
            _meshDrawerFactory.Create(blockSide0010, _blockSideMaterial, Color.magenta),
            _meshDrawerFactory.Create(blockSide0011, _blockSideMaterial, Color.blue),
            
            _meshDrawerFactory.Create(blockSide0111, _blockSideMaterial, blockSideColor),
            _meshDrawerFactory.Create(_blockSideMesh1010, _blockSideMaterial, blockSideColor),
            
            _meshDrawerFactory.Create(_blockSideMesh1111, _blockSideMaterial, blockSideColor),
            _meshDrawerFactory.Create(_blockBottomMesh, _blockBottomMaterial, tileColor)
        );
    }
}