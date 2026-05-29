using ArsVenefici.Framework.API.affinity;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.ability
{
    public record Ability(string id, Affinity affinity, double minBound, double maxBound)
    {
        public static string ABILITY = "ability";

        public bool Test(Farmer player)
        {
            //return bounds().matches(ArsMagicaAPI.get().getAffinityHelper().getAffinityDepth(player, affinity()));

            var api = ModEntry.INSTANCE.arsVeneficiAPILoader.GetAPI();
            var affinityHelper = api.GetAffinityHelper();

            double depth = affinityHelper.GetAffinityDepth(player, affinity);

            if (depth >= minBound && depth <= maxBound)
            {
                return true;
            }
            else
            {
                 return false;
            }
        }

        public string GetId()
        {
            return id;
        }
    }
}
