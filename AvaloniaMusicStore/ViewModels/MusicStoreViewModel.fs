namespace AvaloniaMusicStore.ViewModels

open System
open System.Linq
open System.Threading

open Reactive.Bindings

open AvaloniaMusicStore.Models
open AvaloniaMusicStore.Services
open AvaloniaMusicStore.ViewModels
open System.Reactive.Disposables


type MusicStoreViewModel() =
    inherit ViewModelBase()   

    // Properties and Commands
    let selectedAlbum = new ReactiveProperty<AlbumViewModel>(AlbumViewModel(Album.Empty))
    let searchText = new ReactiveProperty<string>()

    let myAlbumsCollection = new ReactiveCollection<AlbumViewModel>()
    let searchResults = new ReactiveCollection<AlbumViewModel>()

    let mutable isBusy = new ReactiveProperty<bool>()
    let mutable isSelected = new ReactiveProperty<bool>(false)
    let mutable cancellationToken = new CancellationTokenSource()
    let mutable selectedAlbumSubscription = Disposable.Empty

    let buyMusicCommand = new ReactiveCommand()
    let startSearch = new ReactiveCommand()


    // Private Functions
    let loadCovers(cancellationToken:CancellationToken) = async {       
        for album in searchResults.ToList() do
            do! album.LoadCover()           

            if(cancellationToken.IsCancellationRequested) then
                ()
        } 

    let doSearch(s:string) =
        isBusy.Value <- true
        selectedAlbumSubscription.Dispose()
        searchResults.Clear()
        selectedAlbum.Value <- AlbumViewModel(Album.Empty)
        
        cancellationToken.Cancel()
        cancellationToken <- new CancellationTokenSource()
        let cancellationToken = cancellationToken.Token
        
        if String.IsNullOrEmpty(s) |> not then
            async {
            let albums = Album.SearchAsync(s) |> Async.StartAsTask

            for album in albums.Result do 
                let vm = new AlbumViewModel(album)
                searchResults.Add(vm)

            if cancellationToken.IsCancellationRequested |> not then
                loadCovers(cancellationToken) |> Async.Start
                
            isBusy.Value <- false          
            
            selectedAlbumSubscription <- 
                selectedAlbum.Subscribe(fun album -> 
                    isSelected.Value <- String.IsNullOrEmpty(album.Artist) |> not ) 

            } |> Async.Start


    let buyAlbum() = 
        myAlbumsCollection.Add(selectedAlbum.Value)
        selectedAlbum.Value.SaveToDiskAsync() |> Async.Start


    let loadAlbums() = async {     
        let albums = AlbumServices.LoadCachedAsync()

        for album in albums do
            myAlbumsCollection.Add(AlbumViewModel(album))

        for album in myAlbumsCollection.ToList() do
            album.LoadCover() |> Async.Start
        } 
    

            
    // Do and commands
    do 
        buyMusicCommand.Subscribe(fun _ -> 
            DialogServices.CloseDialog() 
            buyAlbum()) |> ignore

        startSearch.Subscribe(fun _ -> 
            doSearch(searchText.Value)) |> ignore

        loadAlbums() |> Async.Start
      

    // Public Members
    member _.BuyMusicCommand = buyMusicCommand
    member _.IsBusy = isBusy
    member _.IsSelected = isSelected 
    member _.MyAlbumsCollection = myAlbumsCollection
    member _.SearchText = searchText
    member _.SearchResults = searchResults
    member _.SelectedAlbum = selectedAlbum
    member _.StartSearch = startSearch

