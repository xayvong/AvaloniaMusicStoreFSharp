namespace AvaloniaMusicStore.Services

open Avalonia
open Avalonia.Controls
open Avalonia.Controls.ApplicationLifetimes

module DialogServices = 


    let MyDialog (window: Window) = 
        let mainWindow = Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime
        window.ShowDialog(mainWindow.MainWindow) |> Async.AwaitTask


    let CloseDialog() = 
        let mainWindow = Application.Current.ApplicationLifetime :?> IClassicDesktopStyleApplicationLifetime
        mainWindow.MainWindow.Close()

