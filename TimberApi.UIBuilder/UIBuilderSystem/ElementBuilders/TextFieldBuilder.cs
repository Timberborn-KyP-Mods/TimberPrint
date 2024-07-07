using Timberborn.CoreUI;

namespace TimberApi.UIBuilder.UIBuilderSystem.ElementBuilders
{
    public class TextFieldBuilder : BaseElementBuilder<TextFieldBuilder, NineSliceTextField>
    {
        protected override TextFieldBuilder BuilderInstance => this;

        public TextFieldBuilder SetMultiLine(bool isMultiLine)
        {
            Root.multiline = isMultiLine;
            return this;
        }

        protected override NineSliceTextField InitializeRoot()
        {
            return new NineSliceTextField();
        }
    }
}
