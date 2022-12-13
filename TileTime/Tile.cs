using Microsoft.Xna.Framework;

namespace TileTime
{
    //Tile class for storing values for each individual tile
    public class Tile
    {
        private Color currentColor = Color.White;
        private Vector2 tileStartPos;
        private Vector2 tileCurrentPos;
        private Vector2 tileSectionPos;
        private Vector2 tileSectionSize;
        private Rectangle tileSection;
        public bool CorrectPos { get; set; }
        public int OrigTileNum { get; set; }
        public int CurrentTileNum { get; set; }
        public int OrigColumn { get; set; }
        public int OrigRow { get; set; }
        public int CurrentRow { get; set; }
        public int CurrentColumn { get; set; }
        public Tile TileAbove { get; set; }
        public Tile TileBelow { get; set; }
        public Tile TileLeft { get; set; }
        public Tile TileRight { get; set; }
        public Color CurrentColor
        {
            get { return currentColor; }
            set { currentColor = value; }
        }
        public Vector2 TileStartPos
        {
            get { return tileStartPos; }
            set { tileStartPos = value; }
        }
        public Vector2 TileCurrentPos
        {
            get { return tileCurrentPos; }
            set { tileCurrentPos = value; }
        }
        public Vector2 TileSectionPos
        {
            get { return tileSectionPos; }
            set { tileSectionPos = value; }
        }
        public Vector2 TileSectionSize
        {
            get { return tileSectionSize; }
            set { tileSectionSize = value; }
        }
        public Rectangle TileSection
        {
            get { return tileSection; }
            set { tileSection = value; }
        }
    }
}
