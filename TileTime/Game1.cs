using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace TileTime
{
    public enum PauseState { Unpaused, Paused, Confirm }
    public enum GridOrPic { GridSize, Picture }
    public enum GameStateCheck { Menu, SelectPicture, Game, Options, HowTo }
    public enum VolumeOptions {FX, Music}



    public class Game1 : Game
    {
        private readonly int baseWindowResX = 1920;
        public int WindowResX { get; set; } = 1920;
        public int WindowResY { get; set; } = 1080;
        public int MillisecondsElapsed { get; set; }
        public int RemainderSeconds { get; set; }
        public int MinutesElapsed { get; set; }
        public int PictureNum { get; set; } = 0;
        public int GridSizeToSet { get; set; } = 3;
        public float CanvasScale { get; set; }
        public float MusicVolume { get; set; } = 0.5f;
        public float EffectsVolume { get; set; } = 0.5f;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont gameFont;
        public SpriteFont DebugFont { get; set; }
        public Texture2D CurrentPic { get; set; }
        private Texture2D theScream, starryNight, kanagawa, persistence, arrowPrompt;
        private Texture2D exitMenuConfirm, exitGameConfirm, returnToMenuConfirm, restartConfirm, enterPrompt, moveCounter, timeCounter;
        private Texture2D tileMenu, tileMenuPause, tileMenuOverlay, pauseOverlay, optionsScreen, howToScreen, winScreen, mainMenuOverlay, tileHolderBack;
        private Rectangle overlayRect;
        private InputManager inputManager;
        public TileHolder TileHolder { get; set; }
        public TileHolder MenuSelector { get; set; }
        public TileHolder PauseMenuSelector { get; set; }
        public SoundEffect VictoryDing { get; set; }
        public SoundEffect ShuffleSound { get; set; }
        public SoundEffect MenuClick { get; set; }
        private Song bgMusic;
        private KeyboardState keyCurrentState;
        private KeyboardState keyPrevState;
        public bool DebugMode { get; set; } = false;
        public PauseState PauseCheck { get; set; }
        public GridOrPic GridPicSwitch { get; set; }
        public GameStateCheck GameState { get; set; }
        public VolumeOptions optionsCheck { get; set; }
        public static Game1 thisInstance;
        public Game1()
        {
            thisInstance = this;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = WindowResX;
            _graphics.PreferredBackBufferHeight = WindowResY;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            MediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            mainMenuOverlay = Content.Load<Texture2D>("Overlays/tiletime");
            pauseOverlay = Content.Load<Texture2D>("Overlays/pauseoverlay");
            tileHolderBack = Content.Load<Texture2D>("Overlays/tileholderback");
            arrowPrompt = Content.Load<Texture2D>("Prompts/arrow");
            exitMenuConfirm = Content.Load<Texture2D>("Prompts/confirmexit");
            exitGameConfirm = Content.Load<Texture2D>("Prompts/confirmexitingame");
            returnToMenuConfirm = Content.Load<Texture2D>("Prompts/confirmmenu");
            restartConfirm = Content.Load<Texture2D>("Prompts/confirmrestart");
            enterPrompt = Content.Load<Texture2D>("Prompts/enterprompt");
            howToScreen = Content.Load<Texture2D>("Overlays/howto");
            winScreen = Content.Load<Texture2D>("Overlays/winscreen");
            optionsScreen = Content.Load<Texture2D>("Overlays/options");
            moveCounter = Content.Load<Texture2D>("Prompts/moves");
            timeCounter = Content.Load<Texture2D>("Prompts/time");
            theScream = Content.Load<Texture2D>("Tile Pictures/thescream");
            starryNight = Content.Load<Texture2D>("Tile Pictures/starrynight");
            kanagawa = Content.Load<Texture2D>("Tile Pictures/kanagawa");
            persistence = Content.Load<Texture2D>("Tile Pictures/persistence");
            tileMenu = Content.Load<Texture2D>("Menu/tileslideroptions");
            tileMenuPause = Content.Load<Texture2D>("Menu/pauseslideroptions");
            tileMenuOverlay = Content.Load<Texture2D>("Overlays/transparentoverlay");
            DebugFont = Content.Load<SpriteFont>("Fonts/font");
            gameFont = Content.Load<SpriteFont>("Fonts/GameFont");
            VictoryDing = Content.Load<SoundEffect>("Sounds/ding");
            ShuffleSound = Content.Load<SoundEffect>("Sounds/shuffle");
            MenuClick = Content.Load<SoundEffect>("Sounds/click");
            bgMusic = Content.Load<Song>("Sounds/bensound-thejazzpiano");
            MediaPlayer.Play(bgMusic);
        }

        protected override void Update(GameTime gameTime)
        {
            SoundEffect.MasterVolume = EffectsVolume;
            MediaPlayer.Volume = MusicVolume;
            CanvasScale = (float)WindowResX / (float)baseWindowResX;
            keyCurrentState = Keyboard.GetState();
            switch (GameState)
            {
                //Menu
                case GameStateCheck.Menu:
                    {
                        //Checks if the menu object exists, if not, creates one
                        if (MenuSelector == null)
                        {
                            MenuSelector = new TileHolder();
                            MenuSelector.GridSize = 2;
                            MenuSelector.CurrentPicture = tileMenuOverlay;
                            MenuSelector.MenuTileSetup(WindowResX, WindowResY, CanvasScale);
                        }
                        if (CurrentPic == null)
                            CurrentPic = theScream;
                        if (inputManager == null)
                            inputManager = new InputManager();
                        inputManager.MenuInput(keyCurrentState, keyPrevState, MenuSelector, thisInstance);
                        break;
                    }
                    //How To screen
                case GameStateCheck.HowTo:
                    {
                        //Escape to return to menu
                        if (keyCurrentState.IsKeyDown(Keys.Escape) && keyPrevState.IsKeyUp(Keys.Escape))
                        {
                            GameState = GameStateCheck.Menu;
                            MenuClick.Play();
                        }
                        break;
                    }
                    //Options screen
                case GameStateCheck.Options:
                    {
                        inputManager.OptionsInput(keyCurrentState, keyPrevState, MenuSelector, thisInstance);
                        break;
                    }
                case GameStateCheck.SelectPicture:
                    {
                        inputManager.SelectPicInput(keyCurrentState, keyPrevState, MenuSelector, thisInstance);
                        switch (PictureNum)
                        {
                            case 0:
                                {
                                    CurrentPic = theScream;
                                    break;
                                }
                            case 1:
                                {
                                    CurrentPic = starryNight;
                                    break;
                                }
                            case 2:
                                {
                                    CurrentPic = kanagawa;
                                    break;
                                }
                            case 3:
                                {
                                    CurrentPic = persistence;
                                    break;
                                }
                        }
                        break;
                    }
                //Game state input
                case GameStateCheck.Game:
                    {
                        //Timer
                        if (TileHolder.ShuffleCheck == shuffleState.Shuffled && PauseCheck == PauseState.Unpaused)
                        {
                            int secondsElapsed;
                            MillisecondsElapsed += gameTime.ElapsedGameTime.Milliseconds;
                            secondsElapsed = MillisecondsElapsed / 1000;
                            MinutesElapsed = secondsElapsed / 60;
                            RemainderSeconds = secondsElapsed % 60;
                        }
                        //Constantly check if the tiles are in the correct position
                        if (TileHolder.ShuffleCheck == shuffleState.Shuffled)
                            TileHolder.WinCheck(thisInstance);
                        inputManager.GameInput(keyCurrentState, keyPrevState, MenuSelector, thisInstance);
                        break;
                    }
    }
            keyPrevState = keyCurrentState;
            base.Update(gameTime);
        }
        
        //Method for resetting the ingame timer
        public void ResetTimer()
        {
            MillisecondsElapsed = 0;
            MinutesElapsed = 0;
            RemainderSeconds = 0;
        }

        public int ScaleCalc(int inputNum)
        {
            float tempFloat;
            tempFloat = (float)inputNum * CanvasScale;
            inputNum = (int)tempFloat;
            return inputNum;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.WhiteSmoke);
            _spriteBatch.Begin();
            Vector2 screenHalfPoint = new Vector2(WindowResX / 2, WindowResY / 2);
            overlayRect = new Rectangle(0, 0, WindowResX, WindowResY);
            Rectangle menuOptionsRect = new Rectangle((int)MenuSelector.TileCanvasPos.X, (int)MenuSelector.TileCanvasPos.Y, (int)MenuSelector.MenuCanvasSize, (int)MenuSelector.MenuCanvasSize);

            switch (GameState)
            {
                    //Menu
                case GameStateCheck.Menu:
                    {
                        //Drawing the tileMenu texture, and the MenuSelector tiles
                        _spriteBatch.Draw(mainMenuOverlay, overlayRect, Color.White);
                        _spriteBatch.Draw(tileMenu, menuOptionsRect, Color.White);
                        MenuSelector.DrawTiles(_spriteBatch);
                        //Confirm exit screen
                        if (PauseCheck == PauseState.Confirm)
                        {
                            _spriteBatch.Draw(pauseOverlay, overlayRect, Color.White);
                            _spriteBatch.Draw(exitMenuConfirm, overlayRect, Color.White);
                        }
                        break;
                    }
                    //How to screen
                case GameStateCheck.HowTo:
                    {
                        _spriteBatch.Draw(howToScreen, overlayRect, Color.White);
                        break;
                    }
                case GameStateCheck.Options:
                    {
                        string fxVolumeString = "Sound Effects: " + EffectsVolume;
                        string musicVolumeString = "Music: " + MusicVolume;
                        Vector2 fxStringSize = gameFont.MeasureString(fxVolumeString);
                        Vector2 musicStringSize = gameFont.MeasureString(musicVolumeString);
                        Vector2 fxStringPos = new Vector2(screenHalfPoint.X - (fxStringSize.X / 2), WindowResY / 3);
                        Vector2 musicStringPos = new Vector2(screenHalfPoint.X - (musicStringSize.X / 2), fxStringPos.Y + fxStringSize.Y);
                        Rectangle arrowPos1 = new Rectangle((int)fxStringPos.X - (arrowPrompt.Width / 2), (int)fxStringPos.Y, arrowPrompt.Width / 2, arrowPrompt.Height / 2);
                        Rectangle arrowPos2 = new Rectangle((int)musicStringPos.X - (arrowPrompt.Width / 2), (int)musicStringPos.Y, arrowPrompt.Width / 2, arrowPrompt.Height / 2);
                        _spriteBatch.Draw(optionsScreen, overlayRect, Color.White);
                        _spriteBatch.DrawString(gameFont, fxVolumeString, fxStringPos, Color.Black);
                        _spriteBatch.DrawString(gameFont, musicVolumeString, musicStringPos, Color.Black);
                        if (optionsCheck == VolumeOptions.FX)
                            _spriteBatch.Draw(arrowPrompt, arrowPos1, Color.White);
                        if (optionsCheck == VolumeOptions.Music)
                            _spriteBatch.Draw(arrowPrompt, arrowPos2, Color.White);
                        break;
                    }
                //Picture select screen
                case GameStateCheck.SelectPicture:
                    {
                        string gridSizePreview = "Grid Size: " + GridSizeToSet;
                        string[] pictureNames = new string[] {"The Scream", "Starry Night", "The Great Wave off Kanagawa", "The Persistence of Memory"};
                        int previewSize = (CurrentPic.Width - (CurrentPic.Width / 4));
                        previewSize = ScaleCalc(previewSize);
                        int previewPosOffset = (previewSize / 2);
                        Vector2 pictureStringSize = gameFont.MeasureString(pictureNames[PictureNum]);
                        Vector2 gridSizeStringSize = gameFont.MeasureString(gridSizePreview);
                        Vector2 gridSizeStringPos = new Vector2(screenHalfPoint.X - (gridSizeStringSize.X / 2), WindowResY - (WindowResY / 10));
                        Vector2 pictureStringPos = new Vector2(screenHalfPoint.X - (pictureStringSize.X / 2), WindowResY - (WindowResY / 6));
                        Rectangle previewPicPos = new Rectangle(((int)screenHalfPoint.X - previewPosOffset), (WindowResY / 7), previewSize, previewSize);
                        Rectangle overlayBanner = new Rectangle(0, WindowResY - (WindowResY / 5), WindowResX, WindowResY / 5);
                        Rectangle arrowPos1 = new Rectangle((int)gridSizeStringPos.X - (arrowPrompt.Width / 2), WindowResY - (WindowResY / 10), arrowPrompt.Width / 2, arrowPrompt.Height / 2);
                        Rectangle arrowPos2 = new Rectangle((int)pictureStringPos.X - (arrowPrompt.Width / 2), WindowResY - (WindowResY / 6), arrowPrompt.Width / 2, arrowPrompt.Height / 2);
                        _spriteBatch.Draw(pauseOverlay, overlayBanner, Color.White);
                        _spriteBatch.Draw(CurrentPic, previewPicPos, Color.White);
                        if (GridPicSwitch == GridOrPic.GridSize)
                            _spriteBatch.Draw(arrowPrompt, arrowPos1, Color.White);
                        if (GridPicSwitch == GridOrPic.Picture)
                            _spriteBatch.Draw(arrowPrompt, arrowPos2, Color.White);
                        _spriteBatch.DrawString(gameFont, gridSizePreview, gridSizeStringPos, Color.White);
                        _spriteBatch.DrawString(gameFont, pictureNames[PictureNum], pictureStringPos, Color.White);
                        break;
                    }
                //Game
                case GameStateCheck.Game:
                    {
                        string moveCounterString = TileHolder.MoveCounter.ToString();
                        string timerString = MinutesElapsed + "M:" + RemainderSeconds + "S";
                        Vector2 moveCounterStringSize = gameFont.MeasureString(moveCounterString);
                        Vector2 timerStringSize = gameFont.MeasureString(timerString);
                        Vector2 moveCounterWinPos = new Vector2((WindowResX / 2) - (moveCounterStringSize.X / 2), (WindowResY / 2) - (WindowResY / 11));
                        Vector2 timerWinPos = new Vector2((WindowResX / 2) - (timerStringSize.X / 2), (WindowResY / 2) + (WindowResY / 9));
                        Vector2 timerStringGamePos = new Vector2((WindowResX / 2) + (WindowResX / 4) + (timeCounter.Width / 3), WindowResY - (WindowResY / 12));
                        Vector2 moveCounterStringGamePos = new Vector2((WindowResX / 2) - (WindowResX / 4) + (moveCounter.Width / 3), WindowResY - (WindowResY / 12));
                        Rectangle overlayBanner = new Rectangle(0, WindowResY - (WindowResY / 8), WindowResX, WindowResY / 8);
                        Rectangle moveCounterGamePos = new Rectangle(((WindowResX / 2) - moveCounter.Width / 2) - (WindowResX / 4), WindowResY - (WindowResY / 8), moveCounter.Width, moveCounter.Height);
                        Rectangle timerGamePos = new Rectangle(((WindowResX / 2) - timeCounter.Width / 2) + (WindowResX / 4), WindowResY - (WindowResY / 8), timeCounter.Width, timeCounter.Height);
                        Rectangle enterPromptPos = new Rectangle((WindowResX / 2) - (enterPrompt.Width / 2), WindowResY - (WindowResY / 8), enterPrompt.Width, enterPrompt.Height);
                        _spriteBatch.Draw(tileHolderBack, overlayRect, Color.White);
                        _spriteBatch.Draw(pauseOverlay, overlayBanner, Color.White);
                        TileHolder.DrawTiles(_spriteBatch);
                        _spriteBatch.Draw(moveCounter, moveCounterGamePos, Color.White);
                        _spriteBatch.Draw(timeCounter, timerGamePos, Color.White);
                        _spriteBatch.DrawString(gameFont, timerString, timerStringGamePos, Color.White);
                        _spriteBatch.DrawString(gameFont, moveCounterString, moveCounterStringGamePos, Color.White);
                        //Enter to start prompt
                        if (TileHolder.ShuffleCheck == shuffleState.Unshuffled)
                            _spriteBatch.Draw(enterPrompt, enterPromptPos, Color.White);
                        //Debug mode
                        if (DebugMode == true)
                            TileHolder.DrawTileDebug(_spriteBatch, pauseOverlay, MinutesElapsed, RemainderSeconds);
                        //Pause menu
                        if (PauseCheck == PauseState.Paused)
                        {
                            _spriteBatch.Draw(pauseOverlay, overlayRect, Color.White);
                            _spriteBatch.Draw(tileMenuPause, menuOptionsRect, Color.White);
                            MenuSelector.DrawTiles(_spriteBatch);
                        }
                        //Confirm screen
                        if (PauseCheck == PauseState.Confirm)
                        {
                            _spriteBatch.Draw(pauseOverlay, overlayRect, Color.White);
                            if (MenuSelector.MainTile.CurrentTileNum == 1)
                                _spriteBatch.Draw(restartConfirm, overlayRect, Color.White);
                            else if (MenuSelector.MainTile.CurrentTileNum == 2)
                                _spriteBatch.Draw(returnToMenuConfirm, overlayRect, Color.White);
                            else if (MenuSelector.MainTile.CurrentTileNum == 3)
                                _spriteBatch.Draw(exitGameConfirm, overlayRect, Color.White);
                        }
                        if (TileHolder.ShuffleCheck == shuffleState.Solved)
                        {
                            _spriteBatch.Draw(pauseOverlay, overlayRect, Color.White);
                            _spriteBatch.Draw(winScreen, overlayRect, Color.White);
                            _spriteBatch.DrawString(gameFont, moveCounterString, moveCounterWinPos, Color.White);
                            _spriteBatch.DrawString(gameFont, timerString, timerWinPos, Color.White);
                        }
                        break;
                    }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
