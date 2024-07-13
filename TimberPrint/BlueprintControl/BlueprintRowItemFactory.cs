using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;

namespace TimberPrint.BlueprintControl;

public class BlueprintRowItemFactory
{
    private readonly VisualElementLoader _visualElementLoader;
    
    private readonly BlueprintManager _blueprintManager;

    private readonly BlueprintRepository _blueprintRepository;

    private readonly DropdownItemsSetter _dropdownItemsSetter;

    public BlueprintRowItemFactory(VisualElementLoader visualElementLoader, BlueprintManager blueprintManager, BlueprintRepository blueprintRepository, DropdownItemsSetter dropdownItemsSetter)
    {
        _visualElementLoader = visualElementLoader;
        _blueprintManager = blueprintManager;
        _blueprintRepository = blueprintRepository;
        _dropdownItemsSetter = dropdownItemsSetter;
    }

    public IBatchControlRowItem Create(Blueprint blueprint)
    {
        var row = _visualElementLoader.LoadVisualElement("BlueprintItemRow");

        return new BlueprintControlRowItem(row, blueprint, _blueprintManager, _blueprintRepository, _visualElementLoader, _dropdownItemsSetter);
    }
}