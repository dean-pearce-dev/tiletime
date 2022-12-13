using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TileTime
{
    public enum shuffleState { Unshuffled, Shuffled, Solved };
    //Custom class for keeping track of tiles. TileHolder sets the area to use for the puzzle, and contains a list which holds objects from the custom class Tile.
    public class TileHolder
    {
        private int tilesToSet;
        private int tilesSet;
        private int canvasGapPercent;
        private int inflatedCanvasSize;
        public int GridSize { get; set; } = 3;
        public int TrueCanvasSize { get; set; } = 800;
        public float CanvasScale { get; set; }
        public float MenuCanvasSize { get; set; }
        public Vector2 TileCanvasPos { get; set; }
        private List<Tile> tileList = new List<Tile>();
        public Texture2D CurrentPicture { get; set; }
        public SpriteFont Font { get; set; }
        public Tile MainTile { get; set; }
        public int MoveCounter { get; set; }
        public int InversionCount { get; set; }
        public int CorrectTiles { get; set; }
        private Random rnd = new Random();
        public shuffleState ShuffleCheck { get; set; }

        //Setting up the tile holder, and adding each tile to a list
        public void TileSetup(int windowResX, int windowResY, float canvScale)
        {
            CanvasScale = canvScale;
            float currentCanvasSize = (float)TrueCanvasSize * canvScale;
            Tile currentTile;
            tilesToSet = GridSize * GridSize;
            canvasGapPercent = ((int)currentCanvasSize / 100) * 3;
            inflatedCanvasSize = (int)currentCanvasSize + canvasGapPercent;
            TileCanvasPos = new Vector2((windowResX / 2) - (inflatedCanvasSize / 2), (windowResY / 3) - (inflatedCanvasSize / 3));
            Vector2 tileSection = new Vector2(CurrentPicture.Width / GridSize, CurrentPicture.Height / GridSize);
            int tileSize = inflatedCanvasSize / GridSize;
            MoveCounter = 0;
            //For loop for iterating through the rows and columns
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    //Position to assign the tile in relation to the space the TileHolder uses
                    Vector2 tilePos = new Vector2(TileCanvasPos.X + (tileSize * j), TileCanvasPos.Y + (tileSize * i));
                    if (tilesSet < tilesToSet)
                    {
                        //Adding a tile to the list, and automatically assigning it's correct values
                        tileList.Add(new Tile()
                        {
                            OrigTileNum = tilesSet,
                            CurrentTileNum = tilesSet,
                            OrigColumn = j,
                            CurrentColumn = j,
                            OrigRow = i,
                            CurrentRow = i,
                            TileStartPos = tilePos,
                            TileCurrentPos = tilePos,
                            TileSectionPos = new Vector2(tileSection.X * j, tileSection.Y * i),
                            TileSectionSize = tileSection,
                            TileSection = new Rectangle((int)tileSection.X * j, (int)tileSection.Y * i, (int)tileSection.X, (int)tileSection.Y),
                        }) ; ;

                        currentTile = tileList[tilesSet];
                        //Setting the MainTile to be the bottom right tile
                        if (i == GridSize - 1 && j == GridSize - 1)
                            MainTile = currentTile;
                        tilesSet++;
                    }
                }
            }

        }
        //Method for clearing the tile list, so there's no conflicting entries when the game is reset without exiting
        public void ClearTileList()
        {
            ShuffleCheck = shuffleState.Unshuffled;
            tileList.Clear();
            tilesSet = 0;
            InversionCount = 0;
        }

        //Method for defaulting the menu tile's position on pause/unpause
        public void ResetMenuTile()
        {
            if (MainTile.CurrentTileNum != 0)
            {
                TilesInRange(MainTile);
                SwapTiles(MainTile, MainTile.TileLeft);
            }
            if (MainTile.CurrentTileNum != 0)
            {
                TilesInRange(MainTile);
                SwapTiles(MainTile, MainTile.TileAbove);
            }
        }

        //Similar method to TileSetup above, however, tailored to suit a 2x2 grid which is used for the menu navigation
        public void MenuTileSetup(int windowResX, int windowResY, float canvScale)
        {
            CanvasScale = canvScale / 2;
            MenuCanvasSize = ((float)TrueCanvasSize / 2) * canvScale;
            Tile currentTile;
            tilesToSet = GridSize * GridSize;
            TileCanvasPos = new Vector2((windowResX / 2) - (MenuCanvasSize / 2), (windowResY / 2) - (MenuCanvasSize / 2));
            Vector2 tileSection = new Vector2(CurrentPicture.Width / GridSize, CurrentPicture.Height / GridSize);
            int tileSize = (int)MenuCanvasSize / GridSize;
            //For loop for iterating through the rows and columns
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    //Position to assign the tile in relation to the space the TileHolder uses
                    Vector2 tilePos = new Vector2(TileCanvasPos.X + (tileSize * j), TileCanvasPos.Y + (tileSize * i));
                    if (tilesSet < tilesToSet)
                    {
                        //Adding a tile to the list, and automatically assigning it's correct values
                        tileList.Add(new Tile()
                        {
                            OrigTileNum = tilesSet,
                            CurrentTileNum = tilesSet,
                            OrigColumn = j,
                            CurrentColumn = j,
                            OrigRow = i,
                            CurrentRow = i,
                            TileStartPos = tilePos,
                            TileCurrentPos = tilePos,
                            TileSectionPos = new Vector2(tileSection.X * j, tileSection.Y * i),
                            TileSectionSize = tileSection,
                            TileSection = new Rectangle((int)tileSection.X * j, (int)tileSection.Y * i, (int)tileSection.X, (int)tileSection.Y),
                        }); ;

                        currentTile = tileList[tilesSet];
                        //Setting the MainTile to be the top left tile
                        if (i == 0 && j == 0)
                            MainTile = currentTile;
                        tilesSet++;
                    }
                }
            }

        }

        //Method to swap two tiles values
        public void SwapTiles(Tile currentTile, Tile targetTile)
        {
            //If the targetTile is null, the code doesn't execute in order to avoid an error/crash
            if (targetTile != null)
            {
                int tempColumn;
                int tempRow;
                int tempTileNum;
                Vector2 tempPos;
                tempColumn = targetTile.CurrentColumn;
                tempRow = targetTile.CurrentRow;
                tempPos = targetTile.TileCurrentPos;
                tempTileNum = targetTile.CurrentTileNum;
                targetTile.CurrentColumn = currentTile.CurrentColumn;
                targetTile.CurrentRow = currentTile.CurrentRow;
                targetTile.TileCurrentPos = currentTile.TileCurrentPos;
                targetTile.CurrentTileNum = currentTile.CurrentTileNum;
                currentTile.CurrentColumn = tempColumn;
                currentTile.CurrentRow = tempRow;
                currentTile.TileCurrentPos = tempPos;
                currentTile.CurrentTileNum = tempTileNum;
            }
        }

        //Method for checking whether the puzzle is solved or not
        public void WinCheck(Game1 game)
        {
            int correctTiles = 0;
            for (int i = 0; i < tileList.Count; i++)
            {
                if (tileList[i].CurrentColumn == tileList[i].OrigColumn && tileList[i].CurrentRow == tileList[i].OrigRow)
                    correctTiles++;
            }
            if (correctTiles == tilesSet)
            {
                ShuffleCheck = shuffleState.Solved;
                game.VictoryDing.Play();
            }
            else ShuffleCheck = shuffleState.Shuffled;
        }

        //Method for shuffling the tiles. Uses the Fisher-Yates Algorithm to shuffle, then a series of conditionals to ensure the puzzle is always solvable
        public void ShuffleTiles()
        {
            MoveCounter = 0;
            InversionCount = 0;
            int n = tileList.Count;
            //Fisher-Yates Algorithm
            for (int i = 0; i < (n - 1); i++)
            {
                int random = i + rnd.Next(n - i);
                Tile tempTile = tileList[random];
                tileList[random] = tileList[i];
                tileList[i] = tempTile;
                SwapTiles(tileList[i], tileList[random]);
            }
            //End of Fisher-Yates

            //Nested for loop to check for total number of inversions, where an inversion is when a larger number appears before a smaller number in the sequence
            for (int i = 0; i < tileList.Count - 1; i++)
            {
                for (int j = i + 1; j < tileList.Count; j++)
                {
                    if (tileList[i].OrigTileNum > tileList[j].OrigTileNum && tileList[i] != MainTile)
                        InversionCount++;
                }
            }

            //Using % operand in a way that lets me check for even or odd numbers
            //Odd grid size number, and even inversions = solvable
            //Even grid size number, and main tile is on an even row counting from bottom, and odd inversions = solvable
            //Even grid size number, and main tile is on an odd row counting from bottom, and even inversions = solvable
            //In all other instances, puzzle can be made solvable by swapping either the first and second tile, or the last and second-last tile, as long as none are the main tile
            if (GridSize % 2 != 0 && InversionCount % 2 == 0)
                ShuffleCheck = shuffleState.Shuffled;
            else if (GridSize % 2 == 0 && InversionCount % 2 != 0 && MainTile.CurrentRow % 2 == 0)
                ShuffleCheck = shuffleState.Shuffled;
            else if (GridSize % 2 == 0 && InversionCount % 2 == 0 && MainTile.CurrentRow % 2 != 0)
                ShuffleCheck = shuffleState.Shuffled;
            else
            {
                //In unsolvable case, swaps first and second tile around in the index, as long as neither are the main tile, otherwise swaps the last two in the index
                if (tileList[0] != MainTile && tileList[1] != MainTile)
                {
                    SwapTiles(tileList[0], tileList[1]);
                }
                else
                {
                    SwapTiles(tileList[tileList.Count -1], tileList[tileList.Count - 2]);
                }
            }
            ShuffleCheck = shuffleState.Shuffled;
        }

        //Method for drawing the tiles, using a for loop to iterate through each tile and use it's values for rendering
        public void DrawTiles(SpriteBatch _spriteBatch)
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                Tile currentTile = tileList[i];
                if (currentTile != MainTile || currentTile == MainTile && ShuffleCheck != shuffleState.Shuffled)
                _spriteBatch.Draw(CurrentPicture, currentTile.TileCurrentPos, currentTile.TileSection, currentTile.CurrentColor, 0f, new Vector2(0, 0), CanvasScale, SpriteEffects.None, 1);
            }
        }

        //Debug method, for checking tile values, move and time counters, and game states
        public void DrawTileDebug(SpriteBatch _spriteBatch, Texture2D overlayTex, int minutes, int seconds)
        {
            Vector2 topLeft = new Vector2(100, 100);
            Vector2 topLeft2 = new Vector2(100, 150);
            Vector2 topLeft3 = new Vector2(100, 200);
            Vector2 topLeft4 = new Vector2(100, 250);
            string moveCountString = "Moves: " + MoveCounter;
            string inversionCountString = "Inversions: " + InversionCount;
            string shuffleStateString = "Shuffle State: " + ShuffleCheck;
            string timeElapsedString = "Time: " + minutes + ":" + seconds;

            //For loop to draw each tile's values in the debug mode
            for (int i = 0; i < tileList.Count; i++)
            {
                Color correctPos = Color.Red;
                Tile currentTile = tileList[i];
                Vector2 secondLine = new Vector2(currentTile.TileCurrentPos.X, currentTile.TileCurrentPos.Y + 20);
                Vector2 thirdLine = new Vector2(currentTile.TileCurrentPos.X, currentTile.TileCurrentPos.Y + 40);
                Vector2 fourthLine = new Vector2(currentTile.TileCurrentPos.X, currentTile.TileCurrentPos.Y + 60);
                Vector2 fifthLine = new Vector2(currentTile.TileCurrentPos.X, currentTile.TileCurrentPos.Y + 80);
                if (currentTile.CurrentRow == currentTile.OrigRow && currentTile.CurrentColumn == currentTile.OrigColumn)
                    correctPos = Color.Green;
                string currentRowColumnString = "Current[R" + currentTile.CurrentRow + "][C" + currentTile.CurrentColumn + "]";
                string origRowColumnString = "Original[R" + currentTile.OrigRow + "][C" + currentTile.OrigColumn + "]";
                string tileIndexNum = "[Index " + tileList.IndexOf(currentTile) + "]";
                string currentTileNum = "[Num. " + currentTile.CurrentTileNum + "]";
                string origTileNum = "[Original Num. " + currentTile.OrigTileNum + "]";
                _spriteBatch.Draw(overlayTex, currentTile.TileCurrentPos, currentTile.TileSection, Color.White, 0f, new Vector2(0, 0), CanvasScale, SpriteEffects.None, 1);
                _spriteBatch.DrawString(Font, tileIndexNum, currentTile.TileCurrentPos, Color.White);
                _spriteBatch.DrawString(Font, origTileNum, secondLine, Color.White);
                _spriteBatch.DrawString(Font, currentTileNum, thirdLine, Color.White);
                _spriteBatch.DrawString(Font, currentRowColumnString, fourthLine, Color.White);
                _spriteBatch.DrawString(Font, origRowColumnString, fifthLine, correctPos);
            }
            //Drawing the states and counters outside of the for loop
            _spriteBatch.DrawString(Font, inversionCountString, topLeft, Color.Black);
            _spriteBatch.DrawString(Font, shuffleStateString, topLeft2, Color.Black);
            _spriteBatch.DrawString(Font, moveCountString, topLeft3, Color.Black);
            _spriteBatch.DrawString(Font, timeElapsedString, topLeft4, Color.Black);
        }

        //Method for checking what tiles are in range of the current tile
        public void TilesInRange(Tile Tile)
        {
            Tile currentTile = Tile;
            int currentRow = currentTile.CurrentRow;
            int currentColumn = currentTile.CurrentColumn;
            currentTile.TileAbove = tileList.Find(i => (i.CurrentRow == currentRow - 1) && (i.CurrentColumn == currentColumn));
            currentTile.TileBelow = tileList.Find(i => (i.CurrentRow == currentRow + 1) && (i.CurrentColumn == currentColumn)); ;
            currentTile.TileLeft = tileList.Find(i => (i.CurrentRow == currentRow) && (i.CurrentColumn == currentColumn - 1)); ;
            currentTile.TileRight = tileList.Find(i => (i.CurrentRow == currentRow) && (i.CurrentColumn == currentColumn + 1)); ;
        }
    }
}
