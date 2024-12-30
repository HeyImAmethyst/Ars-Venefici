using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spell.Effects;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Network;
using StardewValley.Objects;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.TargetGame;

namespace ArsVenefici.Framework.Spell.Shape
{
    public class AoE : AbstractShape
    {
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
                return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);

            var helper = SpellHelper.Instance();
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

            foreach (Character e in GameLocationUtils.GetCharacters(caster, rectangle))
            {
                if (helper.Invoke(modEntry, spell, caster, gameLocation, new CharacterHitResult(e), ticksUsed, index, awardXp) == new SpellCastResult(SpellCastResultType.SUCCESS))
                {
                    appliedToAtLeastOneEntity = true;
                }
            }

            if (appliedToAtLeastOneEntity) 
                return new SpellCastResult(SpellCastResultType.SUCCESS);

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

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override bool IsEndShape()
        {
            return true;
        }

        public override bool NeedsPrecedingShape()
        {
            return true;
        }

        public override int ManaCost()
        {
            return 2;
        }
    }
}
