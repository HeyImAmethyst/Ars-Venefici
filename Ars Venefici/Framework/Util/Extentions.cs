using ArsVenefici.Framework.API.affinity;
using ArsVenefici.Framework.FarmerPlayer;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Registry;
using Microsoft.Xna.Framework;
using SpaceShared.APIs;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
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

        //-----------------DICTIONARY-----------------

        //From https://stackoverflow.com/questions/63476986/equivalent-map-compute-in-c-sharp
        public static V Compute<K, V>(this Dictionary<K, V> dict, K key, Func<K, V, V> func)
        {
            // if no func given, throw.
            if (func == null) throw new ArgumentNullException(nameof(func));

            // if no mapping, return null.
            if (!dict.TryGetValue(key, out var value)) return default;

            // get the new value from func.
            var result = func(key, value);

            if (result == null)
            {
                // if the mapping exists but func => null,
                // remove the mapping and return null.

                dict.Remove(key);
                return default;
            }

            // mapping exists and func returned a non-null value.
            // set and return the new value
            dict[key] = result;
            return result;
        }

        //From https://stackoverflow.com/questions/4245064/method-to-add-new-or-update-existing-item-in-c-sharp-dictionary
        public static void CreateNewOrUpdateExisting<TKey, TValue>(this IDictionary<TKey, TValue> map, TKey key, TValue value)
        {
            if (map.ContainsKey(key))
            {
                map[key] = value;
            }
            else
            {
                map.Add(key, value);
            }
        }

        //-----------------STRING-----------------

        public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };

        //-----------------MATH-----------------

        public const double DegToRad = Math.PI / 180;

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

        //-----------------MOD APIs-----------------

        /// <summary>Get the mod API for Generic Mod Config Menu, if it's loaded and compatible.</summary>
        /// <param name="modRegistry">The mod registry to extend.</param>
        /// <param name="monitor">The monitor with which to log errors.</param>
        /// <returns>Returns the API instance if available, else <c>null</c>.</returns>
        public static IGenericModConfigMenuApi GetGenericModConfigMenuApi(this IModRegistry modRegistry, IMonitor monitor)
        {
            return modRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
        }

        //-----------------MONSTER-----------------

        public static Affinity GetAffinity(this Monster monster)
        {
            if(monster != null)
            {
                //Earth
                if (monster.Name.Equals("Duggy") ||
                    monster.Name.Equals("Rock Crab") ||
                    monster.Name.Equals("Skeleton") ||
                    monster.IsBrownSlime() ||
                    monster.Name.Equals("Mummy") ||
                    monster.IsGraySlime() ||
                    monster.Name.Equals("Stone Golem"))
                {
                    return Affinities.EARTH.Get();
                }

                //Water
                if (
                    monster.Name.Equals("Blue Squid"))
                {
                    return Affinities.WATER.Get();
                }

                //Air
                if (monster.Name.Equals("Cave Fly") ||
                    monster.Name.Equals("Ghost") ||
                    monster.Name.Equals("Carbon Ghost") ||
                    monster.Name.Equals("Bat"))
                {
                    return Affinities.AIR.Get();
                }

                //Fire
                if (monster is LavaLurk ||
                    monster is HotHead ||
                    monster.Name.Equals("Magma Sprite") ||
                    monster.Name.Equals("Magma Sparker") ||
                    monster.Name.Equals("False Magma Cap") ||
                    monster.Name.Equals("Magma Duggy") ||
                    monster.Name.Equals("Lava Crab") ||
                    monster.Name.Equals("Metal Head") ||
                    monster.IsRedSlime() ||
                    monster.IsTigerSlime() ||
                    monster.Name.Equals("Lava Bat"))
                {
                    return Affinities.FIRE.Get();
                }

                //Nature
                if (monster.IsGreenSlime() ||
                    monster.Name.Equals("Bug") ||
                    monster.Name.Equals("Spider") ||
                    monster.Name.Equals("Stick Bug") ||
                    monster.Name.Equals("Pepper Rex") ||
                    monster.Name.Equals("Truffle Crab") ||
                    monster.Name.Equals("Wilderness Golem") ||
                    monster.Name.Equals("Serpent") ||
                    monster.Name.Equals("Grub"))
                {
                    return Affinities.NATURE.Get();
                }

                //Ice
                if (monster.Name.Equals("Dust Sprite") ||
                    monster.IsFrostJelly() ||
                    monster.Name.Equals("Frost Bat"))
                {
                    return Affinities.ICE.Get();
                }

                //Lightning
                if (monster.IsYellowSlime() || monster.IsWhiteSlime())
                {
                    return Affinities.LIGHTNING.Get();
                }

                //Arcane
                if (monster.IsPurpleSlime() ||
                monster.Name.Equals("Iridium Bat") ||
                monster.Name.Equals("Iridium Golem") ||
                monster.Name.Equals("Skeleton Mage") ||
                monster.Name.Equals("Iridium Crab"))
                {
                    return Affinities.ARCANE.Get();
                }

                //Darkness
                if (monster.Name.Equals("Dust Sprite") ||
                    monster.Name.Equals("Shadow Shaman") ||
                    monster.Name.Equals("Shadow Sniper") ||
                    monster.IsBlackSlime() ||
                    monster.Name.Equals("Haunted Skull") ||
                    monster.Name.Equals("Squid Kid") ||
                    monster.Name.Equals("Shadow Brute"))
                {
                    return Affinities.DARKNESS.Get();
                }
            }

            return Affinities.NONE.Get();
        }

        public static void AddAffinityBasedDropsForMonster(this Monster monster)
        {
            if (Utils.PercentChance(0.08))
            {
                switch (monster.GetAffinity().id)
                {
                    case "earth":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Earth_Essence");
                        break;
                    case "water":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Water_Essence");
                        break;
                    case "air":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Air_Essence");
                        break;
                    case "fire":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Fire_Essence");
                        break;
                    case "nature":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Nature_Essence");
                        break;
                    case "ice":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Ice_Essence");
                        break;
                    case "lightning":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Lightning_Essence");
                        break;
                    //case "life":
                    //    if (Game1.random.NextDouble() < 0.95)
                    //        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Life_Essence");
                        break;
                    case "arcane":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Arcane_Essence");
                        break;
                    case "darkness":
                        monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Darkness_Essence");
                        break;
                    default:
                        break;
                }

                if (Utils.PercentChance(0.30))
                    monster.objectsToDrop.Add("HeyImAmethyst.ArsVenefici_Life_Essence");
            }
        }

        public static void AddAffinityBasedDropsForAllMonsters(this GameLocation location)
        {
            foreach (var character in location.characters)
            {
                if (character != null && character is Monster monster)
                {
                    monster.AddAffinityBasedDropsForMonster();
                }
            }
        }

        public static bool IsGreenSlime(this Monster monster)
        {
            if (monster is GreenSlime slime && monster.Name.Equals("Green Slime"))
            {
                return true;
            }

            if (monster is BigSlime bigSlime)
            {
                if (bigSlime.currentLocation is MineShaft mine)
                {
                    if (mine.getMineArea() == 10)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public static bool IsFrostJelly(this Monster monster)
        {
            if (monster is GreenSlime slime)
            {
                if (slime.Name.Equals("Frost Jelly"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if(monster is BigSlime bigSlime)
            {
                if(bigSlime.currentLocation is MineShaft mine)
                {
                    if(mine.getMineArea() == 40)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public static bool IsRedSlime(this Monster monster)
        {
            if (monster is GreenSlime slime && monster.Name.Equals("Sludge"))
            {
                //if (slime.color.R > 220 && (slime.color.G > 90 && slime.color.G < 150) && slime.color.B < 50)
                //{
                //    return true;
                //}
                //if ((slime.color.R > 220) && slime.color.G < 80 && slime.color.B > 150)

                //int green = Game1.random.Next(200, 256);
                //this.color.Value = new Color(green, (Game1.random.NextDouble() < 0.01) ? 255 : (255 - green), green / Game1.random.Next(2, 10));

                //Red: 138 Green: 43 Blue:226

                //min -20, max 21

                //Red: min 200 max 255
                //Green: min 55 max 255
                //Blue: min 26 max 100

                if ((slime.color.R > 200 && slime.color.R < 255) && slime.color.G < 55 && (slime.color.B > 26 && slime.color.B < 100))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (monster is BigSlime bigSlime)
            {
                if (bigSlime.currentLocation is MineShaft mine)
                {
                    if (mine.getMineArea() == 80)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public static bool IsPurpleSlime(this Monster monster)
        {
            if (monster is GreenSlime slime && monster.Name.Equals("Sludge"))
            {
                //Red: 138 Green: 43 Blue:226
                
                //min -20, max 21

                //Red: min 118 max 159
                //Green: min 20 max 64
                //Blue: min 206 max 247

                if ((slime.color.R > 118 && slime.color.R < 159) && (slime.color.G > 20 && slime.color.G < 64) && (slime.color.B > 206 && slime.color.B < 247))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (monster is BigSlime bigSlime)
            {
                if (bigSlime.currentLocation is MineShaft mine)
                {
                    if (mine.getMineArea() == 121)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        public static bool IsYellowSlime(this Monster monster)
        {
            if (monster is GreenSlime slime)
            {
                if (slime.color.R > 200 && slime.color.G > 180 && slime.color.B < 50)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsBlackSlime(this Monster monster)
        {
            if (monster is GreenSlime slime)
            {
                if (slime.color.R < 80 && slime.color.G < 80 && slime.color.B < 80)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsBrownSlime(this Monster monster)
        {
            if (monster is GreenSlime slime)
            {
                if ((slime.color.R > 49 && slime.color.R < 101) && (slime.color.G > 24 && slime.color.G < 51) && slime.color.B < 26)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsGraySlime(this Monster monster)
        {
            if (monster is GreenSlime slime)
            {
                if (slime.color.R > 150 && slime.color.G > 150 && slime.color.B > 150)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsWhiteSlime(this Monster monster)
        {
            if (monster is GreenSlime slime)
            {
                if (slime.color.R > 230 && slime.color.G > 230 && slime.color.B > 230)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        public static bool IsTigerSlime(this Monster monster)
        {
            if (monster is GreenSlime slime)
            {
                if (slime.Name.Equals("Tiger Slime"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        //-----------------PLAYER DATA-----------------

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
