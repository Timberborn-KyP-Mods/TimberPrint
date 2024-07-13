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
        containerDefinition.Bind<BlueprintCompressor>().AsSingleton();
        containerDefinition.Bind<BluePrintSharing>().AsSingleton();
        containerDefinition.Bind<BlueprintManager>().AsSingleton();

        containerDefinition.Bind<BlueprintPlacerTool>().AsSingleton();
        containerDefinition.Bind<BlueprintPlacerButton>().AsSingleton();
        containerDefinition.Bind<BlueprintAreaSelectorButton>().AsSingleton();
        containerDefinition.Bind<BlueprintAreaSelectorTool>().AsSingleton();
        containerDefinition.MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
    }
    
    public class BottomBarModuleProvider(
        BlueprintAreaSelectorButton blueprintAreaSelectorButton,
        BlueprintPlacerButton blueprintPlacerButton)
        : IProvider<BottomBarModule>
    {
        public BottomBarModule Get()
        {
            var builder = new BottomBarModule.Builder();
            builder.AddRightSectionElement(blueprintAreaSelectorButton);
            builder.AddRightSectionElement(blueprintPlacerButton);
            return builder.Build();
        }
    }
}