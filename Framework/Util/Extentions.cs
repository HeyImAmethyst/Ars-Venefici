﻿using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.Spells;
using Microsoft.Xna.Framework;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Util
{
    public static class Extentions
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        //*****MATH*****

        private const double DegToRad = Math.PI / 180;

        public static Vector2 Rotate(this Vector2 v, double degrees)
        {
            return v.RotateRadians(degrees * DegToRad);
        }

        public static Vector2 RotateRadians(this Vector2 v, double radians)
        {
            var ca = Math.Cos(radians);
            var sa = Math.Sin(radians);
            return new Vector2((float)(ca * v.X - sa * v.Y), (float)(sa * v.X + ca * v.Y));
        }

        public static void Resize<T>(this IList<T> list, int size)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            if (list is List<T> genericList)
            {
                genericList.RemoveRange(size, list.Count - size);
            }
            else
            {
                while (list.Count > size)
                    list.RemoveAt(list.Count - 1);
            }
        }

        //*****MOD APIs*****

        /// <summary>Get the mod API for Generic Mod Config Menu, if it's loaded and compatible.</summary>
        /// <param name="modRegistry">The mod registry to extend.</param>
        /// <param name="monitor">The monitor with which to log errors.</param>
        /// <returns>Returns the API instance if available, else <c>null</c>.</returns>
        public static IGenericModConfigMenuApi GetGenericModConfigMenuApi(this IModRegistry modRegistry, IMonitor monitor)
        {
            return modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        }

        //*****PLAYER DATA*****

        public static int GetCurrentMana(this Farmer player)
        {
            return ModEntry.ManaBarApi.GetMana(player);
        }

        public static void AddMana(this Farmer player, int amt)
        {
            ModEntry.ManaBarApi.AddMana(player, amt);
        }

        public static int GetMaxMana(this Farmer player)
        {
            return ModEntry.ManaBarApi.GetMaxMana(player);
        }

        public static void SetMaxMana(this Farmer player, int newCap)
        {
            ModEntry.ManaBarApi.SetMaxMana(player, newCap);
        }

        /// <summary>Get a self-updating cached view of the player's magic metadata.</summary>
        public static SpellBook GetSpellBook(this Farmer player)
        {
            return FarmerMagicHelper.GetSpellBook(player);
        }

        public static FarmerExtData GetExtData(this Farmer farmer)
        {
            return FarmerExtData.data.GetOrCreateValue(farmer);
        }
    }
}
