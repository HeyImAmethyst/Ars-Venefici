using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public class TilePos
    {
        private int tilePosX;
        private int tilePosY;

        public TilePos(int x, int y)
        {
            tilePosX = x;
            tilePosY = y;
        }

        public TilePos(Vector2 vec)
        {
            tilePosX = (int)vec.X;
            tilePosY = (int)vec.Y;
        }

        public TilePos(Character character)
        {
            Vector2 tilePos = Utils.ConvertToTilePos(Utility.clampToTile(character.getStandingPosition()));

            tilePosX = (int)tilePos.X;
            tilePosY = (int)tilePos.Y;
        }

        public int GetTilePosX()
        {
            return tilePosX;
        }

        public int GetTilePosY()
        {
            return tilePosY;
        }

        public Vector2 GetVector()
        {
            return new Vector2(tilePosX, tilePosY);
        }

        public void Add(int x, int y)
        {
            tilePosX += x;
            tilePosY += y;
        }
    }
}
