using Bindito.Core;
using TimberApi.UIBuilder.StyleSheetSystem;
using TimberApi.UIBuilder.UIBuilderSystem.ElementBuilders;

namespace TimberApi.UIBuilder.UIBuilderSystem
{
    [Context("MainMenu")]
    [Context("Game")]
    public class UiBuilderSystemConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<DependencyContainer>().AsSingleton();
            
            containerDefinition.Bind<UIBuilderOld>().AsSingleton();
            
            containerDefinition.Bind<LabelBuilder>().AsTransient();
            containerDefinition.Bind<SliderBuilder>().AsTransient();

            containerDefinition.Bind<ButtonBuilder>().AsTransient();
            containerDefinition.Bind<LocalizableButtonBuilder>().AsTransient();
            containerDefinition.Bind<VisualElementBuilder>().AsTransient();
            
            containerDefinition.Bind<ScrollViewBuilder>().AsTransient();
            containerDefinition.Bind<StyleSheetBuilder>().AsTransient();
            containerDefinition.Bind<UIBuilder>().AsSingleton();
            containerDefinition.Bind<BuilderStyleSheetCache>().AsSingleton();
        }
    }
}
