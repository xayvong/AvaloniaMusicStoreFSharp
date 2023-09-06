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

One thing before you follow along, make sure you grab the templates for Avalonia first. At the time of when I started this project, Avalonia 11 did not come with these templates.
https://github.com/AvaloniaUI/avalonia-dotnet-templates

So if the tutorial starts referencing project files you don't have, that's why. 

Follow along the tutorial for creating the UI. One of my favorite features of Avalonia is the live previewer. You can add and change things and the previewer will update them live. Neat!
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

More details coming soon!










