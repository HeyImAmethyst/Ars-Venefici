using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Explosion : AbstractComponent
    {
        public Explosion() : base(new SpellPartStats(SpellPartStatType.RANGE)) 
        { 

        }

        public override string GetId()
        {
            return "explosion";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            var helper = SpellHelper.Instance();
            int radius = (int)helper.GetModifiedStat(2, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, target, index);

            gameLocation.explode(Utils.ConvertToTilePos(Utility.clampToTile(target.GetCharacter().getStandingPosition())), radius, caster.entity as Farmer);

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            var helper = SpellHelper.Instance();
            int radius = (int)helper.GetModifiedStat(2, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, target, index);

            gameLocation.explode(target.GetTilePos().GetVector(), radius, caster.entity as Farmer);
            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override int ManaCost()
        {
            return 25;
        }
    }
}
