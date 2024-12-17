using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI.Events;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley.Locations;

namespace ArsVenefici.Framework.Spell.Effects
{
    public class WallEffect : AbstractSpellEffect
    {
        ModEntry modEntry;

        private float radius;
        private int boundingBoxRadius;
        private int facingDirection;
        private ISpell spell;
        private int index;
        private readonly Texture2D Tex;

        public WallEffect(ModEntry modEntry, ISpell spell, Vector2 pos, float radius, int dur, int facingDirection) : base(modEntry, pos, dur)
        {
            this.modEntry = modEntry;

            this.spell = spell;
            this.radius = radius;
            this.facingDirection = facingDirection;

            boundingBoxRadius = 3;

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

            if(GetGameLocation() is MineShaft)
            {
                MineShaft effectLocation = (MineShaft) GetGameLocation();
                MineShaft playerLocation = (MineShaft) GetOwner().GetGameLocation();

                if(effectLocation.mineLevel != playerLocation.mineLevel)
                    isActive = false;
            }

            IEntity owner = GetOwner();

            int index = GetIndex();
            //float radius = GetRadius();
            ISpell spell = GetSpell();

            Farmer farmer = owner.entity as Farmer;

            var spellHelper = SpellHelper.Instance();

            if (Game1.activeClickableMenu == null && Game1.game1.IsActive)
            {

                if (e.IsMultipleOf(15))
                {
                    ForAllInRange((int)radius, false, e => spellHelper.Invoke(modEntry, spell, owner, GetGameLocation(), new CharacterHitResult(e), 0, index, true));

                    if (facingDirection == 1 || facingDirection == 3)
                    {
                        Vector2 tilePos = new Vector2(GetPosition().X, GetPosition().Y - radius);
                        Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);

                        SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), 1 * Game1.tileSize, (int)(boundingBoxRadius)));

                        for (int y = (int)(pos.Y - radius); y <= pos.Y + radius; ++y)
                        {
                            TilePos newTilePos = new TilePos((int)pos.X, y);
                            spellHelper.Invoke(modEntry, spell, owner, GetGameLocation(), new TerrainFeatureHitResult(pos, 0, newTilePos, false), 0, index, true);
                        }
                    }

                    if (facingDirection == 0 || facingDirection == 2)
                    {
                        Vector2 tilePos = new Vector2(GetPosition().X - radius, GetPosition().Y);
                        Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);

                        SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), boundingBoxRadius, 1 * Game1.tileSize));

                        for (int x = (int)(pos.X - radius); x <= pos.X + radius; ++x)
                        {
                            TilePos newTilePos = new TilePos(x, (int)pos.Y);
                            spellHelper.Invoke(modEntry, spell, owner, GetGameLocation(), new TerrainFeatureHitResult(pos, 0, newTilePos, false), 0, index, true);
                        }
                    }


                    if (!isActive)
                    {
                        modEntry.ActiveEffects.Remove(this);
                    }
                }
            }
        }

        public override void OneSecondUpdate(OneSecondUpdateTickingEventArgs e)
        {
            
        }
        public override void Draw(SpriteBatch spriteBatch)
        {

            Rectangle r = new Rectangle((int)this.pos.X, (int)this.pos.Y, (int)1, (int)radius);

            Vector2 tilePos = new Vector2((int)r.X, (int)r.Y);
            Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);
            Vector2 screenPos = Utils.TilePosToScreenPos(tilePos);

            float speed = -0.5f;
            //float speed = -0.3f;

            if (facingDirection == 1 || facingDirection == 3)
            {
                for (int y = (int)(pos.Y - radius); y <= pos.Y + radius; ++y)
                {
                    Vector2 vec = new Vector2(r.X, y);
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

            if (facingDirection == 0 || facingDirection == 2)
            {
                for (int x = (int)(pos.X - radius); x <= pos.X + radius; ++x)
                {
                    Vector2 vec = new Vector2(x, r.Y);
                    Vector2 absPos = Utils.TilePosToAbsolutePos(vec);

                    TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite("LooseSprites\\Cursors", new Rectangle(372, 1956, 10, 10), absPos, false, 1f / 500f, new Color(0, 48, 255, 127))
                    {
                        //alphaFade = (float)(1.0 / 1000.0 - (double)speed / 300.0),
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
