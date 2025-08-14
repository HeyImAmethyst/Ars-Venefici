using Microsoft.Win32;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.API.Skill
{
    public class SpellPartSkill
    {
        HashSet<SpellPartSkill> parents;
        Dictionary<Item, int> cost;
        MagicAltarTab tab;

        bool hidden;

        string id;

        public SpellPartSkill(string id, HashSet<SpellPartSkill> parents, Dictionary<Item, int> cost, MagicAltarTab tab, bool hidden)
        {
            this.id = id;
            this.parents = parents;
            this.cost = cost;
            this.tab = tab;
            this.hidden = hidden;
        }

        public SpellPartSkill(string id, Dictionary<Item, int> cost, MagicAltarTab tab, bool hidden)
        {
            this.id = id;
            this.cost = cost;
            this.tab = tab;
            this.hidden = hidden;
        }

        public MagicAltarTab GetOcculusTab()
        {
            return tab;
        }

        public HashSet<SpellPartSkill> Parents()
        {
            return parents;
        }

        public void SetParents(HashSet<SpellPartSkill> parents)
        {
            this.parents = parents;
        }

        public void SetParents(params SpellPartSkill[] parents)
        {
            this.parents = new HashSet<SpellPartSkill>(parents);
        }

        public Dictionary<Item, int> Cost()
        {
            return cost;
        }

        public bool IsHidden()
        {
            return hidden;
        }

        public string GetId()
        {
            return id;
        }
    }
}
