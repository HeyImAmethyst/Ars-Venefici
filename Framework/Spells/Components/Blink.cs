using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Tiles;
using static StardewValley.Minigames.TargetGame;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Blink : AbstractComponent
    {
        public Blink() : base(new SpellPartStats(SpellPartStatType.RANGE))
        {

        }

        public override string GetId()
        {
            return "blink";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            var helper = SpellHelper.Instance();
            int radius = (int)helper.GetModifiedStat(5, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, target, index) * 4;

            Farmer entity = caster.entity as Farmer;
            
            entity.position.X = target.GetTilePos().GetTilePosX() * 64 - entity.GetBoundingBox().Width / 2;
            entity.position.Y = target.GetTilePos().GetTilePosY() * 64 - entity.GetBoundingBox().Height / 2;
            entity.playNearbySoundLocal("powerup");

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override int ManaCost()
        {
            return 45;
        }
    }
}
