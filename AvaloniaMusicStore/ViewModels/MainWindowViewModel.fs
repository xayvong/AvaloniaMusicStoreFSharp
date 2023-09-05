namespace AvaloniaMusicStore.ViewModels

open System

open Reactive.Bindings

open AvaloniaMusicStore.Services
open AvaloniaMusicStore.ViewModels
open AvaloniaMusicStore.Views

type MainWindowViewModel() =
    inherit ViewModelBase()

    // Properties and Commands

    let buyMusicCommand = new ReactiveCommand()

    let store = MusicStoreViewModel()
    let view = MusicStoreWindow()

    // Setting Do's and Subscribe
    do 
        view.DataContext <- store

        view.Closing.Add(fun args -> 
            view.Hide() 
            args.Cancel <- true)

        buyMusicCommand.Subscribe(fun _ -> 
            DialogServices.MyDialog(view) 
            |> Async.Start) |> ignore

    // Public Members
    member _.BuyMusicCommand = buyMusicCommand
    member _.MyAlbums = store.MyAlbumsCollection
