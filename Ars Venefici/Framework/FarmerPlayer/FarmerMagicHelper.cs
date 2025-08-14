using ArsVenefici.Framework.GUI;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewModdingAPI;
using StardewValley;
using SpaceCore;
using ArsVenefici.Framework.Skill;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.FarmerPlayer
{
    public class FarmerMagicHelper
    {
        ModEntry modEntry;

        /// <remarks>This should only be accessed through <see cref="GetSpellBook"/> or <see cref="Extensions.GetSpellBook"/> to make sure an updated instance is retrieved.</remarks>
        private static readonly IDictionary<long, SpellBook> SpellBookCache = new Dictionary<long, SpellBook>();

        public static ArsVeneficiSkill Skill;

        public static bool SpellCastingMode = true;

        public FarmerMagicHelper(ModEntry modEntry)
        {
            this.modEntry = modEntry;
        }

        /// <summary>Fix the player's mana pool to match their skill level if needed.</summary>
        /// <param name="player">The player to fix.</param>
        /// <param name="overrideWizardryLevel">The wizardry skill level, or <c>null</c> to get it from the player.</param>
        public void FixManaPoolIfNeeded(Farmer player, int? overrideWizardryLevel = null)
        {
            var api = modEntry.arsVeneficiAPILoader.GetAPI();
            var magicHelper = api.GetMagicHelper();

            // skip if player hasn't learned wizardry
            if (!magicHelper.LearnedWizardy(player) && overrideWizardryLevel is not > 0)
                return;

            // get wizardry info
            int wizardryLevel = overrideWizardryLevel ?? player.GetCustomSkillLevel(Skill);

            SpellBook spellBook = Game1.player.GetSpellBook();

            // fix mana pool

            //if(LearnedWizardy)
            //{
            //    int expectedPoints = wizardryLevel * ManaPointsPerLevel;

            //    if (player.GetMaxMana() < expectedPoints)
            //    {
            //        player.SetMaxMana(expectedPoints);
            //        player.AddMana(expectedPoints);
            //    }
            //}

            int expectedPoints = wizardryLevel * modEntry.ModSaveData.ManaPointsPerLevel;

            if (player.GetMaxMana() != expectedPoints)
            {
                player.SetMaxMana(expectedPoints);
                player.AddMana(expectedPoints);
            }
        }

        /// <summary>Get a self-updating view of a player's magic metadata.</summary>
        /// <param name="player">The player whose spell book to get.</param>
        public static SpellBook GetSpellBook(Farmer player)
        {
            if (!SpellBookCache.TryGetValue(player.UniqueMultiplayerID, out SpellBook book) || !object.ReferenceEquals(player, book.Player))
                SpellBookCache[player.UniqueMultiplayerID] = book = new SpellBook(player);

            return book;
        }
    }
}
