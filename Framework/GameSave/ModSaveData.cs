﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GameSave
{

    //public sealed record ModSaveDataEntry(ImmutableHashSet<RuleIdent> Rules, ImmutableHashSet<string> Inputs, bool[] Quality);

    public sealed record ModSaveDataEntryMessage(ModSaveData? Entry);

    public sealed class ModSaveData
    {
        /// <summary>The number of mana points gained per magic level.</summary>
        public int ManaPointsPerLevel { get; set; } = 200;
        public int ManaRegenRate { get; set; } = 2;

        public bool InfiniteMana { get; set; } = false;

        public bool EnableGrowSickness { get; set; } = true;
        public int GrowSicknessDurationMillisecondsLessThanLevelSix { get; set; } = 126000;
        public int GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelSix { get; set; } = 84000;
        public int GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelEight { get; set; } = 42000;
        public int GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelTen { get; set; } = 21000;

        public void ResetValues()
        {
            ManaPointsPerLevel = 200;
            ManaRegenRate = 2;
            InfiniteMana = false;

            EnableGrowSickness = false;
            GrowSicknessDurationMillisecondsLessThanLevelSix = 126000;
            GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelSix = 84000;
            GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelEight = 42000;
            GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelTen = 21000;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Save Data");
            sb.AppendLine("Mana Points PerLevel: " + ManaPointsPerLevel);
            sb.AppendLine("Mana Regen Rate: " + ManaRegenRate);
            sb.AppendLine("Infinite Mana: " + InfiniteMana);
            sb.AppendLine("Enable Grow Sickness: " + EnableGrowSickness);
            sb.AppendLine("Grow Sickness Duration In Milliseconds Less Than Level Six: " + GrowSicknessDurationMillisecondsLessThanLevelSix);
            sb.AppendLine("Grow Sickness Duration In Milliseconds Greater Than Or Equal To Level Six: " + GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelSix);
            sb.AppendLine("Grow Sickness Duration In Milliseconds Greater Than Or Equal To Level Eight: " + GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelEight);
            sb.AppendLine("Grow Sickness Duration In Milliseconds Greater Than Or Equal To Level Ten: " + GrowSicknessDurationMillisecondsGreaterThanOrEqualToLevelTen);

            return sb.ToString();
        }
    }
}
