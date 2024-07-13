using Timberborn.BatchControl;
using Timberborn.CoreUI;
using Timberborn.DropdownSystem;

namespace TimberPrint.BlueprintControl;

public class BlueprintRowItemFactory(
    VisualElementLoader visualElementLoader,
    BlueprintManager blueprintManager,
    BlueprintRepository blueprintRepository,
    DropdownItemsSetter dropdownItemsSetter)
{
    public IBatchControlRowItem Create(Blueprint blueprint)
    {
        var row = visualElementLoader.LoadVisualElement("BlueprintItemRow");

        return new BlueprintControlRowItem(row, blueprint, blueprintManager, blueprintRepository, visualElementLoader, dropdownItemsSetter);
    }
}