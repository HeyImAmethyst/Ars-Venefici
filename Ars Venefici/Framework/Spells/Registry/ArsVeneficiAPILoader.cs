using ArsVenefici.Framework.API;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.Registry
{
    public class ArsVeneficiAPILoader
    {
        private ArsVeneficiAPI API;

        public ArsVeneficiAPILoader()
        {

        }

        public void SetAPI(ArsVeneficiAPI api)
        {
            //ModEntry.INSTANCE.Monitor.Log("Open Spell Book", LogLevel.Info);
            API = api;
        }

        public ArsVeneficiAPI GetAPI()
        {
            return API;
        }
    }
}
