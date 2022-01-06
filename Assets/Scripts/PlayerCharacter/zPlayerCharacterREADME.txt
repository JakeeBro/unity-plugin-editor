This file will explain the usage of each script included in the PlayerCharacter Folder



///// PLAYER /////

This is the base class for all Players.
When creating a player, inherit from Player instead of Monobehaviour.
If using Awake, Update, or FixedUpdate, at the start of each, run base.ParentFunction(); (base.Awake();, base.Update();, etc)



///// DEFAULT PLAYER /////

This is a template for a Player.
You can add your own functionality to this class.



///// INTERACTION PLAYER /////

This is the class used for the InteractionPlayer Prefab.
It is essentially just an empty class inheriting from Player, same as DefaultPlayer.



///// PLAYER MANAGER /////

This class is used to Link various components on the player.
This is the ideal place to implement Input functionality, as it has references for the different components.
It is not included on the DefaultPlayer, as that only has the script for Movement / Camera control.
It is included on the InteractionPlayer, as that has additional components that require each other.