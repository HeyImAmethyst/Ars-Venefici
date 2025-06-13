using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArsVenefici.Framework.Spells.Effects;
using ItemExtensions.Additions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class Beam : AbstractShape
    {
        private Rectangle boundingBox;
        private Rectangle horizontalBoundingBox;

        public Beam() : base()
        {

        }

        public override string GetId()
        {
            return "beam";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation level, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            var spellHelper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();

            float radius = spellHelper.GetModifiedStat(6, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, hit, index);
            bool appliedToAtLeastOneEntity = false;
            //radius *= Game1.tileSize;

            Farmer farmer = caster.entity as Farmer;
            Vector2 standingPosition = farmer.Tile;

            Rectangle rectangle = new Rectangle();

            Vector2 hTilePos = new Vector2(standingPosition.X + radius, standingPosition.Y);
            Vector2 hAbsoluteTilePos = Utils.TilePosToAbsolutePos(hTilePos);

            //wallEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), 1 * Game1.tileSize, (int)(boundingBoxRadius)));
            Rectangle hRectangle = new Rectangle((int)hAbsoluteTilePos.X, (int)hAbsoluteTilePos.Y, (int)radius * Game1.tileSize, Game1.tileSize);

            SetHorizontalBoundingBox(hRectangle);

            if (farmer.FacingDirection == 1) //right
            {
                Vector2 tilePos = new Vector2(standingPosition.X + radius, standingPosition.Y);
                Vector2 absoluteTilePos = Utils.TilePosToAbsolutePos(tilePos);

                //wallEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), 1 * Game1.tileSize, (int)(boundingBoxRadius)));
                rectangle = new Rectangle((int)absoluteTilePos.X, (int)absoluteTilePos.Y, (int)radius * Game1.tileSize, Game1.tileSize);

                SetBoundingBox(rectangle);

                foreach (Character e in GameLocationUtils.GetCharacters(caster, rectangle))
                {
                    if (spellHelper.Invoke(modEntry, spell, caster, level, new CharacterHitResult(e), ticksUsed, index, awardXp) == new SpellCastResult(SpellCastResultType.SUCCESS))
                    {
                        appliedToAtLeastOneEntity = true;
                    }
                }

                if (appliedToAtLeastOneEntity)
                    return new SpellCastResult(SpellCastResultType.SUCCESS);

                TilePos pos = new TilePos(standingPosition);

                int rad = (int)radius;

                for (int x = (int)pos.GetVector().X; x <= pos.GetVector().X + radius; ++x)
                {
                    TilePos newTilePos = new TilePos(x, (int)pos.GetVector().Y);
                    spellHelper.Invoke(modEntry, spell, caster, level, new TerrainFeatureHitResult(pos.GetVector(), 0, newTilePos, false), 0, index, true);
                }
            }

            if (farmer.FacingDirection == 3) //left
            {
                Vector2 tilePos = new Vector2(standingPosition.X - radius, standingPosition.Y);
                Vector2 absoluteTilePos = Utils.TilePosToAbsolutePos(tilePos);

                //wallEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), 1 * Game1.tileSize, (int)(boundingBoxRadius)));
                rectangle = new Rectangle((int)absoluteTilePos.X, (int)absoluteTilePos.Y, (int)radius * Game1.tileSize, Game1.tileSize);

                SetBoundingBox(rectangle);

                foreach (Character e in GameLocationUtils.GetCharacters(caster, rectangle))
                {
                    if (spellHelper.Invoke(modEntry, spell, caster, level, new CharacterHitResult(e), ticksUsed, index, awardXp) == new SpellCastResult(SpellCastResultType.SUCCESS))
                    {
                        appliedToAtLeastOneEntity = true;
                    }
                }

                if (appliedToAtLeastOneEntity)
                    return new SpellCastResult(SpellCastResultType.SUCCESS);

                TilePos pos = new TilePos(standingPosition);

                int rad = (int)radius;

                for (int x = (int)(pos.GetVector().X - radius); x <= pos.GetVector().X; ++x)
                {
                    TilePos newTilePos = new TilePos(x, (int)pos.GetVector().Y);
                    spellHelper.Invoke(modEntry, spell, caster, level, new TerrainFeatureHitResult(pos.GetVector(), 0, newTilePos, false), 0, index, true);
                }
            }

            if (farmer.FacingDirection == 0) // up
            {
                Vector2 tilePos = new Vector2(standingPosition.X, standingPosition.Y - radius);
                Vector2 absoluteTilePos = Utils.TilePosToAbsolutePos(tilePos);

                //wallEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), boundingBoxRadius, 1 * Game1.tileSize));
                rectangle = new Rectangle((int)absoluteTilePos.X, (int)absoluteTilePos.Y, Game1.tileSize, (int)radius * Game1.tileSize);
                
                SetBoundingBox(rectangle);

                foreach (Character e in GameLocationUtils.GetCharacters(caster, rectangle))
                {
                    if (spellHelper.Invoke(modEntry, spell, caster, level, new CharacterHitResult(e), ticksUsed, index, awardXp) == new SpellCastResult(SpellCastResultType.SUCCESS))
                    {
                        appliedToAtLeastOneEntity = true;
                    }
                }

                if (appliedToAtLeastOneEntity)
                    return new SpellCastResult(SpellCastResultType.SUCCESS);

                TilePos pos = new TilePos(standingPosition);

                int rad = (int)radius;

                for (int y = (int)(pos.GetVector().Y - radius); y <= pos.GetVector().Y; ++y)
                {
                    TilePos newTilePos = new TilePos((int)pos.GetVector().X, y);
                    spellHelper.Invoke(modEntry, spell, caster, level, new TerrainFeatureHitResult(pos.GetVector(), 0, newTilePos, false), 0, index, true);
                }
            }

            if (farmer.FacingDirection == 2) // down
            {
                Vector2 tilePos = new Vector2(standingPosition.X, standingPosition.Y + radius);
                Vector2 absoluteTilePos = Utils.TilePosToAbsolutePos(tilePos);

                //wallEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), boundingBoxRadius, 1 * Game1.tileSize));
                rectangle = new Rectangle((int)absoluteTilePos.X, (int)absoluteTilePos.Y, Game1.tileSize, (int)radius * Game1.tileSize);

                SetBoundingBox(rectangle);

                foreach (Character e in GameLocationUtils.GetCharacters(caster, rectangle))
                {
                    if (spellHelper.Invoke(modEntry, spell, caster, level, new CharacterHitResult(e), ticksUsed, index, awardXp) == new SpellCastResult(SpellCastResultType.SUCCESS))
                    {
                        appliedToAtLeastOneEntity = true;
                    }
                }

                if (appliedToAtLeastOneEntity)
                    return new SpellCastResult(SpellCastResultType.SUCCESS);

                TilePos pos = new TilePos(standingPosition);

                int rad = (int)radius;

                for (int y = (int)pos.GetVector().Y; y <= (int)(pos.GetVector().Y + radius); ++y)
                {
                    TilePos newTilePos = new TilePos((int)pos.GetVector().X, y);
                    spellHelper.Invoke(modEntry, spell, caster, level, new TerrainFeatureHitResult(pos.GetVector(), 0, newTilePos, false), 0, index, true);
                }
            }

            //Texture2D aoeTexture = modEntry.Helper.ModContent.Load<Texture2D>("assets/beam/beam_animated.png");
            //Vector2 local = new Vector2(boundingBox.X, boundingBox.Y);

            //float speed = -3f;

            //Rectangle imageSourceRect = new Rectangle(0, 0, 16, 32);
            //var size = ExpandToBound(new Rectangle((int)0, (int)0, imageSourceRect.Width, imageSourceRect.Height), boundingBox);

            //TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite()
            //{
            //    initialParentTileIndex = 0,
            //    interval = 40f,
            //    totalNumberOfLoops = 50,
            //    position = local,
            //    animationLength = 6,
            //    flicker = false,
            //    flipped = false,
            //    texture = aoeTexture,
            //    sourceRect = imageSourceRect,
            //    sourceRectStartingPos = new Vector2(imageSourceRect.X, imageSourceRect.Y),
            //    initialPosition = local,
            //    //motion = new Vector2(0.0f, speed),
            //    //acceleration = new Vector2(0.0f, 0.0f),
            //    layerDepth = (float)(boundingBox.Bottom - 3 - Game1.random.Next(5)) / 10000f,
            //    scale = (float)size,
            //    scaleChange = 0.01f,
            //    color = Color.White
            //};

            //Game1.Multiplayer.broadcastSprites(level, sprite);

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public Rectangle GetBoundingBox()
        {
            return boundingBox;
        }

        public void SetBoundingBox(Rectangle boundingBox)
        {
            this.boundingBox = boundingBox;
        }

        public Rectangle GetHorizontalBoundingBox()
        {
            return horizontalBoundingBox;
        }

        public void SetHorizontalBoundingBox(Rectangle horizontalBoundingBox)
        {
            this.horizontalBoundingBox = horizontalBoundingBox;
        }

        public override bool IsContinuous()
        {
            return true;
        }

        public override bool NeedsPrecedingShape()
        {
            return false;
        }

        public override bool NeedsToComeFirst()
        {
            return true;
        }

        public override float ManaCost()
        {
            return 3;
        }

        private double ExpandToBound(Rectangle image, Rectangle boundingBox)
        {
            double widthScale = 0, heightScale = 0;

            if (image.Width != 0)
                widthScale = boundingBox.Width / (double)image.Width;
            if (image.Height != 0)
                heightScale = boundingBox.Height / (double)image.Height;

            double scale = Math.Min(widthScale, heightScale);

            //Rectangle result = new Rectangle((int)(image.Width * scale),
            //                    (int)(image.Height * scale));
            return scale;
        }
    }
}
