using System;
using SwinGameSDK;

/// <summary>
/// The battle phase is handled by the DiscoveryController.
/// </summary>
internal static class DiscoveryController
{

	/// <summary>
	/// Handles input during the discovery phase of the game.
	/// </summary>
	/// <remarks>
	/// Escape opens the game menu. Clicking the mouse will
	/// attack a location.
	/// </remarks>
	public static void HandleDiscoveryInput()
	{
		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE))
		{
			GameController.AddNewState(GameState.ViewingGameMenu);
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton))
		{
			DoAttack();
		}



		if (SwinGame.KeyTyped (KeyCode.vk_1)) {
			PlayBackgroundMusic (1);
		} else if (SwinGame.KeyTyped (KeyCode.vk_2)) {
			PlayBackgroundMusic (2);
		} else if (SwinGame.KeyTyped (KeyCode.vk_3)) {

			PlayBackgroundMusic (3);
		} else if (SwinGame.KeyTyped (KeyCode.vk_m)) {
			PlayBackgroundMusic (0);
		}
	}





	/// <summary>
	/// Plaies the background music.
	/// </summary>
	/// <param name="playMusic">Play music.</param>
	private static void PlayBackgroundMusic (int playMusic)
	{


		if (playMusic == 1) {
			SwinGame.StopMusic ();
			GameController.SetBackgroundMusic ("Background");
			SwinGame.PlayMusic (GameResources.GameMusic ("Background"));
		} else if (playMusic == 2) {
			SwinGame.StopMusic ();

			SwinGame.PlayMusic (GameResources.GameMusic ("closer"));
		} else if (playMusic == 3) {
			SwinGame.StopMusic ();
			GameController.SetBackgroundMusic ("dice");
			SwinGame.PlayMusic (GameResources.GameMusic ("dice"));
		} else if (playMusic == 0) {
			if (!GameController.isBackgroudMuted ()) {
				SwinGame.StopMusic ();
				GameController.SetBackgroundToMute (true);
			} else {
				SwinGame.PlayMusic (GameResources.GameMusic (GameController.getMusic));
				GameController.SetBackgroundToMute (false);
			}
		}

	}



	/// <summary>
	/// Attack the location that the mouse if over.
	/// </summary>
	private static void DoAttack()
	{
		Point2D mouse = SwinGame.MousePosition();


		//Calculate the row/col clicked
		int row = 0;
		int col = 0;
		row = Convert.ToInt32(Math.Floor((mouse.Y - UtilityFunctions.FIELD_TOP) / (UtilityFunctions.CELL_HEIGHT + UtilityFunctions.CELL_GAP)));
		col = Convert.ToInt32(Math.Floor((mouse.X - UtilityFunctions.FIELD_LEFT) / (UtilityFunctions.CELL_WIDTH + UtilityFunctions.CELL_GAP)));

		if (row >= 0 && row < GameController.HumanPlayer.EnemyGrid.Height)
		{
			if (col >= 0 && col < GameController.HumanPlayer.EnemyGrid.Width)
			{
				GameController.Attack(row, col);
			}
		}
	}

	/// <summary>
	/// Draws the game during the attack phase.
	/// </summary>s
	public static void DrawDiscovery()
	{
		const int SCORES_LEFT = 172;
		const int SHOTS_TOP = 157;
		const int HITS_TOP = 206;
		const int SPLASH_TOP = 256;
		const int TIMER_TOP = 306;
		const int AI_SHIPS = 316;
		const int AI_SHIPS_LEFT = 72;



		if (((SwinGame.KeyDown(KeyCode.vk_LSHIFT) | SwinGame.KeyDown(KeyCode.vk_RSHIFT)) & SwinGame.KeyDown(KeyCode.vk_c)))
		{
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, true);
		}
		else
		{
			UtilityFunctions.DrawField(GameController.HumanPlayer.EnemyGrid, GameController.ComputerPlayer, false);
		}

		UtilityFunctions.DrawSmallField(GameController.HumanPlayer.PlayerGrid, GameController.HumanPlayer);
		UtilityFunctions.DrawMessage();

		SwinGame.DrawBitmap (GameResources.GameImage ("mute"), 668, 70);
	
		SwinGame.DrawText(GameController.HumanPlayer.Shots.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SHOTS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Hits.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, HITS_TOP);
		SwinGame.DrawText(GameController.HumanPlayer.Missed.ToString(), Color.White, GameResources.GameFont("Menu"), SCORES_LEFT, SPLASH_TOP);
		SwinGame.DrawText (GameController.TimeElapsed(), Color.White, GameResources.GameFont ("Menu"), AI_SHIPS_LEFT, TIMER_TOP); 
		SwinGame.DrawText (GameController.ComputerPlayer.shipsLeft(), Color.White, GameResources.GameFont ("Menu"), AI_SHIPS_LEFT, AI_SHIPS);
	}

}
