using System.Collections.Generic;
using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using UnityEngine.UIElements;

namespace TimberPrint.BlueprintSharing;

public class BlueprintSharingTab  : BatchControlTab
{
    private readonly BatchControlRowGroupFactory _batchControlRowGroupFactory;
    
    public override string TabNameLocKey => "blueprint.tab.share";
    
    public override string TabImage => "Attractions";
    
    public override string BindingKey => "NoBinding";
    
    public BlueprintSharingTab(VisualElementLoader visualElementLoader, BatchControlDistrict batchControlDistrict, BatchControlRowGroupFactory batchControlRowGroupFactory) : base(visualElementLoader, batchControlDistrict)
    {
        _batchControlRowGroupFactory = batchControlRowGroupFactory;
    }
    
    public override IEnumerable<BatchControlRowGroup> GetRowGroups(IEnumerable<EntityComponent> entities)
    {
        var test = _batchControlRowGroupFactory.CreateUnsorted(new BatchControlRow(new VisualElement()));
        
        test.AddRow(new BatchControlRow(_visualElementLoader.LoadVisualElement("BlueprintShareTab")));

        yield return test;
    }
}