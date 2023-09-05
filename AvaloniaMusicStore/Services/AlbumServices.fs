namespace AvaloniaMusicStore.Services

open System
open System.IO
open System.Linq
open System.Collections.Generic
open AvaloniaMusicStore.Models
open System.Text.Json

type AlbumServices(album:Album) =

    let cachePath = $"./Cache/{album.ArtistId} - {album.Artist} - {album.Title}"
    let bmpCachePath = $"./Cache/Bmp/{album.ArtistId} - {album.Artist} - {album.Title}"


    let saveToStreamAsync(data:Album, stream:Stream) = JsonSerializer.SerializeAsync(stream, data) |> Async.AwaitTask
        

    member _.SaveAsync() = async {

        if Directory.Exists("./Cache") |> not then
            Directory.CreateDirectory("./Cache") |> ignore

        use fs = File.OpenWrite(cachePath + ".json")
            
        saveToStreamAsync(album, fs) |> Async.Start
        }


    member _.SaveCoverBitmapStream() = 

        if Directory.Exists("./Cache/Bmp") |> not then
            Directory.CreateDirectory("./Cache/Bmp") |> ignore

        File.OpenWrite(bmpCachePath + ".bmp")


    static member LoadFromStream(stream:Stream) = JsonSerializer.DeserializeAsync<Album>(stream) 


    static member LoadCachedAsync() =       

        if Directory.Exists("./Cache") |> not then
            Directory.CreateDirectory("./Cache") |> ignore

        // This is the original way they had you set it up in the tutorial, 
        // using an eager load that returns a list. 

        //let results = ResizeArray()
        //for file in Directory.EnumerateFiles("./Cache") do
        //    if String.IsNullOrWhiteSpace(DirectoryInfo(file).Extension) |> not then
        //        use fs = File.OpenRead(file)
        //        results.Add(AlbumServices.LoadFromStream(fs).Result)
        //results

        // This is an example of a lazy load, using an F# friendlier format.
        Directory.EnumerateFiles("./Cache")
        |> Seq.filter (fun file -> String.IsNullOrWhiteSpace(DirectoryInfo(file).Extension) |> not )
        |> Seq.map (fun file -> 
            use fs = File.OpenRead(file)
            AlbumServices.LoadFromStream(fs).Result)

