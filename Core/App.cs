using System;
using MvvmCross.ViewModels;
using MvvmCross.IoC;
using Weather.Core.ViewModels;

namespace Core
{
    public class App : MvxApplication
    {
        public App()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            CreatableTypes()
               .EndingWith("Service")
               .AsInterfaces()
               .RegisterAsLazySingleton();

            RegisterAppStart<MainViewModel>();
            
        }
    }
}
