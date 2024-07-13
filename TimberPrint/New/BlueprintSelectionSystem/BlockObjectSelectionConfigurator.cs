using Bindito.Core;

namespace TimberPrint.New.BlueprintSelectionSystem;

[Context("Game")]
public class BlockObjectSelectionConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<BlockObjectSelectionDrawerFactory>().AsSingleton();
        containerDefinition.Bind<RectangleBoundsDrawerFactory>().AsSingleton();
    }
}
