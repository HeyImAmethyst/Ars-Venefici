using ArsVenefici.Framework.Patches;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici
{
    public class HarmonyHelper
    {
        // Instance of Harmony
        private Harmony harmony;

        // The mods entry
        private ModEntry modEntry;
        public CharacterPatch farmerContingencyPatch;

        /// <summary>
        /// Constructor - Used for all Harmony related patching.
        /// </summary>
        /// <param name="entry">The Mod's Entry class.</param>
        public HarmonyHelper(ModEntry entry)
        {
            modEntry = entry;
            farmerContingencyPatch = new CharacterPatch(modEntry);
        }

        /// <summary>
        /// Initializes the Harmony Instance and starts the patches.
        /// </summary>
        public void InitializeAndPatch()
        {
            harmony = new Harmony(modEntry.ModManifest.UniqueID);
            PatchWithHarmony();
        }

        /// <summary>
        /// Harmony patch for accessory length and skin color length.
        /// </summary>
        private void PatchWithHarmony()
        {
            // Patch the skin color length and skin colors in the save menu
            PatchFarmer();
        }

        /// <summary>
        /// Patches changeSkinColor and SaveFileSlot using a harmony patches.
        /// </summary>
        private void PatchFarmer()
        {
            farmerContingencyPatch.Apply(harmony);
        }
    }
}
