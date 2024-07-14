using Bindito.Core;
using Timberborn.AreaSelectionSystem;

namespace TimberPrint.New.BlueprintSelectionSystem;

[Context("Game")]
public class BlockObjectSelectionConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<BlockObjectSelectionDrawerFactory>().AsSingleton();
        containerDefinition.Bind<TopRectangleBoundsDrawerFactory>().AsSingleton();
        containerDefinition.Bind<BlueprintBlockObjectPicker>().AsSingleton();
        containerDefinition.Bind<BlueprintAreaBlockObjectPickerFactory>().AsSingleton();
        containerDefinition.Bind<BlueprintStackedBlockObjectPicker>().AsSingleton();
    }
}
