using TimberApi.UIBuilder.StyleSheetSystem;
using TimberApi.UIBuilder.UIBuilderSystem.ElementBuilders;
using UnityEngine.UIElements;
using UnityEngine.UIElements.StyleSheets;
using StyleSheetBuilder = TimberApi.UIBuilder.StyleSheetSystem.StyleSheetBuilder;

namespace TimberApi.UIBuilder.UIBuilderSystem.Presets.Buttons
{
    public class MinusButton : MinusButton<MinusButton>
    {
        protected override MinusButton BuilderInstance => this;
    }
    
    public abstract class MinusButton<TBuilder> : BaseBuilder<TBuilder, Button>
        where TBuilder : BaseBuilder<TBuilder, Button>
    {
        protected ButtonBuilder ButtonBuilder = null!;
        
        protected string ImageClass = "api__button__minus-button--normal";

        protected string SizeClass = "api__button__minus-button--size-normal";
        
        public TBuilder Small()
        {
            SizeClass = "api__button__minus-button--size-small";
            return BuilderInstance;
        }
        
        public TBuilder Large()
        {
            SizeClass = "api__button__minus-button--size-large";
            return BuilderInstance;
        }
        
        public TBuilder SetSize(Length size)
        {
            ButtonBuilder.SetHeight(size);
            ButtonBuilder.SetWidth(size);
            return BuilderInstance;
        }

        public TBuilder Active()
        {
            ButtonBuilder.AddClass("api__button__minus-button--active");
            return BuilderInstance;
        }

        public TBuilder Inverted()
        {
            ImageClass = "api__button__minus-button--inverted";
            return BuilderInstance;
        }

        protected override Button InitializeRoot()
        {
            ButtonBuilder = UIBuilder.Create<ButtonBuilder>();
            
            return ButtonBuilder.AddClass("api__button__minus-button").Build();
        }

        protected override void InitializeStyleSheet(StyleSheetBuilder styleSheetBuilder)
        {
            styleSheetBuilder
                .AddClickSoundClass("api__button__minus-button", "UI.Click")
                .AddBackgroundClass("api__button__minus-button", "ui/images/buttons/minus-hover", PseudoClass.Hover)
                .AddBackgroundClass("api__button__minus-button--normal", "ui/images/buttons/minus")
                .AddBackgroundClass("api__button__minus-button--inverted", "ui/images/buttons/minus-inverted")
                .AddBackgroundClass("api__button__minus-button--active", "ui/images/buttons/minus-active", PseudoClass.Active, PseudoClass.Hover)

                .AddClass("api__button__minus-button--size-normal", builder => builder
                    .Add(Property.Height, new Dimension(20, Dimension.Unit.Pixel))
                    .Add(Property.Width, new Dimension(20, Dimension.Unit.Pixel))
                )
                .AddClass("api__button__minus-button--size-small", builder => builder
                    .Add(Property.Height, new Dimension(18, Dimension.Unit.Pixel))
                    .Add(Property.Width, new Dimension(18, Dimension.Unit.Pixel))
                )
                .AddClass("api__button__minus-button--size-large", builder => builder
                    .Add(Property.Height, new Dimension(24, Dimension.Unit.Pixel))
                    .Add(Property.Width, new Dimension(24, Dimension.Unit.Pixel))
                );
        }

        public override Button Build()
        {
            return ButtonBuilder
                .AddClass(ImageClass)
                .AddClass(SizeClass)
                .Build();
        }
    }
}