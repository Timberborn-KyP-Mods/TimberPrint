using Bindito.Core;

namespace TimberPrint.BlueprintSideMenu;

[Context("Game")]
public class BlueprintSideMenuConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<BlueprintSideMenuBox>().AsSingleton();
        containerDefinition.Bind<BlueprintSideMenuTabController>().AsSingleton();
    }
}