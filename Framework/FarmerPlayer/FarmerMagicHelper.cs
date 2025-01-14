using ArsVenefici.Framework.GUI;
using ArsVenefici.Framework.Spell;
using ArsVenefici.Framework.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using StardewModdingAPI;
using StardewValley;
using SpaceCore;
using ArsVenefici.Framework.Skill;
using SpaceShared.APIs;
using SpaceCore.Events;
using ArsVenefici.Framework.Spell.Effects;
using ArsVenefici.Framework.Commands;
using ArsVenefici.Framework.GameSave;
using ArsVenefici.Framework.Events;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Interfaces;
using Netcode;
using StardewValley.Network;
using System.Runtime.CompilerServices;
using System;
using ArsVenefici.Framework.Spell.Buffs;

namespace ArsVenefici.Framework.FarmerPlayer
{
    public class FarmerMagicHelper
    {
        ModEntry modEntry;

        /// <remarks>This should only be accessed through <see cref="GetSpellBook"/> or <see cref="Extensions.GetSpellBook"/> to make sure an updated instance is retrieved.</remarks>
        private static readonly IDictionary<long, SpellBook> SpellBookCache = new Dictionary<long, SpellBook>();

        /// <summary>The ID of the event in which the player learns magic from the Wizard.</summary>
        //public static int LearnedMagicEventId { get; } = 90002;
        public int LearnedWizardryEventId { get; } = 9918172;

        /// <summary>Whether the current player learned wizardry.</summary>
        public bool LearnedWizardy => Game1.player?.eventsSeen?.Contains(LearnedWizardryEventId.ToString()) == true ? true : false;

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
            // skip if player hasn't learned wizardry
            if (!LearnedWizardy && overrideWizardryLevel is not > 0)
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

            if (player.GetMaxMana() < expectedPoints)
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
