# Mimer-in-Unity
This is an example implementation of using a Mimer Information Technology database in the Unity game engine.

## How to use it
The following instructions indicate how to get the example in this repository running. The steps to recreating it in your own environment are similar. The author has only tried this in a pure Windows environment, but other platforms may or may not be supported, see detailed information in the instructions.

### Cloning this repository
UNDER CONSTRUCTION

### Installing a Mimer database server
1. Go to the Downloads page on the Mimer Developer website, https://developer.mimer.com/products/downloads/.
2. Download one of the Mimer SQL Database packages. Choose the one corresponding to the platform of the machine you want to set up the database server on, which is not necessarily the same machine as you run Unity on. This example was tested on a Windows platform with the Windows 64-bit database server version 11.0.6B with Unity on the same machine.
3. Run the installer of the Mimer SQL Database package of choice.
4. Read the Getting Started guide for your chosen platform, they are available for [Windows](https://docs.mimer.com/MimerOnWindows/latest_mimerwin.html "Mimer SQL - Getting Started on Windows") and [Linux](https://docs.mimer.com/MimerOnLinux/latest_mimerlinux.html "Mimer SQL - Getting Started on Linux").
5. Following the instructions in the Getting Started guide, create a database named "UnityDemo". The "Home Directory" should be set to the "Database" folder in this repository. Start the database server.
6. Following the instructoins in the Getting Started guide, use a tool of choice (e.g. Mimer BSQL) to access the database server.
7. How to use the exisitng databank? TO BE CONTINUED

### Installing Unity
UNDER CONSTRUCTION

### Installing a Mimer ADO.NET provider
The Mimer ADO.NET provider contains libraries for accessing your Mimer SQL database through .NET. It supports all versions of .NET Framework, .NET Core 3.1 and .NET 5, and can be used on Windows for all .NET versions, and on Linux for .NET Core 3.1 and .NET 5. Unity does however not support neither .NET Core nor .NET 5 at the time of writing this, and as such, the ADO.NET Provider used has to be for .NET Framework, not supporting Linux. The approach below is a Windows approach.

On Windows, you have to access the Mimer.Data.Client.dll library and place it in your Unity project folder. There are two ways to access it, both are described in detail in the [documentation](https://docs.mimer.com/MimerNetDataProvider/latest_mimerdataprovider.html/#Installaion.html "Installing the Mimer SQL Data Provider"). 

#### Running the Windows installer
1. Go to the Downloads page on the Mimer Developer website, https://developer.mimer.com/products/downloads/.
2. Download the Mimer Data Provider installer, this example was tested with version 11.0.2A.
3. Run the Mimer Data Provider installer.
4. Go to the installation folder of the Mimer Data Provider, by default it is set to C:\Program Files (x86)\Mimer Data Provider. Enter the version folder (in this example the folder is called v1102a as the version 11.0.2A was installed). In the bin folder, there are folders for each supported .NET version. This example was tested with the latest .NET Framework library, which is the one for .NET Framework 4.5.2 and later, that can be found in the net452 folder. Copy the Mimer.Data.Client.dll file.
5. Place the Mimer.Data.Client.dll file in the Assets\Plugins folder in your Unity project.

#### Accessing the nuget package
1. Go to the MimerSQL.Data.Provider nuget package page on nuget.org, https://www.nuget.org/packages/MimerSQL.Data.Provider/.
2. UNDER CONSTRUCTION

### Running the example
UNDER CONSTRUCTION

### Reading more
* [The Mimer SQL platform articles](https://developer.mimer.com/products/platform-articles/)
* [The Mimer.Data.Client documentation](https://docs.mimer.com/MimerNetDataProvider/latest_mimerdataprovider.html/#Mimer.Data.Client~Mimer.Data.Client_namespace.html)
