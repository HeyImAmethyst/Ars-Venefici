using ArsVenefici.Framework.Interfaces.Magic;
using ArsVenefici.Framework.Magic;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Events
{
    public class CharacterEvents
    {
        public event EventHandler<CharacterDamageEventArgs> CharacterDamage;
        public event EventHandler<CharacterHealEventArgs> CharacterHeal;

        public virtual void InvokeOnCharacterDamage(Character character)
        {
            CharacterDamage?.Invoke(this, new CharacterDamageEventArgs(character));
        }

        public virtual void InvokeOnCharacterHeal(Character character)
        {
            CharacterHeal?.Invoke(this, new CharacterHealEventArgs(character));
        }

        public void OnCharacterDamage(object sender, CharacterDamageEventArgs e)
        {
            var contingencyHelper = ContingencyHelper.Instance();

            if (e.character.modData.ContainsKey(ContingencyHelper.ContingencyKey))
            {
                ContingencyType type = contingencyHelper.GetContingencyType(e.character);

                if (type == ContingencyType.DAMAGE)
                    contingencyHelper.TriggerContingency(e.character, ContingencyType.DAMAGE);
            }
        }

        public void OnCharacterHeal(object sender, CharacterHealEventArgs e)
        {
            var contingencyHelper = ContingencyHelper.Instance();

            if (e.character.modData.ContainsKey(ContingencyHelper.ContingencyKey))
            {
                ContingencyType type = contingencyHelper.GetContingencyType(e.character);

                if (type == ContingencyType.HEALTH)
                    contingencyHelper.TriggerContingency(e.character, ContingencyType.HEALTH);
            }
        }
    }
}
