﻿using Microsoft.UI.Xaml;

namespace AK.Toolkit.WinUI3.RichTextBlockExtensionsSampleApp;
public partial class App : Application
{
    private Window? _window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }
}
