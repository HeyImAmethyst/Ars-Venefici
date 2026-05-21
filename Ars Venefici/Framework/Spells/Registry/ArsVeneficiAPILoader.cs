using ArsVenefici.Framework.API;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Registry
{
    /// <summary>
    /// A loader for storing the mod's api. Can be used to set custom api objects to the mod
    /// </summary>
    public class ArsVeneficiAPILoader
    {
        private ArsVeneficiAPI API;

        public ArsVeneficiAPILoader()
        {

        }

        /// <summary>
        /// Sets the mod's api
        /// </summary>
        /// <param name="api">The api object</param>
        public void SetAPI(ArsVeneficiAPI api)
        {
            //ModEntry.INSTANCE.Monitor.Log("Open Spell Book", LogLevel.Info);
            API = api;
        }

        /// <summary>
        /// Gets the mod's api
        /// </summary>
        /// <returns>An ArsVeneficiAPI object</returns>
        public ArsVeneficiAPI GetAPI()
        {
            return API;
        }
    }
}
