using ArsVenefici.Framework.GameSave;
using StardewModdingAPI.Events;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Network;
using static ArsVenefici.ModConfig;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.API;
using ArsVenefici.Framework.Spells.Registry;

namespace ArsVenefici.Framework.Events
{
    public class MultiplayerEvents
    {
        ModEntry modEntryInstance;

        public MultiplayerEvents(ModEntry modEntry)
        {
            modEntryInstance = modEntry;
        }

        public void OnPeerConnected(object sender, PeerConnectedEventArgs e)
        {
            if (!Game1.IsMasterGame)
                return;

            modEntryInstance.Helper.Multiplayer.SendMessage(
                new ModSaveDataEntryMessage(modEntryInstance.ModSaveData),
                ModEntry.SAVEDATA, modIDs: new[] { modEntryInstance.ModManifest.UniqueID });
        }

        /// <summary>
        /// Receive saved data sent from host
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnModMessageReceived(object sender, ModMessageReceivedEventArgs e)
        {
            if (e.FromModID == modEntryInstance.ModManifest.UniqueID)
            {
                switch (e.Type)
                {
                    // entire saveData
                    case ModEntry.SAVEDATA:
                        try
                        {
                            modEntryInstance.ModSaveData = e.ReadAs<ModSaveData>();
                            modEntryInstance.farmerMagicHelper.FixManaPoolIfNeeded(Game1.player);

                        }
                        catch (InvalidOperationException)
                        {
                            modEntryInstance.Monitor.Log($"Failed to read save data sent by host.", LogLevel.Warn);
                            modEntryInstance.ModSaveData = null;
                        }
                        break;
                }
            }
        }

        public void OnNetworkCast(IncomingMessage msg)
        {
            var api = modEntryInstance.arsVeneficiAPILoader.GetAPI();
            Farmer player = Game1.getFarmer(msg.FarmerID);

            if (player == null)
                return;

            var spellHelper = api.GetSpellHelper();

            spellHelper.CastSpell(player, modEntryInstance.buttonEvents.modKeyBinds);
        }
    }
}
