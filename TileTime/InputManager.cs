using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileTime
{
    class InputManager
{

        public void MenuInput(KeyboardState keyCurrentState, KeyboardState keyPrevState, TileHolder MenuSelector, Game1 game)
        {
            //Menu up
            if (keyCurrentState.IsKeyDown(Keys.Up) && keyPrevState.IsKeyUp(Keys.Up))
            {
                MenuSelector.TilesInRange(MenuSelector.MainTile);
                MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileAbove);
                if (MenuSelector.MainTile.TileAbove != null)
                    game.MenuClick.Play();
            }
            //Menu down
            if (keyCurrentState.IsKeyDown(Keys.Down) && keyPrevState.IsKeyUp(Keys.Down))
            {
                MenuSelector.TilesInRange(MenuSelector.MainTile);
                MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileBelow);
                if (MenuSelector.MainTile.TileBelow != null)
                    game.MenuClick.Play();
            }
            //Menu left
            if (keyCurrentState.IsKeyDown(Keys.Left) && keyPrevState.IsKeyUp(Keys.Left))
            {
                MenuSelector.TilesInRange(MenuSelector.MainTile);
                MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileLeft);
                if (MenuSelector.MainTile.TileLeft != null)
                    game.MenuClick.Play();
            }
            //Menu right
            if (keyCurrentState.IsKeyDown(Keys.Right) && keyPrevState.IsKeyUp(Keys.Right))
            {
                MenuSelector.TilesInRange(MenuSelector.MainTile);
                MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileRight);
                if (MenuSelector.MainTile.TileRight != null)
                    game.MenuClick.Play();
            }
            //Enter
            if (keyCurrentState.IsKeyDown(Keys.Enter) && keyPrevState.IsKeyUp(Keys.Enter))
            {
                //Reusing the pause state enum to make use of the confirm exit screen in the menu
                if (game.PauseCheck == PauseState.Unpaused)
                {
                    //Checks for what option the tile is on, and runs the relevant code when enter is pressed
                    if (MenuSelector.MainTile.CurrentTileNum == 0)
                    {
                        game.PictureNum = 0;
                        game.GridPicSwitch = GridOrPic.Picture;
                        game.GridSizeToSet = 3;
                        game.GameState = GameStateCheck.SelectPicture;
                    }
                    //How to screen
                    else if (MenuSelector.MainTile.CurrentTileNum == 1)
                    {
                        game.GameState = GameStateCheck.HowTo;
                    }
                    //Options screen
                    else if (MenuSelector.MainTile.CurrentTileNum == 2)
                    {
                        game.GameState = GameStateCheck.Options;
                    }
                    //Exit to confirm screen
                    else if (MenuSelector.MainTile.CurrentTileNum == 3)
                    {
                        game.PauseCheck = PauseState.Confirm;
                    }
                }
                //Confirm exit
                else if (game.PauseCheck == PauseState.Confirm)
                {
                    game.Exit();
                }
                game.MenuClick.Play();
            }
            //Escape
            if (keyCurrentState.IsKeyDown(Keys.Escape) && keyPrevState.IsKeyUp(Keys.Escape))
            {
                //Returns to menu
                if (game.PauseCheck == PauseState.Confirm)
                    game.PauseCheck = PauseState.Unpaused;
                game.MenuClick.Play();
            }
        }

        public void OptionsInput(KeyboardState keyCurrentState, KeyboardState keyPrevState, TileHolder MenuSelector, Game1 game)
        {
            //Escape to return to menu
            if (keyCurrentState.IsKeyDown(Keys.Escape) && keyPrevState.IsKeyUp(Keys.Escape))
            {
                game.GameState = GameStateCheck.Menu;
                game.MenuClick.Play();
            }
            if (keyCurrentState.IsKeyDown(Keys.Up) && keyPrevState.IsKeyUp(Keys.Up))
            {
                if (game.optionsCheck == VolumeOptions.Music)
                {
                    game.optionsCheck = VolumeOptions.FX;
                    game.MenuClick.Play();
                }
            }
            if (keyCurrentState.IsKeyDown(Keys.Down) && keyPrevState.IsKeyUp(Keys.Down))
            {
                if (game.optionsCheck == VolumeOptions.FX)
                {
                    game.optionsCheck = VolumeOptions.Music;
                    game.MenuClick.Play();
                }
            }
            if (keyCurrentState.IsKeyDown(Keys.Left) && keyPrevState.IsKeyUp(Keys.Left))
            {
                if (game.optionsCheck == VolumeOptions.FX && game.EffectsVolume > 0)
                {
                    game.EffectsVolume -= 0.25f;
                    game.MenuClick.Play();
                }
                if (game.optionsCheck == VolumeOptions.Music && game.MusicVolume > 0)
                {
                    game.MusicVolume -= 0.25f;
                    game.MenuClick.Play();
                }
            }
            if (keyCurrentState.IsKeyDown(Keys.Right) && keyPrevState.IsKeyUp(Keys.Right))
            {
                if (game.optionsCheck == VolumeOptions.FX && game.EffectsVolume < 1)
                {
                    game.EffectsVolume += 0.25f;
                    game.MenuClick.Play();
                }
                if (game.optionsCheck == VolumeOptions.Music && game.MusicVolume < 1)
                {
                    game.MusicVolume += 0.25f;
                    game.MenuClick.Play();
                }
            }
        }

        public void SelectPicInput(KeyboardState keyCurrentState, KeyboardState keyPrevState, TileHolder MenuSelector, Game1 game)
        {
            //Cycles through the available pictures & grid size for the puzzle
            if (keyCurrentState.IsKeyDown(Keys.Up) && keyPrevState.IsKeyUp(Keys.Up))
            {
                game.GridPicSwitch = GridOrPic.Picture;
                game.MenuClick.Play();
            }
            if (keyCurrentState.IsKeyDown(Keys.Down) && keyPrevState.IsKeyUp(Keys.Down))
            {
                game.GridPicSwitch = GridOrPic.GridSize;
                game.MenuClick.Play();
            }
            //Cycles left
            if (keyCurrentState.IsKeyDown(Keys.Left) && keyPrevState.IsKeyUp(Keys.Left))
            {
                if (game.PictureNum > 0 && game.GridPicSwitch == GridOrPic.Picture)
                {
                    game.PictureNum--;
                    game.MenuClick.Play();
                }
                else if (game.PictureNum == 0 && game.GridPicSwitch == GridOrPic.Picture)
                {
                    game.PictureNum = 3;
                    game.MenuClick.Play();
                }
                else if (game.GridPicSwitch == GridOrPic.GridSize && game.GridSizeToSet > 2)
                {
                    game.GridSizeToSet--;
                    game.MenuClick.Play();
                }
            }
            //Cycles right
            if (keyCurrentState.IsKeyDown(Keys.Right) && keyPrevState.IsKeyUp(Keys.Right))
            {
                if (game.PictureNum < 3 && game.GridPicSwitch == GridOrPic.Picture)
                {
                    game.PictureNum++;
                    game.MenuClick.Play();
                }
                else if (game.PictureNum == 3 && game.GridPicSwitch == GridOrPic.Picture)
                {
                    game.PictureNum = 0;
                    game.MenuClick.Play();
                }
                else if (game.GridPicSwitch == GridOrPic.GridSize && game.GridSizeToSet < 8)
                {
                    game.GridSizeToSet++;
                    game.MenuClick.Play();
                }
            }
            //Confirms selection, creates the tile grid, and starts the game state
            if (keyCurrentState.IsKeyDown(Keys.Enter) && keyPrevState.IsKeyUp(Keys.Enter))
            {
                //Checks if the TileHolder exists, and if not, creates one
                if (game.TileHolder == null)
                    game.TileHolder = new TileHolder();
                game.TileHolder.GridSize = game.GridSizeToSet;
                game.TileHolder.CurrentPicture = game.CurrentPic;
                game.TileHolder.Font = game.DebugFont;
                game.TileHolder.TileSetup(game.WindowResX, game.WindowResY, game.CanvasScale);
                game.GameState = GameStateCheck.Game;
                game.MenuClick.Play();
            }
            //Escape input, select picture state
            if (keyCurrentState.IsKeyDown(Keys.Escape) && keyPrevState.IsKeyUp(Keys.Escape))
            {
                game.GameState = GameStateCheck.Menu;
                game.MenuClick.Play();
            }
            //Changing the pic for the picture select screen, and in turn the picture used in the game
        }

        public void GameInput(KeyboardState keyCurrentState, KeyboardState keyPrevState, TileHolder MenuSelector, Game1 game)
        {
            //Debug mode input
            if (keyCurrentState.IsKeyDown(Keys.F12) && keyPrevState.IsKeyUp(Keys.F12))
                game.DebugMode = !game.DebugMode;
            //Up input
            if (keyCurrentState.IsKeyDown(Keys.Up) && keyPrevState.IsKeyUp(Keys.Up))
            {
                //Pause menu move
                if (game.PauseCheck == PauseState.Paused)
                {
                    MenuSelector.TilesInRange(MenuSelector.MainTile);
                    MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileAbove);
                    if (MenuSelector.MainTile.TileAbove != null)
                        game.MenuClick.Play();
                }
                //Game tile move
                else if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck == shuffleState.Shuffled)
                {
                    game.TileHolder.TilesInRange(game.TileHolder.MainTile);
                    game.TileHolder.SwapTiles(game.TileHolder.MainTile, game.TileHolder.MainTile.TileAbove);
                    if (game.TileHolder.MainTile.TileAbove != null)
                    {
                        game.TileHolder.MoveCounter++;
                        game.MenuClick.Play();
                    }
                }
            }
            //Down input
            if (keyCurrentState.IsKeyDown(Keys.Down) && keyPrevState.IsKeyUp(Keys.Down))
            {
                //Pause menu move
                if (game.PauseCheck == PauseState.Paused)
                {
                    MenuSelector.TilesInRange(MenuSelector.MainTile);
                    MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileBelow);
                    if (MenuSelector.MainTile.TileBelow != null)
                        game.MenuClick.Play();
                }
                //Game tile move
                else if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck == shuffleState.Shuffled)
                {
                    game.TileHolder.TilesInRange(game.TileHolder.MainTile);
                    game.TileHolder.SwapTiles(game.TileHolder.MainTile, game.TileHolder.MainTile.TileBelow);
                    if (game.TileHolder.MainTile.TileBelow != null)
                    {
                        game.TileHolder.MoveCounter++;
                        game.MenuClick.Play();
                    }
                }
            }
            //Left input
            if (keyCurrentState.IsKeyDown(Keys.Left) && keyPrevState.IsKeyUp(Keys.Left))
            {
                //Pause menu move
                if (game.PauseCheck == PauseState.Paused)
                {
                    MenuSelector.TilesInRange(MenuSelector.MainTile);
                    MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileLeft);
                    if (MenuSelector.MainTile.TileLeft != null)
                        game.MenuClick.Play();
                }
                //Game tile move
                else if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck == shuffleState.Shuffled)
                {
                    game.TileHolder.TilesInRange(game.TileHolder.MainTile);
                    game.TileHolder.SwapTiles(game.TileHolder.MainTile, game.TileHolder.MainTile.TileLeft);
                    if (game.TileHolder.MainTile.TileLeft != null)
                    {
                        game.TileHolder.MoveCounter++;
                        game.MenuClick.Play();
                    }
                }
            }
            //Right input
            if (keyCurrentState.IsKeyDown(Keys.Right) && keyPrevState.IsKeyUp(Keys.Right))
            {
                //Pause menu move
                if (game.PauseCheck == PauseState.Paused)
                {
                    MenuSelector.TilesInRange(MenuSelector.MainTile);
                    MenuSelector.SwapTiles(MenuSelector.MainTile, MenuSelector.MainTile.TileRight);
                    if (MenuSelector.MainTile.TileRight != null)
                        game.MenuClick.Play();
                }
                //Game tile move
                else if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck == shuffleState.Shuffled)
                {
                    game.TileHolder.TilesInRange(game.TileHolder.MainTile);
                    game.TileHolder.SwapTiles(game.TileHolder.MainTile, game.TileHolder.MainTile.TileRight);
                    if (game.TileHolder.MainTile.TileRight != null)
                    {
                        game.TileHolder.MoveCounter++;
                        game.MenuClick.Play();
                    }
                }
            }
            //Enter input
            if (keyCurrentState.IsKeyDown(Keys.Enter) && keyPrevState.IsKeyUp(Keys.Enter))
            {
                //Pause menu
                if (game.PauseCheck == PauseState.Paused)
                {
                    //Checks what pause option is selected
                    //Resume
                    if (MenuSelector.MainTile.CurrentTileNum == 0)
                    {
                        game.PauseCheck = PauseState.Unpaused;
                    }
                    //Restart
                    else if (MenuSelector.MainTile.CurrentTileNum == 1)
                    {
                        game.PauseCheck = PauseState.Confirm;
                    }
                    //Main Menu
                    else if (MenuSelector.MainTile.CurrentTileNum == 2)
                    {
                        game.PauseCheck = PauseState.Confirm;
                    }
                    //Exit confirm screen
                    else if (MenuSelector.MainTile.CurrentTileNum == 3)
                    {
                        game.PauseCheck = PauseState.Confirm;
                    }
                    game.MenuClick.Play();
                }
                //Confirm exit
                else if (game.PauseCheck == PauseState.Confirm)
                {
                    //Restart puzzle
                    if (MenuSelector.MainTile.CurrentTileNum == 1)
                    {
                        game.TileHolder.ClearTileList();
                        game.TileHolder.TileSetup(game.WindowResX, game.WindowResY, game.CanvasScale);
                        game.ResetTimer();
                        game.PauseCheck = PauseState.Unpaused;
                    }
                    //Return to menu
                    if (MenuSelector.MainTile.CurrentTileNum == 2)
                    {
                        game.GameState = GameStateCheck.Menu;
                        game.PauseCheck = PauseState.Unpaused;
                        game.TileHolder.ClearTileList();
                        game.ResetTimer();
                        MenuSelector.ResetMenuTile();
                    }
                    //Exit
                    if (MenuSelector.MainTile.CurrentTileNum == 3)
                        game.Exit();
                    game.MenuClick.Play();
                }
                //If the tile state is solved, and the game is not paused, clears the tile list, and re-runs the setup method, then shuffles the board
                else if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck == shuffleState.Solved)
                {
                    game.TileHolder.ClearTileList();
                    game.TileHolder.TileSetup(game.WindowResX, game.WindowResY, game.CanvasScale);
                    game.ResetTimer();
                    game.MenuClick.Play();
                }
                //If the tile state is unshuffled, and the game is not paused, shuffles the tiles
                else if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck == shuffleState.Unshuffled)
                {
                    game.TileHolder.ShuffleTiles();
                    game.ShuffleSound.Play();
                }
            }
            //Escape input
            if (keyCurrentState.IsKeyDown(Keys.Escape) && keyPrevState.IsKeyUp(Keys.Escape))
            {
                if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck != shuffleState.Solved)
                    game.PauseCheck = PauseState.Paused;
                else if (game.PauseCheck == PauseState.Paused)
                    game.PauseCheck = PauseState.Unpaused;
                else if (game.PauseCheck == PauseState.Confirm)
                    game.PauseCheck = PauseState.Paused;
                else if (game.PauseCheck == PauseState.Unpaused && game.TileHolder.ShuffleCheck == shuffleState.Solved)
                {
                    game.GameState = GameStateCheck.Menu;
                    game.TileHolder.ClearTileList();
                    game.ResetTimer();
                }
                game.MenuClick.Play();
                //Method for resetting the menu position on pause/unpause
                MenuSelector.ResetMenuTile();
            }
        }
}
}
