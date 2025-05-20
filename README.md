**3DCityTest** – XR Prototype for Meta Quest 3

Overview

3DCityTest is a XR experience developed for Meta Quest 3 using Unity 6. 
It allows users to place and manipulate 3D UI elements within a virtual citys. 
The project showcases UI to 3D world interactions in a mixed reality environment.
Built using OpenXR and XR Interaction Toolkit, MR Unity Sample for the basics setup and asset package from SimplePoly City - Low Poly.

I have built on top of MR Unity sample adjusting some prefabs to suit the purpose of this project.
Created 2D icons with a editor script based on the 3D models to spawn.
Used interactions layers to filter the validation raycast to spawn the objects.
Created prefabs and variants for the UI Menu and for the prefabs to spawn. 

Features

*  **Drag UI Item Spawner**: Grab UI icons and spawn 3D objects directly into the environment.
*  **ObjectPlacementHelper**: Helps validate object placement.
*  **ScaleAndPopEffect**: Visual feedback when placing or interacting with elements.
*  **ParticleBurstEffect**: Triggered on successful object spawn.
*  **PlayQuickSound**: Immediate sound feedback.
*  **MenuController**: Toggleable UI menu accessible via controller input.

 Prerequisites

* Unity `6000.0.37f1`
* Meta Quest 3 


Interactions & Controls

Menu Interaction Guide

* Press the **Secondary Button (B/Y)** to open or close the floating menu.
* Point at the menu using your dominant controller's **far interactor** ray.
* Press trigger to select panels
* Press **grip** while using **far interactor** to Grab a UI icon to drag it into the 3D scene.
* Release **grip** to spawn the object at the desired location.
* Use **Joystick** to move and rotate grabbed objects.

* Valid locations are determined using `ObjectPlacementHelper`.
* A particle effect and sound confirm the spawn.
       
 Issues

* Releasing UI Icon with Menu closed doesnt allow to reset the icon to the UI.
* Moving grabed UI to close to the controller breaks that grabbing.
* Spawned Objects can overlap.
* Not able to grab city after moving it to far away.
* Incomplete feedback in placement áreas.
* Hand tracking not fully supported.

