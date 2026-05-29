using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;

namespace ArsVenefici.Framework.Util
{
    public class RenderUtils
    {

        static Texture2D _pointTexture;

        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(_pointTexture, point1, null, color, angle, Vector2.Zero, new Vector2(length, lineWidth), SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draws a fractal line.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch to draw with.</param>
        /// <param name="startX">The start x coordinate.</param>
        /// <param name="startY">The start y coordinate.</param>
        /// <param name="endX">The end x coordinate.</param>
        /// <param name="endY">The end y coordinate.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="displace">The displace value to use.</param>
        /// <param name="fractalDetail">The fractal detail to use.</param>

        public static void FractalLine2dd(SpriteBatch spriteBatch, double startX, double startY, double endX, double endY, Color color, float displace, float fractalDetail, int lineWidth)
        {
            FractalLine2df(spriteBatch, (float)startX, (float)startY, (float)endX, (float)endY, color, displace, fractalDetail, lineWidth);
        }

        /// <summary>
        /// Draws a fractal line.
        /// </summary>
        /// <param name="spriteBatch">The spriteBatch to draw with.</param>
        /// <param name="startX">The start x coordinate.</param>
        /// <param name="startY">The start y coordinate.</param>
        /// <param name="endX">The end x coordinate.</param>
        /// <param name="endY">The end y coordinate.</param>
        /// <param name="color">The color of the line.</param>
        /// <param name="displace">The displace value to use.</param>
        /// <param name="fractalDetail">The fractal detail to use.</param>
        public static void FractalLine2df(SpriteBatch spriteBatch, float startX, float startY, float endX, float endY, Color color, float displace, float fractalDetail, int lineWidth)
        {
            if (displace < fractalDetail)
            {
                DrawLine(spriteBatch, new Vector2(startX, startY), new Vector2(endX, endY), color, lineWidth);
            }
            else
            {
                int mx = (int)((endX + startX) / 2);
                int my = (int)((endY + startY) / 2);

                //Random random = Optional.ofNullable(Minecraft.getInstance().level).map(Level::getRandom).orElseGet(RandomSource::create);
                //Random random = ModEntry.RandomGen;
                Random random = Game1.random;

                //mx += (int)((Utils.NextFloat(random) - 0.5) * displace);
                //my += (int)((Utils.NextFloat(random) - 0.5) * displace);

                mx += (int)((random.NextDouble() - 0.5) * displace);
                my += (int)((random.NextDouble() - 0.5) * displace);

                FractalLine2df(spriteBatch, startX, startY, mx, my, color, displace / 2f, fractalDetail, lineWidth);
                FractalLine2df(spriteBatch, endX, endY, mx, my, color, displace / 2f, fractalDetail, lineWidth);
            }
        }
    }
}
