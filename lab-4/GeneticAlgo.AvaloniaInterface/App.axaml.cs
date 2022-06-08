using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GeneticAlgo.AvaloniaInterface.ViewModels;
using GeneticAlgo.Shared;
using GeneticAlgo.Shared.Models;
using GeneticAlgo.Shared.Tools;
using Microsoft.Extensions.DependencyInjection;

namespace GeneticAlgo.AvaloniaInterface
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                const int maximum = 10;

                var collection = new ServiceCollection();
                collection.AddSingleton<MainWindowViewModel>();
                collection.AddSingleton<IExecutionContext>(_ => new DummyExecutionContext(100, maximum, 3));
                collection.AddSingleton(new ExecutionConfiguration(TimeSpan.FromMilliseconds(1000), maximum, 0));

                var provider = collection.BuildServiceProvider();

                desktop.MainWindow = new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}