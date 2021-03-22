# Babel 3D Folder Explorer

Welcome to Babel a new way of accessing and viewing your files!

## Why try Babel?

Babel is an experimental folder explorer that generates a fully navigable 3D library where all of the files specified by you will appear.

### Babel rules:

Babel follows three simple rules when creating your personal library:

1. **Each folder will be represented as a hexagon room fitted with shelves**
2. **Rooms that are located further down are subdirectories of rooms above**
3. **No two rooms can overlap i.e. every room has a unique 3D point in space allocated**

You can navigate through all of the rooms and open your files directly from within Babel!

**Beware even simple folder structures can be very big don't get lost in your own folders** :wink:

## Installation

Use this link to install, you will find a Unity build just download and play the BabelDrive.exe:

## Instructions

Open the BabelDrive.exe application then enter the folder path you want to generate. Press Alt + Enter to enter windowed mode if you need.

## Limitations

Currently each folder in your folder structure can only contain a **<span style="color: red;">maximum of 300 folders</span>** with **<span style="color: red;">204 files max in each of them</span>**. This may change in the future. 

# System Overview #

![System overview UML](/diagrams/BabelContextModelBlack.jpg)

The application has four systems:

* **Library Generation** is responsible for the creation of rooms and their placement in 3D as well as connecting them with corridors and stairs.
* The **Occlusion System** is responsible to render only visible rooms and cull all rooms that are not in view.
* **Room Management** is responsible for placing files in the shelves, keeping track of files present and open files when prompted. 
* **Menu Management** is responsible for displaying the initial menu and provide Library Generation with the root folder of the library


# Class References #

* **Controls**
	* `MouseLook.cs`	
	* `PlayerMovement.cs`
* **Library Generation**
	* `LibraryArchitect.cs`	
	* `LibraryBuilder.cs`
	* `Node.cs`
* **Menu**
	* `ExceptionRaiser.cs`	10:09 22/03/2021
	* `LibraryMenu.cs`
	* `RootFolderPath.cs`
	* `TerminalInteractible.cs`
	* `TutorialTerminal.cs`
* **Occlusion Stacks**
	* `OcclusionCullingStackManager.cs`	
	* `OcclusionCullingStacksManagerBuilder.cs`
* **RoomManagement**
	* `BoxDispatcher.cs`	
	* `BoxMenu.cs`
	* `FileBox.cs`
	* `GridCell.cs`
	* `LibraryTerminal.cs`
	* `Room.cs`
* **Misc**
	* `God.cs`
	* `Singleton.cs`

