using ArsVenefici.Framework.Spells.Registry;
using Microsoft.Xna.Framework;

namespace ArsVenefici.Framework.API.affinity
{
    public record Affinity(string id, Color color, HashSet<string> minorOpposites, HashSet<string> majorOpposites, string directOpposite, string castSound, string loopSound) : IComparable<Affinity>
    {
        public static string AFFINITY = "affinity";
        public static string REGISTRY_KEY = ModEntry.ArsVenificiModId + "_" + AFFINITY;

        //public static string NONE      = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "none";
        //public static string EARTH = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "earth";
        //public static string WATER = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "water";
        //public static string AIR = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "air";
        //public static string FIRE = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "fire";
        //public static string NATURE = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "nature";
        //public static string ICE = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "ice";
        //public static string LIGHTNING = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "lightning";
        //public static string LIFE = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "life";
        //public static string ARCANE    = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "arcane";
        //public static string DARKNESS = ModEntry.ArsVenificiModId + "_" + "affinity" + "_" + "darkness";

        public static string NONE = "none";
        public static string EARTH = "earth";
        public static string WATER = "water";
        public static string AIR = "air";
        public static string FIRE = "fire";
        public static string NATURE = "nature";
        public static string ICE = "ice";
        public static string LIGHTNING = "lightning";
        public static string LIFE = "life";
        public static string ARCANE = "arcane";
        public static string DARKNESS = "darkness";

        /// <returns>A new affinity builder.</returns>
        public static Builder CreateBuilder()
        {
            return new Builder();
        }

        /// <returns>The minor opposing affinities for this affinity.</returns>
        public HashSet<string> GetMinorOpposites()
        {
            return minorOpposites;
        }

        /// <returns>The major opposing affinities for this affinity.</returns>
        public HashSet<string> GetMajorOpposites()
        {
            return majorOpposites;
        }

        public HashSet<string> GetAdjacentAffinities()
        {
            //return ArsMagicaAPI.get().getAffinityRegistry().getValues().stream().filter(iAffinity-> !minorOpposites().contains(iAffinity.getId()) && !majorOpposites().contains(iAffinity.getId()) && !directOpposite().equals(iAffinity.getId())).map(Affinity::getId).collect(Collectors.toSet());
            return Affinities.AFFINITIES.GetObjectList().Where(affinity => !GetMinorOpposites().Contains(affinity.Get().GetId()) && !GetMajorOpposites().Contains(affinity.Get().GetId()) && !directOpposite.Equals(affinity.Get().GetId())).Select(aff => aff.Get().GetId()).ToHashSet();
        }


        public string GetCastSound()
        {
            return castSound;
        }

        public string GetLoopSound()
        {
            return loopSound;
        }

        public string GetAffinityType()
        {
            return AFFINITY;
        }

        public string GetId()
        {
            //return Objects.requireNonNull(ArsMagicaAPI.get().getAffinityRegistry().getKey(this));
            return id;
        }

        public int CompareTo(Affinity obj)
        {
            //if(obj is Affinity other)
            //{
            //    Comparison<Affinity> comparison = (Comparison<Affinity>)obj;
            //    Comparer<Affinity> comparer = Comparer<Affinity>.Create(comparison);

            //    return comparer.Compare(this, other);
            //}

            return Compare(this, obj);
        }

        public int Compare(Affinity affinity, Affinity other)
        {

            if(affinity == null)
            {
                if(other == null)
                {
                    // If affinity is null and other is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If affinity is null and other is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {
                // If affinity is not null...
                // ...and other is null, affinity is greater.
                if (other == null)
                {
                    return 1;
                }
                else
                {
                    // ...and other is not null, first compare the
                    // lengths of the two id strings.
                    int idCompare = affinity.id.CompareTo(other.id);

                    if (idCompare != 0)
                    {
                        // If the id strings are not of equal length,
                        // the longer string is greater.
                        return idCompare;
                    }
                    else
                    {
                        // If the id strings are of equal length,
                        // sort them by color value.

                        int colorCompare = affinity.color.PackedValue.CompareTo(other.color.PackedValue);

                        if (colorCompare != 0)
                        {
                            return colorCompare;
                        }
                        else
                        {
                            // If the color values are of equal,
                            // sort them by directOpposite.
                            return affinity.directOpposite.CompareTo(other.directOpposite);
                        }
                    }
                }
            }
        }

        public class Builder
        {
            private string id;

            private HashSet<string> minorOpposites = new HashSet<string>();
            private HashSet<string> majorOpposites = new HashSet<string>();

            private Color color;

            private string directOpposite;

            private string castSound;
            private string loopSound;

            public Builder SetId(string id)
            {
                this.id = id;
                return this;
            }

            /// <summary>
            /// The color to set.
            /// </summary>
            /// <param name="color">The color to set.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder SetColor(Color color)
            {
                this.color = color;
                return this;
            }

            /// <summary>
            /// The minor opposite to add.
            /// </summary>
            /// <param name="minorOpposite">The minor opposite to add.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder AddMinorOpposite(String minorOpposite)
            {
                minorOpposites.Add(minorOpposite);
                return this;
            }

            /// <summary>
            /// The major opposite to add.
            /// </summary>
            /// <param name="majorOpposite">The major opposite to add.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder AddMajorOpposite(String majorOpposite)
            {
                majorOpposites.Add(majorOpposite);
                return this;
            }

            /// <summary>
            /// The minor opposite(s) to add.
            /// </summary>
            /// <param name="minorOpposite">The minor opposite(s) to add.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder AddMinorOpposites(params string[] minorOpposite)
            {
                foreach (var item in minorOpposite)
                {
                    minorOpposites.Add(item);
                }

                return this;
            }

            /// <summary>
            /// The major opposite(s) to add.
            /// </summary>
            /// <param name="majorOpposite">The major opposite(s) to add.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder AddMajorOpposites(params string[] majorOpposite)
            {
                foreach (var item in majorOpposite)
                {
                    majorOpposites.Add(item);
                }

                return this;
            }

            /// <summary>
            /// The direct opposite to set.
            /// </summary>
            /// <param name="directOpposite">The direct opposite to set.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder SetDirectOpposite(string directOpposite)
            {
                this.directOpposite = directOpposite;
                return this;
            }

            /// <summary>
            /// The cast sound to set.
            /// </summary>
            /// <param name="castSound">The cast sound to set.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder SetCastSound(string castSound)
            {
                this.castSound = castSound;
                return this;
            }

            /// <summary>
            /// The loop sound to set.
            /// </summary>
            /// <param name="loopSound">The loop sound to set.</param>
            /// <returns>This builder, for chaining.</returns>
            public Builder SetLoopSound(string loopSound)
            {
                this.loopSound = loopSound;
                return this;
            }

            /// <summary>
            /// The affinity created from this builder.
            /// </summary>
            /// <returns>The built Affinity instance.</returns>
            public Affinity Build()
            {

                //if (this.color.)
                //{
                //    throw new InvalidOperationException("An affinity needs a color!");
                //}

                //if (directOpposite == null)
                //{
                //    throw new InvalidOperationException("An affinity needs a direct opposite!");
                //}

                return new Affinity(id, color, minorOpposites, majorOpposites, directOpposite, castSound, loopSound);
            }
        }
    }
}
