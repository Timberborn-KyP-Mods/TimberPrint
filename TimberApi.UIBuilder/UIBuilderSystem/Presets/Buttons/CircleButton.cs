using TimberApi.UIBuilder.StyleSheetSystem;
using TimberApi.UIBuilder.UIBuilderSystem.ElementBuilders;
using UnityEngine.UIElements;
using UnityEngine.UIElements.StyleSheets;
using StyleSheetBuilder = TimberApi.UIBuilder.StyleSheetSystem.StyleSheetBuilder;

namespace TimberApi.UIBuilder.UIBuilderSystem.Presets.Buttons
{
    public class CircleButton : CircleButton<CircleButton>
    {
        protected override CircleButton BuilderInstance => this;
    }
    
    public abstract class CircleButton<TBuilder> : BaseBuilder<TBuilder, Button>
        where TBuilder : BaseBuilder<TBuilder, Button>
    {
        protected ButtonBuilder ButtonBuilder = null!;
        
        protected string SizeClass = "api__button__circle-button--size-normal";
        
        public TBuilder Small()
        {
            SizeClass = "api__button__circle-button--size-small";
            return BuilderInstance;
        }
        
        public TBuilder Large()
        {
            SizeClass = "api__button__circle-button--size-large";
            return BuilderInstance;
        }
        
        public TBuilder Active()
        {
            ButtonBuilder.AddClass("api__button__circle-button--active");
            return BuilderInstance;
        }
        
        public TBuilder SetSize(Length size)
        {
            ButtonBuilder.SetHeight(size);
            ButtonBuilder.SetWidth(size);
            return BuilderInstance;
        }

        protected override Button InitializeRoot()
        {
            ButtonBuilder = UIBuilder.Create<ButtonBuilder>();
            
            return ButtonBuilder.AddClass("api__button__circle-button").Build();
        }

        protected override void InitializeStyleSheet(StyleSheetBuilder styleSheetBuilder)
        {
            styleSheetBuilder
                .AddClickSoundClass("api__button__circle-button", "UI.Click")
                .AddBackgroundHoverClass("api__button__circle-button", "ui/images/buttons/circle-empty", "ui/images/buttons/circle-empty-hover")
                .AddBackgroundClass("api__button__circle-button--active", "ui/images/buttons/circle-empty-active", PseudoClass.Active, PseudoClass.Hover)
                .AddClass("api__button__circle-button--size-normal", builder => builder
                    .Add(Property.Height, new Dimension(22, Dimension.Unit.Pixel))
                    .Add(Property.Width, new Dimension(22, Dimension.Unit.Pixel))
                )
                .AddClass("api__button__circle-button--size-small", builder => builder
                    .Add(Property.Height, new Dimension(20, Dimension.Unit.Pixel))
                    .Add(Property.Width, new Dimension(20, Dimension.Unit.Pixel))
                )
                .AddClass("api__button__circle-button--size-large", builder => builder
                    .Add(Property.Height, new Dimension(24, Dimension.Unit.Pixel))
                    .Add(Property.Width, new Dimension(24, Dimension.Unit.Pixel))
                );
        }

        public override Button Build()
        {
            return ButtonBuilder
                .AddClass(SizeClass)
                .Build();
        }
    }
}