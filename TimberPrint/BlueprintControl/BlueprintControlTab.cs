using System.Collections.Generic;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.EntitySystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint.BlueprintControl;

public class BlueprintControlTab  : BatchControlTab
{
    private readonly BatchControlRowGroupFactory _batchControlRowGroupFactory;

    private readonly BlueprintRowItemFactory _blueprintRowItemFactory;

    private readonly BlueprintRepository _blueprintRepository;
    
    public override string TabNameLocKey => "Test A";
    
    public override string TabImage => "Migration";
    
    public override string BindingKey => "Test B";
    
    public BlueprintControlTab(VisualElementLoader visualElementLoader, BatchControlDistrict batchControlDistrict, BatchControlRowGroupFactory batchControlRowGroupFactory, BlueprintRowItemFactory blueprintRowItemFactory, BlueprintRepository blueprintRepository) : base(visualElementLoader, batchControlDistrict)
    {
        _batchControlRowGroupFactory = batchControlRowGroupFactory;
        _blueprintRowItemFactory = blueprintRowItemFactory;
        _blueprintRepository = blueprintRepository;
    }
    
    public override IEnumerable<BatchControlRowGroup> GetRowGroups(IEnumerable<EntityComponent> entities)
    {
        var batchControlRowGroup = _batchControlRowGroupFactory.CreateUnsorted(new BatchControlRow(new VisualElement()));

        foreach (var blueprint in _blueprintRepository.Blueprints)
        {
            batchControlRowGroup.AddRow(new BatchControlRow(new Label("A"), _blueprintRowItemFactory.Create(blueprint)));
        }
        
       
        
        yield return batchControlRowGroup;
    }
}