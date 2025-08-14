using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.Client
{
    public interface IRenderable
    {
        void Draw(SpriteBatch spriteBatch, int positionX, int positionY, float pPartialTick);
    }
}
