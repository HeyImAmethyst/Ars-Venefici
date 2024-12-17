using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spell.Components;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using xTile.Tiles;
using static System.Net.Mime.MediaTypeNames;

namespace ArsVenefici.Framework.Spell.Effects
{
    public class ZoneEffect : AbstractSpellEffect
    {
        ModEntry modEntry;

        private float radius;
        private ISpell spell;
        private int index;
        private readonly Texture2D Tex;

        public ZoneEffect(ModEntry modEntry, ISpell spell, Vector2 pos, float radius, int dur) : base(modEntry, pos, dur) 
        {
            this.modEntry = modEntry;

            this.spell = spell;
            this.radius = radius;

            int boundingBoxRadius = 3;

            switch ((int)radius)
            {
                case 1:
                    boundingBoxRadius = 3;
                    break;
                case 2:
                    boundingBoxRadius = 5;
                    break;
                case 3:
                    boundingBoxRadius = 7;
                    break;
                default:
                    boundingBoxRadius = 3;
                    break;

            }

            boundingBoxRadius *= Game1.tileSize;

            //this.Tex = new Texture2D(Game1.graphics.GraphicsDevice, Game1.tileSize * (int)radius, Game1.tileSize * (int)radius);
            this.Tex = new Texture2D(Game1.graphics.GraphicsDevice, boundingBoxRadius, boundingBoxRadius);

            int width = Tex.Width;
            int height = Tex.Height;
            int area = width * height;

            Color[] data = new Color[area];
            Color manaCol = new Color(0, 48, 255, 127);

            for (int pixel = 0; pixel < data.Count(); pixel++)
            {
                //the function applies the color according to the specified pixel
                data[pixel] = manaCol;
            }

            Tex.SetData(data);
        }

        public override void Update(UpdateTickedEventArgs e)
        {
            if (Game1.activeClickableMenu == null && Game1.game1.IsActive)
                isActive = --this.duration > 0 || GetOwner() == null;

            if (GetOwner().GetGameLocation() != this.GetGameLocation())
                isActive = false;

            if (GetGameLocation() is MineShaft)
            {
                MineShaft effectLocation = (MineShaft)GetGameLocation();
                MineShaft playerLocation = (MineShaft)GetOwner().GetGameLocation();

                if (effectLocation.mineLevel != playerLocation.mineLevel)
                    isActive = false;
            }
        }

        public override void OneSecondUpdate(OneSecondUpdateTickingEventArgs e)
        {
            IEntity owner = GetOwner();

            int index = GetIndex();
            float radius = GetRadius();
            ISpell spell = GetSpell();

            var spellHelper = SpellHelper.Instance();

            if (Game1.activeClickableMenu == null && Game1.game1.IsActive)
            {
                ForAllInRange((int)radius, false, e => spellHelper.Invoke(modEntry, spell, owner, GetGameLocation(), new CharacterHitResult(e), 0, index, true));

                for (int x = (int)(pos.X - radius); x <= pos.X + radius; ++x)
                {
                    for (int y = (int)(pos.Y - radius); y <= pos.Y + radius; ++y)
                    {
                        TilePos newTilePos = new TilePos(x, y);
                        spellHelper.Invoke(modEntry, spell, owner, GetGameLocation(), new TerrainFeatureHitResult(pos, 0, newTilePos, false), 0, index, true);
                    }
                }

                if (!isActive)
                {
                    modEntry.ActiveEffects.Remove(this);
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle r = new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)radius, (int)radius);

            Vector2 tilePos = new Vector2((int)r.X, (int)r.Y);
            Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);
            Vector2 screenPos = Utils.TilePosToScreenPos(tilePos);

            //spriteBatch.Draw(Tex, Game1.GlobalToLocal(Game1.viewport, this.GetBoundingBox()), new Rectangle(0, 0, Tex.Width, Tex.Height), Color.White);
            //spriteBatch.Draw(Tex, screenPos, r, Color.White);

            float speed = -0.5f;

            for (int x = (int)-radius; x <= (int)radius; x++)
            {
                for (int y = (int)-radius; y <= (int)radius; y++)
                {
                    Vector2 vec = new Vector2(r.X + x, r.Y + y);
                    Vector2 absPos = Utils.TilePosToAbsolutePos(vec);

                    TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(372, 1956, 10, 10), absPos, false, 1f / 500f, new Color(0, 48, 255, 127))
                    {
                        alphaFade = (float)(1.0 / 1000.0 - (double)speed / 300.0),
                        alpha = 0.1f,
                        //motion = new Vector2(0.0f, speed),
                        //acceleration = new Vector2(0.0f, 0.0f),
                        interval = 99999f,
                        layerDepth = (float)(this.GetBoundingBox().Bottom - 3 - Game1.random.Next(5)) / 10000f,
                        scale = 8f,
                        scaleChange = 0.01f,
                        rotationChange = (float)((double)Game1.random.Next(-5, 6) * 3.1415927410125732 / 256.0)
                    };

                    this.GetGameLocation().temporarySprites.Add(sprite);
                }
            }
        }

        public override int GetDuration()
        {
            return this.duration;
        }

        public void SetDuration(int duration)
        {
            this.duration = duration;
        }

        public override void SetOwner(IEntity owner)
        {
            this.owner = owner;
        }

        public int GetIndex()
        {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }
        public float GetRadius()
        {
            return radius;
        }

        public void SetRadius(float radius)
        {
            this.radius = radius;
        }

        public ISpell GetSpell()
        {
            return spell;
        }

        public void SetSpell(ISpell spell)
        {
            this.spell = spell;
        }
    }
}
