using Bindito.Core;

namespace TimberPrint.BlueprintControl;

[Context("Game")]
public class BlueprintControlConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<BlueprintControlTab>().AsSingleton();
        containerDefinition.Bind<BlueprintRowItemFactory>().AsSingleton();
    }
}