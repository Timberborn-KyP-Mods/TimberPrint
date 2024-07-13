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
    
    public class BottomBarModuleProvider : IProvider<BottomBarModule>
    {
        private readonly MouseTestingButton _mouseTestingButton;

        public BottomBarModuleProvider(MouseTestingButton mouseTestingButton)
        {
            _mouseTestingButton = mouseTestingButton;
        }

        public BottomBarModule Get()
        {
            var builder = new BottomBarModule.Builder();
            
            builder.AddRightSectionElement(_mouseTestingButton);
            
            return builder.Build();
        }
    }
}