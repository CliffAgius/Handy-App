# HandyApp
Mobile app companion for the #Handy project.

This Xmarin mobile application is being built as an OpenSource application that will allow users of the OpenSource Bionic Hand project to control the hand and it's settings via bluetooth.

This is still very much a work in progress but the current build will allow a Bluetooth Connection to the Adafruit board inside the hand and remote control and set-up.

There are plans to continue the building of this app to a point that any user can configure their own Bionic hand as well as record sensor reading and upload them to an Azure Cloud function for analysis and suggestion of settings, but this is still on the todo list.

FOr now you should be able to download and build this Xamarin.Forms application and deploy to your mobile phone to see where we have got to so far, when we feel it's ready it will get deployed to the App stores so that it's open and usable by anyone around the world.

This application is Built using [Xamarin.Forms](https://dotnet.microsoft.com/apps/xamarin/xamarin-forms) and the [Xamarin Shell](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/shell/navigation) navigation system and uses [Xamarin.Essentials](https://github.com/xamarin/Essentials) and for Bluetooth connection we are using [Bluetooth LE plugin for Xamarin](https://github.com/xabre/xamarin-bluetooth-le) and for the Dialog Toast messages we are using [Acr.UserDialogs](https://github.com/aritchie/userdialogs).

On the Grip Order page we were using SyncFusions amazing tools but that required a Licence which is free for OS projects but did require anyone building the project to get their own, so I have just recently (Jan2020) switched to a new OS project called [Sharpnado](https://github.com/roubachof/Sharpnado.Presentation.Forms) which is completely OpenSource and needs no licence.

If your here looking to help out then please do I do most of the work on the Dev branch and merge the master when I want to kick off a build on AppCentre that is then pushed out to the current Beta Tester.

If you have suggestions/comments about the project and what we are trying to do then again just reach out and let me know.

Thanks.

Clifford Agius
Twitter - @CliffordAgius


