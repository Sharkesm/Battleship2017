
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SwinGameSDK;

/// <summary>
/// The menu controller handles the drawing and user interactions
/// from the menus in the game. These include the main menu, game
/// menu and the settings m,enu.
/// </summary>

static class MenuController
{

	/// <summary>
	/// The menu structure for the game.
	/// </summary>
	/// <remarks>
	/// These are the text captions for the menu items.
	/// </remarks>
	private static readonly string[][] _menuStructure = {
    new string[] {
			"PLAY",
			"SETUP",
			"SCORES",
			"MUTE",
			"BGM",
			"QUIT"
		},
		new string[] {
			"RETURN",
			"SURRENDER",
			"MUTE",
			"RESTART",
			"QUIT"

		},
		new string[] {
			"EASY",
			"MEDIUM",
			"HARD"
		},
		new string[]{
			"BGM 1",
			"BGM 2",
			"BGM 3"
		}
	};
	private const int MENU_TOP = 575;
	private const int MENU_LEFT = 30;
	private const int MENU_GAP = 0;
	private const int BUTTON_WIDTH = 75;
	private const int BUTTON_HEIGHT = 15;
	private const int BUTTON_SEP = BUTTON_WIDTH + MENU_GAP;

	private const int TEXT_OFFSET = 0;
	private const int MAIN_MENU = 0;
	private const int GAME_MENU = 1;
	private const int SETUP_MENU = 2;
	private const int BGM_MENU = 3;

	private const int MAIN_MENU_PLAY_BUTTON = 0;
	private const int MAIN_MENU_SETUP_BUTTON = 1;
	private const int MAIN_MENU_TOP_SCORES_BUTTON = 2;
	private const int MAIN_MENU_MUTE_BUTTON = 3;
	private const int MAIN_MENU_BGM_BUTTON = 4;
	private const int MAIN_MENU_QUIT_BUTTON = 5;

	private const int SETUP_MENU_EASY_BUTTON = 0;
	private const int SETUP_MENU_MEDIUM_BUTTON = 1;
	private const int SETUP_MENU_HARD_BUTTON = 2;

	private const int BGM_MENU_1 = 0;
	private const int BGM_MENU_2 = 1;
	private const int BGM_MENU_3 = 2;

	private const int SETUP_MENU_EXIT_BUTTON = 3;
	private const int GAME_MENU_RETURN_BUTTON = 0;
	private const int GAME_MENU_SURRENDER_BUTTON = 1;
	private const int GAME_MENU_MUTE_BUTTON = 2;
	private const int GAME_MENU_RESTART_BUTTON = 3;

	private const int GAME_MENU_QUIT_BUTTON = 4;
	private static bool isMute = false;
	private static int bgmPlaying = 1;
	private static readonly Color MENU_COLOR = SwinGame.RGBAColor(2, 167, 252, 255);

	private static readonly Color HIGHLIGHT_COLOR = SwinGame.RGBAColor(1, 57, 86, 255);
	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleMainMenuInput()
	{
		HandleMenuInput(MAIN_MENU, 0, 0);
	}

	/// <summary>
	/// Handles the processing of user input when the main menu is showing
	/// </summary>
	public static void HandleSetupMenuInput()
	{
		bool handled = false;
		handled = HandleMenuInput(SETUP_MENU, 1, 1);

		if (!handled) {
			HandleMenuInput(MAIN_MENU, 0, 0);
		}
	}

	/// <summary>
	/// Handles the BGM Menu input.
	/// </summary>
	public static void HandleBGMMenuInput ()
	{
		bool handled = false;
		handled = HandleMenuInput (BGM_MENU, 1, 4);

		if (!handled) {
			HandleMenuInput (MAIN_MENU, 0, 0);
		}
	}

	/// <summary>
	/// Handle input in the game menu.
	/// </summary>
	/// <remarks>
	/// Player can return to the game, surrender, or quit entirely
	/// </remarks>
	public static void HandleGameMenuInput()
	{
		HandleMenuInput(GAME_MENU, 0, 0);
	}

	/// <summary>
	/// Handles input for the specified menu.
	/// </summary>
	/// <param name="menu">the identifier of the menu being processed</param>
	/// <param name="level">the vertical level of the menu</param>
	/// <param name="xOffset">the xoffset of the menu</param>
	/// <returns>false if a clicked missed the buttons. This can be used to check prior menus.</returns>
	private static bool HandleMenuInput(int menu, int level, int xOffset)
	{
		if (SwinGame.KeyTyped(KeyCode.vk_ESCAPE)) {
			GameController.EndCurrentState();
			return true;
		}

		if (SwinGame.MouseClicked(MouseButton.LeftButton)) {
			int i = 0;
			for (i = 0; i <= _menuStructure[menu].Length - 1; i++) {
				//IsMouseOver the i'th button of the menu
				if (IsMouseOverMenu(i, level, xOffset)) {
					PerformMenuAction(menu, i);
					return true;
				}
			}

			if (level > 0) {
				//none clicked - so end this sub menu
				GameController.EndCurrentState();
			}
		}

		return false;
	}

	/// <summary>
	/// Draws the main menu to the screen.
	/// </summary>
	public static void DrawMainMenu()
	{
		//Clears the Screen to Black
	    SwinGame.DrawText ("Main Menu", Color.White, GameResources.GameFont ("ArialLarge"), 50, 50);
		SwinGame.DrawText ("Hotkeys", Color.DeepSkyBlue, GameResources.GameFont ("Arial"), 15, 500);
		SwinGame.DrawText ("F1 - Toggle fullscreen", Color.DeepSkyBlue, GameResources.GameFont ("Arial"), 15, 520);
		SwinGame.DrawText ("F2 - Take screenshot", Color.DeepSkyBlue, GameResources.GameFont ("Arial"), 15, 540);

		DrawButtons(MAIN_MENU);
	}

	/// <summary>
	/// Draws the Game menu to the screen
	/// </summary>
	public static void DrawGameMenu()
	{
		//Clears the Screen to Black
		//SwinGame.DrawText("Paused", Color.White, GameFont("ArialLarge"), 50, 50)

		DrawButtons(GAME_MENU);
	}

	/// <summary>
	/// Draws the settings menu to the screen.
	/// </summary>
	/// <remarks>
	/// Also shows the main menu
	/// </remarks>
	public static void DrawSettings()
	{
		//Clears the Screen to Black
		//SwinGame.DrawText("Settings", Color.White, GameFont("ArialLarge"), 50, 50)

		DrawButtons(MAIN_MENU);
		DrawButtons(SETUP_MENU, 1, 1);
	}

	/// <summary>
	/// Draws the BGM Settings.
	/// </summary>
	public static void DrawBGMSettings ()
	{
		//Clears the Screen to Black
		//SwinGame.DrawText("Settings", Color.White, GameFont("ArialLarge"), 50, 50)

		DrawButtons (MAIN_MENU);
		DrawButtons (BGM_MENU, 1, 4);
	}

	/// <summary>
	/// Draw the buttons associated with a top level menu.
	/// </summary>
	/// <param name="menu">the index of the menu to draw</param>
	private static void DrawButtons(int menu)
	{
		DrawButtons(menu, 0, 0);
	}

	/// <summary>
	/// Draws the menu at the indicated level.
	/// </summary>
	/// <param name="menu">the menu to draw</param>
	/// <param name="level">the level (height) of the menu</param>
	/// <param name="xOffset">the offset of the menu</param>
	/// <remarks>
	/// The menu text comes from the _menuStructure field. The level indicates the height
	/// of the menu, to enable sub menus. The xOffset repositions the menu horizontally
	/// to allow the submenus to be positioned correctly.
	/// </remarks>
	private static void DrawButtons(int menu, int level, int xOffset)
	{
		int btnTop = 0;

		btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int i = 0;
		string s = "UNMUTE";

		for (i = 0; i <= _menuStructure[menu].Length - 1; i++) {
			int btnLeft = 0;
			btnLeft = MENU_LEFT + BUTTON_SEP * (i + xOffset);
			//SwinGame.FillRectangle(Color.White, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT)
			SwinGame.DrawTextLines(_menuStructure[menu][i], MENU_COLOR, Color.Black, GameResources.GameFont("Menu"), FontAlignment.AlignCenter, btnLeft + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);

			if (_menuStructure [menu] [i] == "MUTE" && isMute == true)
				SwinGame.DrawTextLines (s, Color.Red, Color.Black, GameResources.GameFont ("Menu"), FontAlignment.AlignCenter, btnLeft + TEXT_OFFSET, btnTop + TEXT_OFFSET, BUTTON_WIDTH, BUTTON_HEIGHT);
			if (SwinGame.MouseDown (MouseButton.LeftButton) & IsMouseOverMenu (i, level, xOffset)) {
				SwinGame.DrawRectangle (HIGHLIGHT_COLOR, btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
			}
		}
	}

	/// <summary>
	/// Determined if the mouse is over one of the button in the main menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <returns>true if the mouse is over that button</returns>
	private static bool IsMouseOverButton(int button)
	{
		return IsMouseOverMenu(button, 0, 0);
	}

	/// <summary>
	/// Checks if the mouse is over one of the buttons in a menu.
	/// </summary>
	/// <param name="button">the index of the button to check</param>
	/// <param name="level">the level of the menu</param>
	/// <param name="xOffset">the xOffset of the menu</param>
	/// <returns>true if the mouse is over the button</returns>
	private static bool IsMouseOverMenu(int button, int level, int xOffset)
	{
		int btnTop = MENU_TOP - (MENU_GAP + BUTTON_HEIGHT) * level;
		int btnLeft = MENU_LEFT + BUTTON_SEP * (button + xOffset);

		return UtilityFunctions.IsMouseInRectangle(btnLeft, btnTop, BUTTON_WIDTH, BUTTON_HEIGHT);
	}

	/// <summary>
	/// A button has been clicked, perform the associated action.
	/// </summary>
	/// <param name="menu">the menu that has been clicked</param>
	/// <param name="button">the index of the button that was clicked</param>

	private static void PerformMenuAction (int menu, int button)
	{
		switch (menu) {
		case MAIN_MENU:
			PerformMainMenuAction (button);
			break;
		case SETUP_MENU:
			PerformSetupMenuAction (button);
			break;
		case GAME_MENU:
			PerformGameMenuAction (button);
			break;
		case BGM_MENU:
			PerformBGMMenuAction (button);
			break;
		}
	}
	/// <summary>
	/// The main menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>



	private static void PerformMainMenuAction (int button)
	{
		switch (button) {
		case MAIN_MENU_PLAY_BUTTON:
			GameController.StartGame ();
			break;
		case MAIN_MENU_SETUP_BUTTON:
			GameController.AddNewState (GameState.AlteringSettings);
			break;
		case MAIN_MENU_TOP_SCORES_BUTTON:
			GameController.AddNewState (GameState.ViewingHighScores);
			break;
		case MAIN_MENU_MUTE_BUTTON:
			Mute ();
			break;
		case MAIN_MENU_BGM_BUTTON:
			GameController.AddNewState (GameState.BGMSettings);
			break;

		case MAIN_MENU_QUIT_BUTTON:
			GameController.EndCurrentState ();
			break;
		}
	}


	/// <summary>
	/// The setup menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	private static void PerformSetupMenuAction(int button)
	{
		switch (button) {
			case SETUP_MENU_EASY_BUTTON:
			GameController.SetDifficulty(AIOption.Hard);
				break;
			case SETUP_MENU_MEDIUM_BUTTON:
			GameController.SetDifficulty(AIOption.Hard);
				break;
			case SETUP_MENU_HARD_BUTTON:
			GameController.SetDifficulty(AIOption.Hard);
				break;
		}
		//Always end state - handles exit button as well
		GameController.EndCurrentState();
	}

	/// <summary>
	/// Stops the BGM music.
	/// </summary>
	public static void StopBGMMusic(){
		Audio.StopMusic ();
	}

	/// <summary>
	/// Performs the BGM menu action.

	/// <summary>
	/// Performs the BGM Menu action of different sounds
	/// </summary>
	/// <param name="button">Button.</param>
	private static void PerformBGMMenuAction (int button)
	{
		switch (button) {
		case BGM_MENU_1:
///			Audio.StopMusic ();
			SwinGame.StopMusic();
			SwinGame.PlayMusic (GameResources.GameMusic ("Background"));
			bgmPlaying = 1;
			break;
		case BGM_MENU_2:
///			Audio.StopMusic ();
			SwinGame.StopMusic();
			SwinGame.FadeMusicIn (GameResources.GameMusic ("closer"), 3000);
			bgmPlaying = 2;
			break;
		case BGM_MENU_3:
///			Audio.StopMusic ();
			SwinGame.StopMusic ();
			SwinGame.FadeMusicIn (GameResources.GameMusic ("dice"), 3000);
			bgmPlaying = 3;
			break;
		}
		//Always end state - handles exit button as well
		GameController.EndCurrentState ();
	}


	/// <summary>
	/// The game menu was clicked, perform the button's action.
	/// </summary>
	/// <param name="button">the button pressed</param>
	 
	private static void PerformGameMenuAction (int button)
	{
		switch (button) {
		case GAME_MENU_RETURN_BUTTON:
			GameController.EndCurrentState ();
			break;
		case GAME_MENU_SURRENDER_BUTTON:
			GameController.EndCurrentState ();
			//end game menu
			GameController.EndCurrentState ();
			//end game
			break;
		case GAME_MENU_MUTE_BUTTON:
			Mute ();
			break;
		case GAME_MENU_RESTART_BUTTON:
			GameController.EndCurrentState ();
			//end game menu
			GameController.EndCurrentState ();
			//end game
			GameController.StartGame ();
			break;

		case GAME_MENU_QUIT_BUTTON:
			GameController.AddNewState (GameState.Quitting);
			break;
		}
	}

	/// <summary>
	/// added new feature for muting the game
	/// </summary>
	private static void Mute ()
	{
		if (isMute == false) {
			Audio.CloseAudio ();
			isMute = true;
		} else {
			Audio.OpenAudio ();
			switch (bgmPlaying) {
			case 1:
				SwinGame.PlayMusic (GameResources.GameMusic ("Background"));
				isMute = false;
				break;
			case 2:
				SwinGame.FadeMusicIn (GameResources.GameMusic ("closer"), 3000);
				isMute = false;
				break;
			case 3:
				SwinGame.FadeMusicIn (GameResources.GameMusic ("dice"), 3000);
				isMute = false;
				break;
			}
		}
	}
}