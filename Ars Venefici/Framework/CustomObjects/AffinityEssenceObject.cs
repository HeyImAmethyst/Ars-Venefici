using ArsVenefici.Framework.API.affinity;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.CustomObjects
{
    public class AffinityEssenceObject : StardewValley.Object, IAffinityObject
    {
        public AffinityEssenceObject(string id)
        {
            string itemID = ValidateUnqualifiedItemId(id);
            ItemId = itemID;

            ResetParentSheetIndex();
            ParentSheetIndex = 0;
        }

        public override string checkForSpecialItemHoldUpMeessage()
        {
            if(ItemId.StartsWith("HeyImAmethyst.ArsVenefici_") && ItemId.EndsWith("_Essence"))
            {
                return ModEntry.INSTANCE.Helper.Translation.Get("essence.acquire_first_time.message");
            }
            else
            {
                return base.checkForSpecialItemHoldUpMeessage();
            }
        }
    }
}
