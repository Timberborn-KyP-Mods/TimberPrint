using Timberborn.AreaSelectionSystem;

namespace TimberPrint.New.BlueprintSelectionSystem;

public class BlueprintAreaBlockObjectPickerFactory(
    AreaSelectionControllerFactory areaSelectionControllerFactory,
    AreaSelector areaSelector,
    BlueprintBlockObjectPicker blockObjectPicker)
{
    public BlueprintAreaBlockObjectPicker CreatePickingUpwards()
    {
        return Create(BlockObjectPickDirection.Upwards);
    }

    public BlueprintAreaBlockObjectPicker CreatePickingDownwards()
    {
        return Create(BlockObjectPickDirection.Downwards);
    }

    private BlueprintAreaBlockObjectPicker Create(BlockObjectPickDirection pickDirection)
    {
        return new BlueprintAreaBlockObjectPicker(areaSelectionControllerFactory.Create(), areaSelector, blockObjectPicker, pickDirection);
    }
}