using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.API.Skill;
using ArsVenefici.Framework.CustomObjects;
using ArsVenefici.Framework.Magic;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Util;
using ItemExtensions;
using SpaceShared;
using StardewValley;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spells.affinity
{
    //public class AffinityData
    //{
    //    public string id { get; set; }

    //    public double affinityDepth { get; set; }
    //}

    public class AffinityHelper : IAffinityHelper
    {
        public static float MAX_DEPTH = 1F;

        private static float ADJACENT_FACTOR = 0.25f;
        private static float MINOR_OPPOSING_FACTOR = 0.5f;
        private static float MAJOR_OPPOSING_FACTOR = 0.75f;

        // <summary>The prefix added to mod data keys.</summary>
        private const string Prefix = "HeyImAmethyst.ArsVenifici";

        /// <summary>The data key for the player's affinities.</summary>
        private const string AffinitiesKey = Prefix + "/Affinities";

        /// <summary>The player's affinity depths.</summary>
        //public IDictionary<string, AffinityData> AffinityDatas { get; set; }
        public AffinityHolder AFFINITY_HOLDER { get; set; }

        private static AffinityHelper INSTANCE = new AffinityHelper();

        public static AffinityHelper Instance()
        {
            return INSTANCE;
        }

        private AffinityHelper() 
        {
            Dictionary<string, double> depths = new Dictionary<string, double>();

            depths.Add(Affinities.EARTH.Get().GetId(), 0);
            depths.Add(Affinities.WATER.Get().GetId(), 0);
            depths.Add(Affinities.AIR.Get().GetId(), 0);
            depths.Add(Affinities.FIRE.Get().GetId(), 0);
            depths.Add(Affinities.NATURE.Get().GetId(), 0);
            depths.Add(Affinities.ICE.Get().GetId(), 0);
            depths.Add(Affinities.LIGHTNING.Get().GetId(), 0);
            depths.Add(Affinities.LIFE.Get().GetId(), 0);
            depths.Add(Affinities.ARCANE.Get().GetId(), 0);
            depths.Add(Affinities.DARKNESS.Get().GetId(), 0);

            AFFINITY_HOLDER = new AffinityHolder(depths, false);

            //AffinityDatas.Add(Affinities.DARKNESS.Get().GetId(), new AffinityData
            //{
            //    id = Affinities.DARKNESS.Get().GetId(),
            //    affinityDepth = 0
            //});
        }

        public Item GetEssenceForAffinity(string affinity)
        {
            //string e = "HeyImAmethyst.ArsVenefici_Lightning_Tome";

            AffinityEssenceObject essence = new AffinityEssenceObject("HeyImAmethyst.ArsVenefici_" + affinity.FirstCharToUpper() + "_Essence");
            //AffinityEssenceObject essence = new AffinityEssenceObject("HeyImAmethyst.ArsVenefici_Earth_Essence");

            essence.Category = -8;
            essence.Type = "Crafting";

            return GetStackForAffinity(essence, affinity);
        }

        public Item GetEssenceForAffinity(Affinity affinity)
        {
            return GetEssenceForAffinity(affinity.GetId());
        }

        public Item GetTomeForAffinity(string affinity)
        {
            AffinityTomeObject essenceTome = new AffinityTomeObject("HeyImAmethyst.ArsVenefici_" + affinity.FirstCharToUpper() + "_Tome");

            essenceTome.Category = -103;
            essenceTome.Type = "asdf";

            return GetStackForAffinity(essenceTome, affinity);
        }

        public Item GetTomeForAffinity(Affinity affinity)
        {
            return GetTomeForAffinity(affinity.GetId());
        }

        public Item GetStackForAffinity<T>(T stardewValleyObject, string affinity) where T : StardewValley.Object, IAffinityObject
        {
            //Item stack = ItemRegistry.Create(stardewValleyObject.ItemId);
            //Optional.ofNullable(ArsMagicaAPI.get().getAffinityRegistry().get(aff)).ifPresent(affinity->item.setAffinity(stack, affinity));
            //return stack;
            stardewValleyObject.Stack = 1;
            return stardewValleyObject;
        }

        public Item GetStackForAffinity<T>(T stardewValleyObject, Affinity affinity) where T : StardewValley.Object, IAffinityObject
        {
            return GetStackForAffinity(stardewValleyObject, affinity.GetId());
        }

        public Affinity GetAffinityForItem(StardewValley.Object item)
        {
            //if (stack.getItem() instanceof IAffinityItem item) return item.getAffinity(stack);
            //return Objects.requireNonNull(ArsMagicaAPI.get().getAffinityRegistry().get(Affinity.NONE));

            if(item is IAffinityObject)
            {
                IAffinityObject affinityObject = (IAffinityObject)item;

                return affinityObject.GetAffinity(item);
            }

            return Affinities.NONE.Get();
        }

        public double GetAffinityDepth(Farmer farmer, string affinity)
        {
            //AffinityDatas.TryGetValue(affinity, out AffinityData affinityData);
            //return affinityData.affinityDepth;

            return AFFINITY_HOLDER.GetAffinityDepth(affinity);
        }

        public double GetAffinityDepth(Farmer farmer, Affinity affinity)
        {
            //AffinityDatas.TryGetValue(affinity.GetId(), out AffinityData affinityData);
            //return affinityData.affinityDepth;

            return AFFINITY_HOLDER.GetAffinityDepth(affinity.GetId());
        }

        public double GetAffinityDepthOrElse(Farmer farmer, Affinity affinity, double defaultValue)
        {
            //AffinityDatas.TryGetValue(affinity.GetId(), out AffinityData affinityData);
            //return affinityData != null ? affinityData.affinityDepth : defaultValue;

            return farmer.health == 0 ? defaultValue : GetAffinityDepth(farmer, affinity);
        }

        public double GetAffinityDepthOrElse(Farmer farmer, string affinity, double defaultValue)
        {
        //AffinityDatas.TryGetValue(affinity, out AffinityData affinityData);
        //return affinityData != null ? affinityData.affinityDepth : defaultValue;

            return GetAffinityDepthOrElse(farmer, affinity, defaultValue);
        }

        public void SetAffinityDepth(Farmer farmer, string affinity, float amount)
        {
            //AffinityData affinityData = new AffinityData()
            //{
            //    id = affinity,
            //    affinityDepth = amount
            //};


            //AffinityDatas.TryGetValue(affinity, out AffinityData affinityData);

            //affinityData.affinityDepth = amount;

            //AffinityDatas.CreateNewOrUpdateExisting(affinity, affinityData);

            AFFINITY_HOLDER.SetAffinity(affinity, amount);
            SyncToPlayer(farmer);
            UpdateIfNeeded(ModEntry.INSTANCE, farmer);
        }

        public void SetAffinityDepth(Farmer farmer, Affinity affinity, float amount)
        {
            //AffinityData affinityData = new AffinityData()
            //{
            //    id = affinity.GetId(),
            //    affinityDepth = amount
            //};

            //AffinityDatas.TryGetValue(affinity.GetId(), out AffinityData affinityData);

            //affinityData.affinityDepth = amount;

            //AffinityDatas.CreateNewOrUpdateExisting(affinity.GetId(), affinityData);

            AFFINITY_HOLDER.SetAffinity(affinity.GetId(), amount);
            SyncToPlayer(farmer);
            UpdateIfNeeded(ModEntry.INSTANCE, farmer);
        }

        public void IncreaseAffinityDepth(Farmer farmer, string affinity, float amount)
        {
            //AffinityDatas.TryGetValue(affinity, out AffinityData affinityData);

            //affinityData.affinityDepth += amount;

            //AffinityDatas.CreateNewOrUpdateExisting(affinity, affinityData);

            AFFINITY_HOLDER.AddToAffinity(affinity, amount);
            SyncToPlayer(farmer);
            UpdateIfNeeded(ModEntry.INSTANCE, farmer);
        }

        public void IncreaseAffinityDepth(Farmer farmer, Affinity affinity, float amount)
        {
            //AffinityData affinityData = new AffinityData()
            //{
            //    id = affinity.GetId(),
            //    affinityDepth = affinityDepth + amount
            //};

            //AffinityDatas.TryGetValue(affinity.GetId(), out AffinityData affinityData);

            //affinityData.affinityDepth += amount;

            //AffinityDatas.CreateNewOrUpdateExisting(affinity.GetId(), affinityData);

            AFFINITY_HOLDER.AddToAffinity(affinity.GetId(), amount);
            SyncToPlayer(farmer);
            UpdateIfNeeded(ModEntry.INSTANCE, farmer);
        }

        public void DecreaseAffinityDepth(Farmer farmer, string affinity, float amount)
        {
            //AffinityDatas.TryGetValue(affinity, out AffinityData affinityData);

            //affinityData.affinityDepth -= amount;

            //AffinityDatas.CreateNewOrUpdateExisting(affinity, affinityData);

            AFFINITY_HOLDER.SubtractFromAffinity(affinity, amount);
            SyncToPlayer(farmer);
            UpdateIfNeeded(ModEntry.INSTANCE, farmer);
        }

        public void DecreaseAffinityDepth(Farmer farmer, Affinity affinity, float amount)
        {
            //AffinityDatas.TryGetValue(affinity.GetId(), out AffinityData affinityData);

            //affinityData.affinityDepth += amount;

            //AffinityDatas.CreateNewOrUpdateExisting(affinity.GetId(), affinityData);

            AFFINITY_HOLDER.SubtractFromAffinity(affinity.GetId(), amount);
            SyncToPlayer(farmer);
            UpdateIfNeeded(ModEntry.INSTANCE, farmer);
        }

        public void ApplyAffinityShift(Farmer farmer, string affinity, float shift)
        {
            //ApplyAffinityShift(farmer, Objects.requireNonNull(ArsMagicaAPI.get().getAffinityRegistry().get(affinity)), shift);
            ApplyAffinityShift(farmer, Affinities.AFFINITIES.GetObjectList().Find(objectHolder => objectHolder.Get().GetId() == affinity).Get(), shift);
        }

        public void ApplyAffinityShift(Farmer farmer, Affinity affinity, float shift)
        {
            if (affinity.GetId() == Affinity.NONE) return;

            if (AFFINITY_HOLDER.Locked()) return;

            float adjacentDecrement = shift * ADJACENT_FACTOR;
            float minorOppositeDecrement = shift * MINOR_OPPOSING_FACTOR;
            float majorOppositeDecrement = shift * MAJOR_OPPOSING_FACTOR;

            AFFINITY_HOLDER.AddToAffinity(affinity.GetId(), shift);

            if (AFFINITY_HOLDER.GetAffinityDepth(affinity) == MAX_DEPTH)
            {
                AFFINITY_HOLDER.SetLocked(true);
            }

            foreach (string adjacent in affinity.GetAdjacentAffinities())
            {
                AFFINITY_HOLDER.SubtractFromAffinity(adjacent, adjacentDecrement);
            }

            foreach (string minorOpposite in affinity.minorOpposites)
            {
                AFFINITY_HOLDER.SubtractFromAffinity(minorOpposite, minorOppositeDecrement);
            }

            foreach (string majorOpposite in affinity.majorOpposites)
            {
                AFFINITY_HOLDER.SubtractFromAffinity(majorOpposite, majorOppositeDecrement);
            }

            string directOpposite = affinity.directOpposite;
            AFFINITY_HOLDER.SubtractFromAffinity(directOpposite, shift);

            SyncToPlayer(farmer);
            UpdateIfNeeded(ModEntry.INSTANCE, farmer);
        }

        public void Lock(Farmer farmer)
        {
            AFFINITY_HOLDER.SetLocked(true);
        }

        public void Unlock(Farmer farmer)
        {
            AFFINITY_HOLDER.SetLocked(false);
        }

        public void UpdateLock(Farmer farmer)
        {
            AFFINITY_HOLDER.SetLocked(true);

            foreach (var affinityHolder in Affinities.AFFINITIES.GetObjectList())
            {
                Affinity affinity = affinityHolder.Get();

                if (affinity.GetId().Equals(Affinity.NONE)) continue;

                if (AFFINITY_HOLDER.GetAffinityDepth(affinity) == MAX_DEPTH)
                {
                    Lock(farmer);
                    return;
                }
            }

            Unlock(farmer);
            SyncToPlayer(farmer);
        }

        /// <summary>
        /// Updates the Affinities to what is in the player mod data
        /// </summary>
        /// <param name="modEntry">Mod entry point object</param>
        /// <param name="player">The player</param>
        public void UpdateIfNeeded(ModEntry modEntry, Farmer player)
        {
            //this.AffinityDatas = player.modData.GetCustom(AffinitiesKey, parse => this.ParseAffinities(modEntry, parse), suppressError: false) ?? new Dictionary<string, AffinityData>();
            Dictionary<string, double> depths = player.modData.GetCustom(AffinitiesKey, parse => this.ParseAffinities(modEntry, parse), suppressError: false) ?? new Dictionary<string, double>();

            if(depths.Count == 0)
            {
                depths.Add(Affinities.EARTH.Get().GetId(), 0);
                depths.Add(Affinities.WATER.Get().GetId(), 0);
                depths.Add(Affinities.AIR.Get().GetId(), 0);
                depths.Add(Affinities.FIRE.Get().GetId(), 0);
                depths.Add(Affinities.NATURE.Get().GetId(), 0);
                depths.Add(Affinities.ICE.Get().GetId(), 0);
                depths.Add(Affinities.LIGHTNING.Get().GetId(), 0);
                depths.Add(Affinities.LIFE.Get().GetId(), 0);
                depths.Add(Affinities.ARCANE.Get().GetId(), 0);
                depths.Add(Affinities.DARKNESS.Get().GetId(), 0);
            }

            AFFINITY_HOLDER = new AffinityHolder(depths, false);
        }

        public AffinityHolder GetPlayerAffinityHolder(ModEntry modEntry, Farmer player)
        {
            //this.AffinityDatas = player.modData.GetCustom(AffinitiesKey, parse => this.ParseAffinities(modEntry, parse), suppressError: false) ?? new Dictionary<string, AffinityData>();
            Dictionary<string, double> depths = player.modData.GetCustom(AffinitiesKey, parse => this.ParseAffinities(modEntry, parse), suppressError: false) ?? new Dictionary<string, double>();

            return new AffinityHolder(depths, false);
        }

        /// <summary>
        /// Updates the player's mod data to what is in Affinities
        /// </summary>
        /// <param name="player"></param>
        public void SyncToPlayer(Farmer player)
        {
            //player.modData.SetCustom(AffinitiesKey, AffinityDatas.Values, serialize: this.SerializeAffinityDatas);
            player.modData.SetCustom(AffinitiesKey, AFFINITY_HOLDER.Depths(), serialize: this.SerializeAffinityDatas);
        }

        /// <summary>Parse serialized known spell part skills.</summary>
        /// <param name="raw">The raw serialized string.</param>
        private Dictionary<string, double> ParseAffinities(ModEntry modEntry, string raw)
        {
            Dictionary<string, double> depths = new();

            if (raw != null)
            {
                //foreach (double depth in this.ParseAffinity(modEntry, raw))
                //{
                //    depths.Add(affinityData.id, depth);
                //}

                depths = this.ParseAffinity(modEntry, raw);
            }

            return depths;
        }

        /// <summary>Parse a serialized affinity list.</summary>
        /// <param name="raw">The raw serialized string.</param>
        private Dictionary<string, double> ParseAffinity(ModEntry modEntry, string raw)
        {
            //List<double> depths = new();
            Dictionary<string, double> depths = new();

            if (string.IsNullOrWhiteSpace(raw))
                return depths;

            foreach (string rawAffinityData in raw.Split('&'))
            {
                if (string.IsNullOrWhiteSpace(rawAffinityData))
                    depths.Add("null", 0);
                else
                {
                    var id = rawAffinityData.Split(',')[0];
                    var depth = rawAffinityData.Split(',')[1];

                    depths.Add(id, Double.Parse(depth));
                }
            }

            return depths;
        }
        //private List<AffinityData> ParseAffinity(ModEntry modEntry, string raw)
        //{
        //    List<AffinityData> affinityData = new();

        //    if (string.IsNullOrWhiteSpace(raw))
        //        return affinityData;

        //    foreach (string rawAffinityData in raw.Split('&'))
        //    {
        //        if (string.IsNullOrWhiteSpace(rawAffinityData))
        //            affinityData.Add(null);
        //        else
        //        {
        //            foreach (AffinityData data in AffinityDatas.Values)
        //            {

        //                if (rawAffinityData.Contains(data.id))
        //                {
        //                    affinityData.Add(data);
        //                }
        //            }
        //        }
        //    }

        //    return affinityData;
        //}

        /// <summary>Serialize affinity datas for storage.</summary>
        /// <param name="affinityDatas">The affinity datas to serialize.</param>
        private string SerializeAffinityDatas(Dictionary<string, double> depths)
        {
            //return string.Join("&", depths.Select(data => "affinity" + "{" + $"{data.Key}" + "," + $"{data.Value}" + "}")).TrimEnd('&');
            return string.Join("&", depths.Select(data => $"{data.Key}" + "," + $"{data.Value}")).TrimEnd('&');
        }

        public class AffinityHolder
        {
            private Dictionary<string, double> depths = new Dictionary<string, double>();
            //private Dictionary<string, AffinityData> depths = new Dictionary<string, AffinityData>();
            private bool locked;

            public AffinityHolder(Dictionary<string, double> depths, bool locked)
            {
                this.depths = depths;
                this.locked = locked;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>An affinity holder.</returns>
            public static AffinityHolder Empty()
            {
                return new AffinityHolder(new Dictionary<string, double>(), false);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>A dictionary of all affinity depths in this holder.</returns>
            public Dictionary<string, double> Depths()
            {
                return depths;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns>Whether this affinity holder is locked or not.</returns>
            public bool Locked()
            {
                return locked;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="affinity">The id of the affinity to get the depth for.</param>
            /// <returns></returns>
            public double GetAffinityDepth(string affinity)
            {
                return Depths().GetValueOrDefault(affinity, 0d);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="affinity">The affinity to get the depth for.</param>
            /// <returns>The depth for the given affinity.</returns>
            public double GetAffinityDepth(Affinity affinity)
            {
                return GetAffinityDepth(affinity.GetId());
            }

            /// <summary>
            /// Adds the given shift to the given affinity.
            /// </summary>
            /// <param name="affinity">The id of the affinity to add the given shift to.</param>
            /// <param name="shift">The shift to add.</param>
            public void SetAffinity(string affinity, float shift)
            {
                //depths.compute(affinity, (rl, curr)->Mth.clamp((double)shift, 0, MAX_DEPTH));
                depths.Compute(affinity, (s, curr) => Math.Clamp((double)shift, 0, MAX_DEPTH));
            }

            /// <summary>
            /// Adds the given shift to the given affinity.
            /// </summary>
            /// <param name="affinity">The id of the affinity to add the given shift to.</param>
            /// <param name="shift">The shift to add.</param>
            public void AddToAffinity(string affinity, float shift)
            {
                depths.Compute(affinity, (s, curr) => Math.Clamp(curr + shift, 0, MAX_DEPTH));
            }

            /// <summary>
            /// Subtracts the given shift from the given affinity.
            /// </summary>
            /// <param name="affinity">The id of the affinity to add the given shift to.</param>
            /// <param name="shift">The shift to subtract.</param>
            public void SubtractFromAffinity(string affinity, float shift)
            {
                depths.Compute(affinity, (s, curr) => Math.Clamp(curr - shift, 0, MAX_DEPTH));
            }

            public void SetLocked(bool locked)
            {
                this.locked = locked;
            }

            public override bool Equals(object obj)
            {
                if (obj == this) return true;

                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                AffinityHolder that = (AffinityHolder)obj;

                return base.Equals(that) && locked == that.locked;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override String ToString()
            {
                return "AffinityHolder[" + "depths=" + depths + ",locked=" + locked + ']';
            }


            public static AffinityHolder Copy(AffinityHolder affinityHolder)
            {
                return new AffinityHolder(new Dictionary<string, double>(affinityHolder.Depths()), affinityHolder.Locked());
            }
        }
    }
}
