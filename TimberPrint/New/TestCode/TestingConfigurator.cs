using Bindito.Core;
using Timberborn.BottomBarSystem;

namespace TimberPrint.New.TestCode;

[Context("Game")]
public class TestingConfigurator : IConfigurator
{
    public void Configure(IContainerDefinition containerDefinition)
    {
        containerDefinition.Bind<MouseTestingTool>().AsSingleton();
        containerDefinition.Bind<MouseTestingButton>().AsSingleton();
        containerDefinition.MultiBind<BottomBarModule>().ToProvider<BottomBarModuleProvider>().AsSingleton();
    }
    
    public class BottomBarModuleProvider(MouseTestingButton mouseTestingButton) : IProvider<BottomBarModule>
    {
        public BottomBarModule Get()
        {
            var builder = new BottomBarModule.Builder();
            
            builder.AddRightSectionElement(mouseTestingButton);
            
            return builder.Build();
        }
    }
}