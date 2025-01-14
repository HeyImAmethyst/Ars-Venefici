using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Spell.Effects;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ArsVenefici.Framework.Spell.Shape
{
    public class Wall : AbstractShape
    {
        public Wall() : base(new SpellPartStats(SpellPartStatType.DURATION), new SpellPartStats(SpellPartStatType.RANGE))
        {

        }

        public override string GetId()
        {
            return "wall";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, HitResult hit, int ticksUsed, int index, bool awardXp)
        {
            var helper = SpellHelper.Instance();

            Vector2 position = Vector2.One;

            if (hit != null)
            {
                //position = Utils.ConvertToTilePos(Utility.clampToTile(hit.GetLocation()));
                position = hit.GetLocation();
            }
            else
            {
                position = Utils.AbsolutePosToTilePos(Utility.clampToTile(caster.GetPosition()));
            }

            float radius = helper.GetModifiedStat(1, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, hit, index);
            int duration = (int)(200 + helper.GetModifiedStat(100, new SpellPartStats(SpellPartStatType.DURATION), modifiers, spell, caster, hit, index));

            Farmer farmer = caster.entity as Farmer;

            WallEffect wallEffect = new WallEffect(modEntry, spell, position, radius, duration, farmer.FacingDirection);
            wallEffect.SetIndex(index);
            wallEffect.SetOwner(caster);

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

            if (farmer.FacingDirection == 1 || farmer.FacingDirection == 3)
            {
                Vector2 tilePos = new Vector2(wallEffect.GetPosition().X, wallEffect.GetPosition().Y - radius);
                Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);

                wallEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), 1 * Game1.tileSize, (int)(boundingBoxRadius)));
            }

            if (farmer.FacingDirection == 0 || farmer.FacingDirection == 2)
            {
                Vector2 tilePos = new Vector2(wallEffect.GetPosition().X - radius, wallEffect.GetPosition().Y);
                Vector2 absolutePos = Utils.TilePosToAbsolutePos(tilePos);

                wallEffect.SetBoundingBox(new Rectangle((int)(absolutePos.X), (int)(absolutePos.Y), boundingBoxRadius, 1 * Game1.tileSize));
            }

            modEntry.ActiveEffects.Add(wallEffect);

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override bool NeedsPrecedingShape()
        {
            return true;
        }

        public override bool IsEndShape()
        {
            return true;
        }

        public override float ManaCost()
        {
            return 2.5f;
        }
    }
}
