using System;
using (Microsoft.VisualBasic;)
{
using (System;)
{
using (System.Collections;)
{
using (System.Collections.Generic;)
{
using (System.Data;)
{
using (System.Diagnostics;)
{
public static class GameLogic { static @void Main() {; //Opens a new Graphics Window
		SwinGame.OpenGraphicsWindow("Battle Ships", 800, 600);

		//Load Resources
		GameResources.LoadResources();

		SwinGame.PlayMusic(GameResources.GameMusic("Background"));

		//Game Loop
		do { GameController.HandleUserInput();
			GameController.DrawScreen(); } while (!(SwinGame.WindowCloseRequested() == true | GameController.GameController.CurrentState == GameState.Quitting));

		SwinGame.StopMusic();

		//Free Resources and Close Audio, to end the program.FreeResources(); } }

//======================================================= //Service provided by Telerik(www.telerik.com)
//Conversion powered by NRefactory.//Twitter
telerik;
//Facebook
facebook.com / telerik;
//======================================================= //=======================================================