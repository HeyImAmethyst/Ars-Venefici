using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spells.Effects;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Shape
{
    public class AoE : AbstractShape
    {
        private Rectangle boundingBox;
        private SpellCastResult spellCastResult;

        public AoE() : base(new SpellPartStats(SpellPartStatType.RANGE))
        {

        }

        public override string GetId()
        {
            return "aoe";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            //Game1.showRedMessage("Invoking Spell Part " + GetId());
            //modEntry.Monitor.Log("Invoking Spell Part " + GetId(), StardewModdingAPI.LogLevel.Info);

            if (hit == null)
            {
                spellCastResult = new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
                return spellCastResult;
            }

            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();
            float radius = helper.GetModifiedStat(1, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, hit, index);
            bool appliedToAtLeastOneEntity = false;


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

            Vector2 tilePos = new Vector2(hit.GetLocation().X - radius, hit.GetLocation().Y - radius);
            Vector2 absoluteTilePos = Utils.TilePosToAbsolutePos(tilePos);

            Rectangle rectangle = new Rectangle((int)absoluteTilePos.X, (int)absoluteTilePos.Y, boundingBoxRadius, boundingBoxRadius);

            SetBoundingBox(rectangle);

            foreach (Character e in GameLocationUtils.GetCharacters(caster, rectangle))
            {
                if (helper.Invoke(modEntry, spell, caster, gameLocation, new CharacterHitResult(e), ticksUsed, index, awardXp) == new SpellCastResult(SpellCastResultType.SUCCESS))
                {
                    appliedToAtLeastOneEntity = true;
                }
            }

            if (appliedToAtLeastOneEntity)
            {
                spellCastResult = new SpellCastResult(SpellCastResultType.SUCCESS);
                return spellCastResult;
            }

            TilePos pos  = new TilePos(hit.GetLocation());

            int rad = (int)radius;

            for (int x = (int)(pos.GetVector().X - rad); x <= pos.GetVector().X + rad; ++x)
            {
                for (int y = (int)(pos.GetVector().Y - rad); y <= pos.GetVector().Y + rad; ++y)
                {
                    if (hit.GetHitResultType() == HitResult.HitResultType.TERRAIN_FEATURE)
                    {
                        TilePos newTilePos = new TilePos(x, y);

                        helper.Invoke(modEntry, spell, caster, gameLocation, new TerrainFeatureHitResult(hit.GetLocation(), ((TerrainFeatureHitResult)hit).GetDirection(), newTilePos, ((TerrainFeatureHitResult)hit).IsInside()), ticksUsed, index, awardXp);
                    }
                }
            }

            Texture2D aoeTexture = modEntry.Helper.ModContent.Load<Texture2D>("assets/aoe/aoe.png");
            Vector2 local = new Vector2(boundingBox.X, boundingBox.Y);

            float speed = -3f;

            Rectangle imageSourceRect = new Rectangle(0, 0, 16, 16);
            var size = ExpandToBound(new Rectangle((int)0, (int)0, imageSourceRect.Width, imageSourceRect.Height), boundingBox);

            TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite()
            {
                initialParentTileIndex = 0,
                interval = 40f,
                totalNumberOfLoops = 50,
                position = local,
                animationLength = 6,
                flicker = false,
                flipped = false,
                texture = aoeTexture,
                sourceRect = imageSourceRect,
                sourceRectStartingPos = new Vector2(imageSourceRect.X, imageSourceRect.Y),
                initialPosition = local,
                alphaFade = (float)(1.0 / 1000.0 - (double)speed / 300.0),
                alpha = 1f,
                //motion = new Vector2(0.0f, speed),
                //acceleration = new Vector2(0.0f, 0.0f),
                layerDepth = (float)(boundingBox.Bottom - 3 - Game1.random.Next(5)) / 10000f,
                scale = (float)size,
                scaleChange = 0.01f,
                //rotationChange = (float)((double)Game1.random.Next(-5, 6) * 3.1415927410125732 / 256.0),
                color = Color.White
            };

            //Rectangle imageSourceRect = new Rectangle(372, 1956, 10, 10);
            //var size = ExpandToBound(new Rectangle((int)0, (int)0, imageSourceRect.Width, imageSourceRect.Height), boundingBox);

            //TemporaryAnimatedSprite sprite = new TemporaryAnimatedSprite("LooseSprites\\Cursors", imageSourceRect, local, false, 1f / 500f, new Color(0, 48, 255, 127))
            //{
            //    alphaFade = (float)(1.0 / 1000.0 - (double)speed / 300.0),
            //    alpha = 0.5f,
            //    motion = new Vector2(0.0f, speed),
            //    //acceleration = new Vector2(0.0f, 0.0f),
            //    interval = 99999f,
            //    layerDepth = (float)(boundingBox.Bottom - 3 - Game1.random.Next(5)) / 10000f,
            //    //scale = 8f,
            //    scale = (float)size,
            //    scaleChange = 0.01f,
            //    rotationChange = (float)((double)Game1.random.Next(-5, 6) * 3.1415927410125732 / 256.0)
            //};

            Game1.Multiplayer.broadcastSprites(gameLocation, sprite);

            spellCastResult = new SpellCastResult(SpellCastResultType.SUCCESS);

            return spellCastResult;
        }

        public void SetBoundingBox(Rectangle boundingBox)
        {
            this.boundingBox = boundingBox;
        }

        public Rectangle GetBoundingBox()
        {
            return boundingBox;
        }

        public SpellCastResult GetCastResult()
        {
            return spellCastResult;
        }

        public override bool IsEndShape()
        {
            return true;
        }

        public override bool NeedsPrecedingShape()
        {
            return true;
        }

        public override float ManaCost()
        {
            return 2;
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
