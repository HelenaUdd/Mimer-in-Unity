# Mimer-in-Unity
This is an example implementation of using a Mimer Information Technology database in the Unity game engine.

## How to use it
The following instructions indicate how to get the example in this repository running. The steps to recreating it in your own environment are similar. The author has only tried this in a pure Windows environment, but other platforms may or may not be supported, see detailed information in the instructions.

### Cloning this repository
Install git. This document will not go into detail on how to install and use git, as there are many other resources for that. The "Get started" guide at [GitHub Docs](https://docs.github.com/en/get-started) is a recommended starting point. Clone this repository onto your local hard drive using your preferred git client.

### Installing Unity
Install Unity from Unity's website, https://unity.com/. This document will not go into detail on how it's done, or how to work with Unity, as there are many other resources for that. The [Unity Learn](https://unity.com/learn) site is recommended for learning how to use Unity, and the [Unity Forums](https://forum.unity.com/) is recommended for asking questions. This example was created using Unity version 2021.3.4f1.

A Unity project is not a project file, but rather a folder structure, containing a folder called Assets, among others. The Assets folder in this example has a subfolder called Scripts, where the example code that calls the Mimer SQL database is found. Any APIs or plugins that are used by the scripts in Assets\Scripts are to be placed in an Assets\Plugins folder, which is not under version control in this repository, so you have to create it manually.

### Installing a Mimer database server
1. Go to the Downloads page on the Mimer Developer website, https://developer.mimer.com/products/downloads/.
2. Download one of the Mimer SQL Database packages. Choose the one corresponding to the platform of the machine you want to set up the database server on, which is not necessarily the same machine as you run Unity on. This example was tested on a Windows platform with the Windows 64-bit database server version 11.0.6B with Unity on the same machine.
3. Run the installer of the Mimer SQL Database package of choice.
4. Read the Getting Started guide for your chosen platform, they are available for [Windows](https://docs.mimer.com/MimerOnWindows/latest_mimerwin.html "Mimer SQL - Getting Started on Windows") and [Linux](https://docs.mimer.com/MimerOnLinux/latest_mimerlinux.html "Mimer SQL - Getting Started on Linux").
5. Following the instructions in the Getting Started guide, create a database named "UnityDemo". Start the database server.
6. Following the instructions in the Getting Started guide, use a tool of choice (e.g. Mimer BSQL) to access the database server with your chosen system administrator credentials.
7. Run the SQL script in the repository's Database folder, if Mimer BSQL is used you use the following command: `READ INPUT FROM '<path>\tictactoe.sql';`. This will create the data in the database that is expected from the example.

### Installing a Mimer ADO.NET provider
The Mimer ADO.NET provider contains libraries for accessing your Mimer SQL database through .NET. It supports all versions of .NET Framework, .NET Core 3.1 and .NET 5, and can be used on Windows for all .NET versions, and on Linux for .NET Core 3.1 and .NET 5. Unity does however not support neither .NET Core nor .NET 5 at the time of writing this, and as such, the ADO.NET Provider used has to be for .NET Framework, not supporting Linux. The approach below is a Windows approach.

On Windows, you have to access the Mimer.Data.Client.dll library and place it in your Unity project folder. There are two ways to access it, both are described in more detail in the [documentation](https://docs.mimer.com/MimerNetDataProvider/latest_mimerdataprovider.html/#Installaion.html "Installing the Mimer SQL Data Provider"). Choose one of the two following approaches:

#### Running the Windows installer
1. Go to the Downloads page on the Mimer Developer website, https://developer.mimer.com/products/downloads/.
2. Download the Mimer Data Provider installer, this example was tested with version 11.0.2A.
3. Run the Mimer Data Provider installer.
4. Go to the installation folder of the Mimer Data Provider, by default it is set to C:\Program Files (x86)\Mimer Data Provider. Enter the version folder (in this example the folder is called v1102a as the version 11.0.2A was installed). In the bin folder, there are folders for each supported .NET version. This example was tested with the latest .NET Framework library, which is the one for .NET Framework 4.5.2 and later, that can be found in the net452 folder. Copy the Mimer.Data.Client.dll file.
5. Place the Mimer.Data.Client.dll file in the Assets\Plugins folder in your Unity project.

#### Unpacking the nuget package
1. Go to the MimerSQL.Data.Provider nuget package page on nuget.org, https://www.nuget.org/packages/MimerSQL.Data.Provider/.
2. Choose to "download package" from the nuget website. A .nupkg file will be downloaded.
3. Rename the file and change the file extension from ".nupkg" to ".zip", and unzip the .nupkg file. If you use a more advanced zipping tool like 7-zip, it can unzip the .nupkg file directly.
4. Browse the unzipped folder structure, and go into the "lib" folder, which contains folders for each supported .NET version. This example was tested with the latest .NET Framework library, which is the one for .NET Framework 4.5.2 and later, that can be found in the net452 folder. Copy the Mimer.Data.Client.dll file.
5. Place the Mimer.Data.Client.dll file in the Assets\Plugins folder in your Unity project.

### Running the example
1. Open the Unity Hub and select to Open or Add project from disk. Enter the TicTacToe folder in the repository, and select Add Project. The TicTacToe project should now have been added to your list of projects in Unity Hub.
2. Click the TicTacToe project in the list in order to open it. Opening it might take a while, as Unity needs to load all assets and compile all scripts.
3. Press the Play button to start the game. Highscores will be fetched from the Mimer SQL database upon starting.
4. Press the "Start new game" button to start a game. The game will be timed. Click/tap on the boxes to place a marker there for the current player.
5. The game will detect if the game is won by either player, or if the game is over and no one won. When either player wins, a new entry is added to the database and the highscore list will be updated.

#### Database communication
There are two very basic calls to the database. One is a `SELECT`-statement, that fetches highscores, and one is an `INSERT`-statement, that adds new entries to the table. The `SELECT` statement is called by `ScorePopulator.UpdateHighscoreTable`, and the `INSERT` statement is called by `GameLogic.AddHighscore`.
The class `DatabaseCommunicator` has been made a singleton, as in this case, only one connection to one specific server is had. This is absolutely not mandatory, multiple connections are supported by Mimer.Data.Client.dll.

### Reading more
* [The Mimer SQL platform articles](https://developer.mimer.com/products/platform-articles/)
* [The Mimer.Data.Client documentation](https://docs.mimer.com/MimerNetDataProvider/latest_mimerdataprovider.html/#Mimer.Data.Client~Mimer.Data.Client_namespace.html)
