namespace AvaloniaMusicStore.Models

open System.Linq
open System.Net.Http
open System.IO

open iTunesSearch.Library

type Album(artist, title, coverUrl, artistId: int64) = 
    
    let httpClient = new HttpClient()
    let bmpCachePath = $"./Cache/Bmp/{artistId} - {artist} - {title}"

    member val Artist = artist
    member val Title = title
    member val CoverUrl = coverUrl
    member val ArtistId = artistId

    static member Empty = Album("", "", "", 0)

    static member SearchAsync(searchTerm) = async {

        let searchManager = new iTunesSearchManager()

        let! query = searchManager.GetAlbumsAsync(searchTerm) |> Async.AwaitTask

        return query.Albums.Select(fun album -> 
            Album(
                album.ArtistName, 
                album.CollectionName, 
                album.ArtworkUrl100.Replace("100x100bb", "600x600bb"), 
                album.ArtistId))
        }

    member _.LoadCoverBitmapAsync() : Stream = 
             
        if File.Exists(bmpCachePath + ".bmp") then
            File.OpenRead(bmpCachePath + ".bmp")

        else
            let data = httpClient.GetByteArrayAsync(coverUrl).Result
            new MemoryStream(data)
