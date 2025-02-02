using System;
using TimberApi.UIBuilder.UIBuilderSystem.StylingElements;
using UnityEngine.UIElements;

namespace TimberApi.UIBuilder.UIBuilderSystem.ElementBuilders
{
    public abstract class BaseElementBuilder<TBuilder, TElement> : BaseBuilder<TBuilder, TElement>
        where TBuilder : BaseElementBuilder<TBuilder, TElement>
        where TElement : VisualElement, new()
    {
        public TBuilder AddStyleSheet(StyleSheet styleSheet)
        {
            Root.styleSheets.Add(styleSheet);
            return BuilderInstance;
        }

        public TBuilder AddClass(string className)
        {
            Root.AddToClassList(className);
            return BuilderInstance;
        }

        public TBuilder RemoveClass(string className)
        {
            Root.RemoveFromClassList(className);
            return BuilderInstance;
        }
        
        public TBuilder AddComponent<TComponentBuilder>(string name, Func<TComponentBuilder, TComponentBuilder> builder) where TComponentBuilder : BaseBuilder
        {
            var component = builder.Invoke(UIBuilder.Create<TComponentBuilder>(name));
            
            Root.Add(component.BuildElement());
            return BuilderInstance;
        }
        
        public TBuilder AddComponent<TComponentBuilder>(Func<TComponentBuilder, TComponentBuilder> builder) where TComponentBuilder : BaseBuilder
        {
            var component = builder.Invoke(UIBuilder.Create<TComponentBuilder>());
            
            Root.Add(component.BuildElement());
            return BuilderInstance;
        }
        
        public TBuilder AddComponent<TComponentBuilder>(string name) where TComponentBuilder : BaseBuilder
        {
            Root.Add(UIBuilder.Build<TComponentBuilder>(name));
            return BuilderInstance;
        }
        
        public TBuilder AddComponent<TComponentBuilder>() where TComponentBuilder : BaseBuilder
        {
            Root.Add(UIBuilder.Build<TComponentBuilder>());
            return BuilderInstance;
        }
        
        public TBuilder AddComponent(string name, Type builderType)
        {
            Root.Add(UIBuilder.Build(name, builderType));
            return BuilderInstance;
        }
        
        public TBuilder AddComponent(Type builderType)
        {
            Root.Add(UIBuilder.Build(builderType));
            return BuilderInstance;
        }

        public TBuilder AddComponent(VisualElement element)
        {
            Root.Add(element);
            return BuilderInstance;
        }

        public TBuilder SetWidth(Length width)
        {
            Root.style.width = width;
            return BuilderInstance;
        }

        public TBuilder SetHeight(Length height)
        {
            Root.style.height = height;
            return BuilderInstance;
        }

        public TBuilder SetStyle(Action<IStyle> style)
        {
            style.Invoke(Root.style);
            return BuilderInstance;
        }

        public TBuilder SetMargin(Margin margin)
        {
            Root.style.marginLeft = margin.Left;
            Root.style.marginTop = margin.Top;
            Root.style.marginRight = margin.Right;
            Root.style.marginBottom = margin.Bottom;
            return BuilderInstance;
        }

        public TBuilder SetPadding(Padding padding)
        {
            Root.style.paddingLeft = padding.Left;
            Root.style.paddingTop = padding.Top;
            Root.style.paddingRight = padding.Right;
            Root.style.paddingBottom = padding.Bottom;
            return BuilderInstance;
        }

        public TBuilder SetJustifyContent(Justify justify)
        {
            Root.style.justifyContent = justify;
            return BuilderInstance;
        }

        public TBuilder SetAlignItems(Align align)
        {
            Root.style.alignItems = align;
            return BuilderInstance;
        }

        public TBuilder SetAlignContent(Align align)
        {
            Root.style.alignContent = align;
            return BuilderInstance;
        }

        public TBuilder SetFlexWrap(Wrap wrap)
        {
            Root.style.flexWrap = wrap;
            return BuilderInstance;
        }

        public TBuilder SetFlexDirection(FlexDirection direction)
        {
            Root.style.flexDirection = direction;
            return BuilderInstance;
        }

        public TBuilder SetBackgroundImage(StyleBackground backgroundImage)
        {
            Root.style.backgroundImage = backgroundImage;
            return BuilderInstance;
        }

        public TBuilder SetBackgroundColor(StyleColor color)
        {
            Root.style.backgroundColor = color;
            return BuilderInstance;
        }

        public TBuilder ModifyElement(Action<TElement> elementAction)
        {
            elementAction.Invoke(Root);
            return BuilderInstance;
        }
    }
}