using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.affinity
{
    public interface IAffinityObject
    {


        /// <summary>
        /// 
        /// </summary>
        /// <returns> Whether this item should have a variant without an affinity or not.</returns>
        internal bool HasNoneVariant()
        {
            return false;
        }

        internal Affinity GetAffinity(StardewValley.Object stardewValleyObject)
        {
            //var registry = ArsMagicaAPI.get().getAffinityRegistry();

            ObjectRegister<Affinity> affinities = Affinities.AFFINITIES;
            List<ObjectHolder<Affinity>> affinityList = affinities.GetObjectList();

            //ResourceLocation key = ResourceLocation.tryParse(stack.getOrCreateTag().getString(registry.key().location().ToString()));

            //Affinity affinity = Affinity.CreateBuilder().Build();
            //Affinity affinity = Affinities.NONE.Get();

            if(stardewValleyObject != null)
            {
                //if (affinity.GetId() == string.Empty)
                //{
                //    affinity = Affinities.NONE.Get();
                //    return affinity;
                //}
                //else
                //{
                //    foreach (var affinityObjectHolder in affinityList)
                //    {
                //        Affinity aff = affinityObjectHolder.Get();

                //        if (stardewValleyObject.ItemId.Contains(aff.id))
                //        {
                //            affinity = aff;
                //            break;
                //        }
                //    }

                //    return affinity;
                //}
                Affinity aff = Affinities.NONE.Get();

                foreach (var affinityObjectHolder in affinityList)
                {
                    if (stardewValleyObject.ItemId.ToLower().Contains(affinityObjectHolder.Get().id))
                    {
                        aff = affinityObjectHolder.Get();
                    }
                }

                return aff;

            }
            else
            {
                return Affinities.NONE.Get();
            }
            
        }

        //internal Item SetAffinity(Item stack, Affinity affinity);
        //{
        //    stack.getOrCreateTag().putString(ArsMagicaAPI.get().getAffinityRegistry().key().location().ToString(), affinity.getId().ToString());
        //    return stack;
        //}

    }
}
