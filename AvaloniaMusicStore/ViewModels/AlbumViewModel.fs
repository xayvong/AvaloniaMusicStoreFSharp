namespace AvaloniaMusicStore.ViewModels

open System

open Reactive.Bindings
open Avalonia.Media.Imaging

open AvaloniaMusicStore.Models
open AvaloniaMusicStore.Services
open AvaloniaMusicStore.ViewModels


type AlbumViewModel(album: Album) =
    inherit ViewModelBase()   

    let albumService = AlbumServices(album)

    let mutable cover = new ReactiveProperty<Bitmap>()     

    member _.LoadCover() = async {
        let imageStream = album.LoadCoverBitmapAsync()
        let v = Bitmap.DecodeToWidth(imageStream, 400)
        cover.Value <- v
    }

    member _.SaveToDiskAsync() = async {  
        albumService.SaveAsync() |> Async.Start
        let bitmap = cover.Value
        use fs = albumService.SaveCoverBitmapStream()
        bitmap.Save(fs)
    } 
        
    member _.Artist = album.Artist
    member _.Cover = cover
    member _.Title = album.Title
