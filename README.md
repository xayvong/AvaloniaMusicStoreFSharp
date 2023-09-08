# AvaloniaMusicStore with F# 

![AvaloniaMusicStore](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/9c4a3ca7-5a17-4d97-839b-d78da8741c6f)

This is an MVVM project that uses Avalonia UI, F#, and ReactiveProperty. This will be a guide and an example of the Music Store App from from their tutorial, and the different ways I approached
and overcame some the difficulties I had during the process. 

I'm also still quite new to F# and Avalonia, so if anyone has any advice on what I could have differenlty or better (Or maybe explain how the heck I use ReactiveUI with F#), any suggestions are welcome!

You can check out the Avalonia here: 
https://avaloniaui.net/

Here's the tutorial I followed: 
https://docs.avaloniaui.net/docs/next/tutorials/music-store-app/

And here is ReactiveProperty, which I used as an alternative to ReactiveUI and Community Toolkit:
https://github.com/runceel/ReactiveProperty#documentation

One thing before you follow along, make sure you first grab the templates for Avalonia. At the time of when I started this project, Avalonia 11 did not come with these templates by default.
https://github.com/AvaloniaUI/avalonia-dotnet-templates

So if the tutorial starts referencing project files you don't have, that might be why. 

Follow along with the original tutorial for creating the UI. One of my favorite features of Avalonia is the live previewer. You can add and change things and the previewer will update them live. Neat!

![AvaloniaPreviewer](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/e50aca59-3cdd-4fa7-bc87-bf3296456561)

Another thing to keep in mind is that hierarchy of your classes and files matters in F#! Whenever you make a new view or class, make sure to order them properly. You can pass things down, but not up!

![AvaloniaFileOrder](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/02839b26-07e8-4ee6-862f-d3668ef70400)

The first hurdle I had to overcome was figuring out how to set the DataContext, and how to open a dialog. 

The tutorial uses ReactiveUI to generate it and I was not able to figure out how to get it to work with F#. I ended up asking for help on the Avalonia [Telegram](https://t.me/Avalonia) and did some digging through their docs about it. 
The solution I came up with was to create a Service folder and then create static classes that managed my dialog functions. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/cba61475-e231-4b04-a50f-ff06b68a393b)
>Doc about referencing the main window https://docs.avaloniaui.net/docs/next/concepts/the-main-window

This way, I can easily open and close a dialog by calling the function and simply pass the view I want to be the dialog. 
I then set the data context, create a reactive command, and subscribe to my dialog service function so I can bind that to a button. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/4e0718a9-f30a-4a50-9223-2fceaad73b4f)
>The ShowDialog() function is an async task, so you have to await it using |> Async.AwaitTask and start it with |> Async.Start. More on this later...

In the tutorial, they have you set the search text and the loading bar using reactiveUI, however with reactiveProperty, you only need to create it and set it to a public member for binding. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/e2e853a0-58f3-4bf5-bd8d-e3d03e368c63)

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/5a07e534-76d8-49d8-a9bb-5d4d0f569403)

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/827ab672-588b-4c8b-a2e1-4bad7d1e4101)

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/7c1fb9e9-a0b7-461b-85b5-167e81343dcb)

Another difference is that when you are binding your reactive properties, you need to add .Value to the binding.
![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/d8680bdc-d45e-4548-9b23-9c80429180b1)
>If your bindings aren't working, check that first!

And instead of Observable collection, I use a ReactiveCollection for my search results, and another reactive property for selectedAlbum.

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/5667448e-faf1-4933-8afd-59446c07e00d)

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/9ba1bb66-a074-4287-aaff-48e493f90da9)

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/ab5cd847-f180-4ad6-ac2e-c51e48640f27)

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/9f393c40-7afe-4e8a-8c21-6364c39f8ab4)
>For some reason, you don't need to add .Value to a ReactiveCollection when binding. I'm not sure what makes this different, but it works, so ¯\\\_(ツ)_/¯

# Album Service

When it comes to creating the album model, and implementing the iTunes search library I really struggled with this section since I'm still a beginner when it comes to Asynchronous programming. 
I eventually started getting comfortable with it and figured out how to use F# async tasks. Here's how I did it!

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/68626b6a-e85f-499f-a68f-dc5824438eaa)

>I added in the ArtistId to make sure we're saving the json file with a unique file name. More on that later!

Notice how I also set static members. This is a really neat F# feature that lets you call static members like functions without needing an instance of the type!

![AvaloniaStaticMember](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/a4122e88-70b9-4790-a66f-cfb4fbc6b8c1)

I also created a static function as a shortcut for creating an empty album. We'll need this later. 

If you look at the SearchAsync() function, you'll notice the = async. This is how you build an async workflow in F# and you put your logic inside of the curly brackets.  

In my async builder, I first set my searchManager as a new iTunesSearchManager() and then set my query using the GetAlbumsAsync() function. Notice how the let has a !. This is how you bind a Task to a name.
Since the GetAlbumAsync() function is a Task, I have to pipe it to an Async.AwaitTask. 

We then create the doSearch function in MusicStoreViewModel and it'll look something like this:

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/2fe12ab1-7c2c-4a0c-9581-59ae36035420)

Under my first if statement, I create another async builder and bind my SearchAsync. Notice how I pipe it to an Async.StartAsTask? Since that function is a computation, I have to start it as a Task. 

In F#, Async and Task are not the same thing. Yes, they are both asynchronous workflows, but are considered different value types. However, with the Async.AwaitTask, and Async.StartAsTask, I can work with
those tasks inside of an async{} instead of Task{}. 

![AvaloniaAsyncAwaitTask](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/2de853e9-2950-49ad-a113-47b38ddc9071)

At the end of my async builder, I pipe it to an Async.Start 
>In F#, you await async logic with Async.Await, and when you call your async function you have to use Async.Start. Don't forget to start your async functions!

Another thing I did differently was instead of having the search begin with typing, I created a button, made it invisible, and set the hotkey to Enter.

![AvaloniaSearchKey](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/f2120100-0a55-4632-b767-9aab4d57e153)

In order to start the search, I need create a command and subscribe to it. I create a startSearch ReactiveCommand().

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/835edb93-a20e-4b90-8e2e-026b7bb853f4)

And subscribe to it where I set all my do commands.

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/5c22aa09-fc84-40fb-9dae-7d0a60de644e)

>Notice how my startSearch is one line lower than the do? You can actually keep adding commands without having to type do ... over and over again for additional commands!

Then we bind the command to a public member and add it to our invisible search button. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/3cd703a7-4d19-49a7-bb05-aaa0420616bf)

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/f8843297-a205-4748-b219-10ee230979d7)

To load the covers in my album model, I set a public member with LoadCoverBitmapAsync() and set my logic. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/12650e63-810a-4d75-836f-085794a4a1a4)

And in my AlbumViewModel, we set the logic to load our covers. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/1919fe91-afe7-458d-a8d3-acea5bcb6dff)

I create the cover as a new ReactiveProperty and make my LoadCover() function async. 

And in my MusicStoreViewModel, I add an async loadCover() function. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/c1f042d1-9dc8-4acc-b04e-7307061a5c83)
>Don't worry about the cancellationToken yet, we'll get to it!

First we set our cancellation token.

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/3a8b2c3a-18ce-41ff-ad6a-9a47500ae287)

And then add the cancellation logic to our doSearch() function. 

![image](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/6fc77833-ccc7-4aca-b5f9-eccbbd7a832c)

> loadCovers() is an async function, so don't forget to start it!

![AvaloniaSearchOasis](https://github.com/xayvong/AvaloniaMusicStoreFSharp/assets/89797311/bb4bdfeb-6a64-45e5-8971-cac969d95c1a)

#Buy Music Command 

Now that we have the album search working, we need to bind the music we buy to a collection. Follow along with the original tutorial for the xaml parts, and I'll show how I handled the F# code.

One issue I ran into is that I'm still not sure how to create a proper Dialog Return. In the original tutorial, 



