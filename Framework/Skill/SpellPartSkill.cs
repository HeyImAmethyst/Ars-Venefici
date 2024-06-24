using ArsVenefici.Framework.GUI.Menus;
using Microsoft.Win32;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Skill
{
    public class SpellPartSkill
    {
        HashSet<SpellPartSkill> parents;
        Dictionary<Item, int> cost;
        MagicAltarTab tab;

        int x;
        int y;

        bool hidden;

        string id;

        public SpellPartSkill(string id, HashSet<SpellPartSkill> parents, Dictionary<Item, int> cost, MagicAltarTab tab, int x, int y, bool hidden)
        {
            this.id = id;
            this.parents = parents;
            this.cost = cost;
            this.tab = tab;
            this.x = x;
            this.y = y;
            this.hidden = hidden;
        }

        public MagicAltarTab GetOcculusTab()
        {
            return this.tab;
        }

        public HashSet<SpellPartSkill> Parents()
        {
            return parents;
        }

        public Dictionary<Item, int> Cost()
        {
            return cost;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
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
