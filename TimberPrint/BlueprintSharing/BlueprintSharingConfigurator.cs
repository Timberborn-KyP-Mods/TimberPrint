using Bindito.Core;

namespace TimberPrint.BlueprintSharing;

[Context("Game")]
public class BlueprintSharingConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<BlueprintSharingTab>().AsSingleton();
    }
}