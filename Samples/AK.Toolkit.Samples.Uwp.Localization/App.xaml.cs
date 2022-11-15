using AK.Toolkit.Uwp.Localization;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AK.Toolkit.Samples.Uwp.Localization
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            // Initialize localizer by registering as a service.
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .UseLocalizer()
                    .BuildServiceProvider());
            // Initialize Localizer using the LocalizerBuilder.
            //StorageFolder stringsFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Strings");
            //InitializeLocalizer(stringsFolder.Path);

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    _ = rootFrame.Navigate(typeof(ShellPage), e.Arguments);
                }

                Window.Current.Activate();
            }
        }

        private static void InitializeLocalizer(string stringsResourceFolderPath)
        {
            ILocalizer localizer = new LocalizerBuilder()
                // For a packaged app:
                .AddResourcesStringsFolder(new LocalizerResourcesStringsFolder(stringsResourceFolderPath))
                .AddLanguageDictionaries(
                    new List<LanguageDictionary>()
                    {
                    new LanguageDictionary("ja")
                    {
                        new StringResource("ToggleSwitchHeader", "HeaderProperty", "トグルスイッチ"),
                    }
                    })
                .Build();
            Localizer.Set(localizer);
        }

        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}