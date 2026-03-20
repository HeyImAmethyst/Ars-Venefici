using ArsVenefici.Framework.Affinity;
using ArsVenefici.Framework.API.Magic;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.Spells;
using Microsoft.Xna.Framework;
using SpaceCore;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Magic
{
    public class MagicHelper : IMagicHelper
    {
        private static MagicHelper INSTANCE = new MagicHelper();

        /// <summary>The ID of the event in which the player learns magic from the Wizard.</summary>
        //public static int LearnedMagicEventId { get; } = 90002;
        int LearnedWizardryEventId { get; } = 9918172;

        public static MagicHelper Instance()
        {
            return INSTANCE;
        }

        public void AwardXp(Farmer farmer, int amount)
        {
            farmer.AddCustomSkillExperience(FarmerMagicHelper.Skill, amount);
        }

        public int GetLearnedWizardryEventId()
        {
            return LearnedWizardryEventId;
        }

        public bool LearnedWizardy(Farmer farmer)
        {
            return Game1.player?.eventsSeen?.Contains(LearnedWizardryEventId.ToString()) == true ? true : false;
        }

        public Color GetColorForMagicType(ISpell spell)
        {
            Color color = Color.White;

            if (spell.spellStack() != null)
            {
                MagicType magicType = spell.GetMagicType();

                switch (magicType)
                {
                    case MagicType.None:
                        color = Color.White;
                        return color;
                    case MagicType.Earth:
                        color = Color.Peru;
                        return color;
                    case MagicType.Water:
                        color = new Color(159, 185, 255, 230);
                        return color;
                    case MagicType.Air:
                        color = Color.White;
                        return color;
                    case MagicType.Fire:
                        color = new Color(255, 133, 0, 230);
                        return color;
                    case MagicType.Nature:
                        color = new Color(137, 252, 80, 230);
                        return color;
                    case MagicType.Ice:
                        color = new Color(201, 238, 255, 230);
                        return color;
                    case MagicType.Lightning:
                        color = Color.PaleGoldenrod;
                        return color;
                    case MagicType.Life:
                        color = new Color(255, 45, 104, 230);
                        return color;
                    case MagicType.Arcane:
                        color = new Color(0, 248, 255, 230);
                        return color;
                    case MagicType.Darkness:
                        color = new Color(103, 51, 125, 230);
                        return color;
                    default:
                        color = Color.White;
                        return color;
                }
            }

            return color;
        }


    }
}
