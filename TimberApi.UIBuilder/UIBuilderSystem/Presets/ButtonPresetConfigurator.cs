using Bindito.Core;
using TimberApi.UIBuilder.UIBuilderSystem.Presets.Buttons;

namespace TimberApi.UIBuilder.UIBuilderSystem.Presets
{
    [Context("Game")]
    public class ButtonPresetConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<ArrowDownButton>().AsTransient();
            containerDefinition.Bind<ArrowLeftButton>().AsTransient();
            containerDefinition.Bind<ArrowUpButton>().AsTransient();
            containerDefinition.Bind<ArrowRightButton>().AsTransient();
            containerDefinition.Bind<ButtonMenu>().AsTransient();
            containerDefinition.Bind<ButtonGame>().AsTransient();
            containerDefinition.Bind<ButtonEmpty>().AsTransient();
            containerDefinition.Bind<ButtonEmptyText>().AsTransient();
            containerDefinition.Bind<ButtonNewGame>().AsTransient();
            containerDefinition.Bind<ClampUp>().AsTransient();
            containerDefinition.Bind<ClampDown>().AsTransient();
            containerDefinition.Bind<PlusButton>().AsTransient();
            containerDefinition.Bind<PlusBatchButton>().AsTransient();
            containerDefinition.Bind<PlusBatchMultiButton>().AsTransient();
            containerDefinition.Bind<MinusButton>().AsTransient();
            containerDefinition.Bind<MinusBatchButton>().AsTransient();
            containerDefinition.Bind<MinusBatchMultiButton>().AsTransient();
            containerDefinition.Bind<CircleButton>().AsTransient();
            containerDefinition.Bind<CloseButton>().AsTransient();
            containerDefinition.Bind<CrossButton>().AsTransient();
            containerDefinition.Bind<WideButton>().AsTransient();
            containerDefinition.Bind<MigrationArrowLeftButton>().AsTransient();
            containerDefinition.Bind<MigrationArrowRightButton>().AsTransient();
            containerDefinition.Bind<CyclerRight>().AsTransient();
            containerDefinition.Bind<CyclerLeft>().AsTransient();
            containerDefinition.Bind<CyclerMainRight>().AsTransient();
            containerDefinition.Bind<CyclerMainLeft>().AsTransient();
        }
    }
}
