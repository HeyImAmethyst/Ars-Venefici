using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Objects;
using Microsoft.Xna.Framework;
using ArsVenefici.Framework.Spells.Effects;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Spells.Components
{
    public class Summon : AbstractComponent
    {
        public Summon() : base()
        {

        }

        public override string GetId()
        {
            return "summon";
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, CharacterHitResult target, int index, int ticksUsed)
        {
            return new SpellCastResult(SpellCastResultType.EFFECT_FAILED);
        }

        public override SpellCastResult Invoke(ModEntry modEntry, ISpell spell, IEntity caster, GameLocation gameLocation, List<ISpellModifier> modifiers, TerrainFeatureHitResult target, int index, int ticksUsed)
        {
            var helper = modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper();

            Vector2 position = Vector2.One;

            if (target != null)
            {
                //position = Utils.ConvertToTilePos(Utility.clampToTile(hit.GetLocation()));
                position = target.GetLocation();
            }
            else
            {
                position = Utils.AbsolutePosToTilePos(Utility.clampToTile(caster.GetPosition()));
            }

            int duration = (int)(60 * 60 + helper.GetModifiedStat(100, new SpellPartStats(SpellPartStatType.DURATION), modifiers, spell, caster, target, index));

            SummonEffect summonEffect = new SummonEffect(modEntry, caster, spell, position, duration);
            summonEffect.SetOwner(caster);

            Farmer farmer = caster.entity as Farmer;
            farmer.playNearbySoundLocal("powerup");

            modEntry.ActiveEffects.Add(summonEffect);

            return new SpellCastResult(SpellCastResultType.SUCCESS);
        }

        public override float ManaCost()
        {
            return 50;
        }
    }
}
