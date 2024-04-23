Game Concept:

You are trapped in a nightmare filled with monsters. The only way to escape is to turn on the lights to escape the dream. 
Until all the lights are turned on the monsters will keep coming.

Each level will get progressively harder and more dream-like.

--
Note: This game contains only a demo level. User expereince optimizations and game play balances are largely unimplemented at this time.

All C# code written for this game can be found under Wave Survival Game/Assets/Scripts in the Unity3D Assets repo.

--


Game Play Mechanics:

	* The player is a white cube. 
	* The player has Health represented by sanity.
        * The player has experience and levels. Experience is represented by Will Power.
        * Monsters are red cubes. If a monster touches the player they will lose sanity. If the player's sanity reaches 0 - they die. 
        * The player can shoot monsters to kill them. 
        * Monsters spawn in waves with rest (cooldown) periods in between. In the demo level both only last for a few seconds.
        * A wave will not end if monsters are remaining.
        * Lights are dark blue cubes.
        * The player must pick them up and move them near outlets (black rectangles on the wall).
        * A player holding a light cannot shoot. The light must be dropped first. 
        * A light dropped near an outlet can be plugged in.
	* A light plugged into an on outlet will turn on if the outlet has power. 
        * Switches are represented by white rectangles on the wall.
	* Some outlets are connected to switches.
	* A player may toggle a switch to on/off power for a connected outlet.
        * If all lights in a room are turned on, spawns in that room will stop generating monsters. 
       
Future Plans:
	* Level Design
        * Additional Puzzle Mechanics
	* Rewards system (power ups/hints/weapons based on will power)
	* Weapon Diversity
	* Monster Diversity
	* Graphics overhaul
	* User Experience Improvements
	* and More

Controls:

	-Move = WASD
	-Look = Mouse
	-Shoot = Left Click
	-Pick Up Light/Plug In/Drop Light = Right Click
	-Exit Game = ESC

Download and Play:

	1) Download the zip file at this link: https://drive.google.com/file/d/1sXJgit5wB-zNbHMVOViBQwJReJkbfc_h/view?usp=sharing
	2) Once your download is complete, unzip the WaveSurvivalGame folder. All contents must be unzipped to the target destination.
	3) Double click the "Wave Survival Game" application to launch.


Solution Set for Level 1:
	1) Move the player (WASD) to the light cube in the bottom left hand corner.
	2) Pick up the light (Right Click) when text prompt appears.
	3) Move the light cube (WASD) to the plug next to the closet door in the upper right.
	4) Drop the light (Right Click).
	5) Plug in the light (Right Click).
	6) Move the Player (WASD) to the light switch in the upper left hand corner
	7) Turn on the switch (Right Click). 
