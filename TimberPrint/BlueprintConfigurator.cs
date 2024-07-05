using Bindito.Core;
using Timberborn.BottomBarSystem;

namespace TimberPrint;

[Context("Game")]
public class BlueprintConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<BlueprintRepository>().AsSingleton();
        containerDefinition.Bind<BlueprintPreviewPlacerFactory>().AsSingleton();
        containerDefinition.Bind<BlueprintService>().AsSingleton();
        containerDefinition.Bind<BlueprintPlacer>().AsSingleton();
        // containerDefinition.Bind<FakeTool>().AsSingleton();

        containerDefinition.Bind<BlueprintAreaSelectorButton>().AsSingleton();
        containerDefinition.Bind<BlueprintAreaSelectorTool>().AsSingleton();
        containerDefinition.MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
    }
    
    public class BottomBarModuleProvider : IProvider<BottomBarModule>
    {
        private readonly BlueprintAreaSelectorButton _blueprintAreaSelectorButton;

        public BottomBarModuleProvider(BlueprintAreaSelectorButton blueprintAreaSelectorButton)
        {
            _blueprintAreaSelectorButton = blueprintAreaSelectorButton;
        }

        public BottomBarModule Get()
        {
            var builder = new BottomBarModule.Builder();
            builder.AddRightSectionElement(_blueprintAreaSelectorButton);
            return builder.Build();
        }
    }
}