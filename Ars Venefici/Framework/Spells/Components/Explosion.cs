using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Util;
using StardewValley;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.API.affinity;

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

        public override HashSet<Affinity> GetAffinities()
        {
            return new HashSet<Affinity> { Affinities.FIRE.Get() };
        }

        public override Dictionary<Affinity, float> GetAffinityShifts()
        {
            return new Dictionary<Affinity, float> { { Affinities.FIRE.Get(), 0.002f } };
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            var api = modEntry.arsVeneficiAPILoader.GetAPI();
            var helper = api.GetSpellHelper();

            int radius = (int)helper.GetModifiedStat(2, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, target, index);

            gameLocation.explode(Utils.AbsolutePosToTilePos(Utility.clampToTile(target.GetCharacter().getStandingPosition())), radius, caster.entity as Farmer);

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            var api = modEntry.arsVeneficiAPILoader.GetAPI();
            var helper = api.GetSpellHelper();

            int radius = (int)helper.GetModifiedStat(2, new SpellPartStats(SpellPartStatType.RANGE), modifiers, spell, caster, target, index);

            gameLocation.explode(target.GetTilePos().GetVector(), radius, caster.entity as Farmer);
            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override float ManaCost()
        {
            return 45;
        }
    }
}
