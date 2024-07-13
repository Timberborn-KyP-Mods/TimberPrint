using System.Collections.Generic;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;
using Timberborn.EntitySystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimberPrint.BlueprintControl;

public class BlueprintControlTab(
    VisualElementLoader visualElementLoader,
    BatchControlDistrict batchControlDistrict,
    BatchControlRowGroupFactory batchControlRowGroupFactory,
    BlueprintRowItemFactory blueprintRowItemFactory,
    BlueprintRepository blueprintRepository)
    : BatchControlTab(visualElementLoader, batchControlDistrict)
{
    public override string TabNameLocKey => "Test A";
    
    public override string TabImage => "Migration";
    
    public override string BindingKey => "Test B";

    public override IEnumerable<BatchControlRowGroup> GetRowGroups(IEnumerable<EntityComponent> entities)
    {
        var batchControlRowGroup = batchControlRowGroupFactory.CreateUnsorted(new BatchControlRow(new VisualElement()));

        foreach (var blueprint in blueprintRepository.Blueprints)
        {
            batchControlRowGroup.AddRow(new BatchControlRow(new Label("A"), blueprintRowItemFactory.Create(blueprint)));
        }
        
       
        
        yield return batchControlRowGroup;
    }
}