using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.FarmerPlayer
{
    public class FarmerExtData
    {
        public static ConditionalWeakTable<Farmer, FarmerExtData> data = new();

        public float HealthRegen { get; set; } = 0;
        public float StaminaRegen { get; set; } = 0;

        public float ManaRegen { get; set; } = 0;

        public float healthBuffer { get; set; } = 0;
        public float staminaBuffer { get; set; } = 0;
        public float manaBuffer { get; set; } = 0;
    }
}
