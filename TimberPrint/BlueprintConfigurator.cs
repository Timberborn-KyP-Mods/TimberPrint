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

        containerDefinition.Bind<BlueprintPlacerTool>().AsSingleton();
        containerDefinition.Bind<BlueprintPlacerButton>().AsSingleton();
        containerDefinition.Bind<BlueprintAreaSelectorButton>().AsSingleton();
        containerDefinition.Bind<BlueprintAreaSelectorTool>().AsSingleton();
        containerDefinition.MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
    }
    
    public class BottomBarModuleProvider : IProvider<BottomBarModule>
    {
        private readonly BlueprintAreaSelectorButton _blueprintAreaSelectorButton;
        
        private readonly BlueprintPlacerButton _blueprintPlacerButton;

        public BottomBarModuleProvider(BlueprintAreaSelectorButton blueprintAreaSelectorButton, BlueprintPlacerButton blueprintPlacerButton)
        {
            _blueprintAreaSelectorButton = blueprintAreaSelectorButton;
            _blueprintPlacerButton = blueprintPlacerButton;
        }

        public BottomBarModule Get()
        {
            var builder = new BottomBarModule.Builder();
            builder.AddRightSectionElement(_blueprintAreaSelectorButton);
            builder.AddRightSectionElement(_blueprintPlacerButton);
            return builder.Build();
        }
    }
}