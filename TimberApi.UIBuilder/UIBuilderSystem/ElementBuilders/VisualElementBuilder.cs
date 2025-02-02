using Timberborn.CoreUI;

namespace TimberApi.UIBuilder.UIBuilderSystem.ElementBuilders
{
    public class VisualElementBuilder : BaseElementBuilder<VisualElementBuilder, NineSliceVisualElement>
    {
        protected override VisualElementBuilder BuilderInstance => this;
        protected override NineSliceVisualElement InitializeRoot()
        {
            return new NineSliceVisualElement();
        }
    }
}