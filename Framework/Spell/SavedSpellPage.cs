using ArsVenefici.Framework.GUI.DragNDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.Spell
{
    public class SavedSpellPage
    {
        private SavedShapeGroupArea<SpellPartDraggable>[] spellShapes = new SavedShapeGroupArea<SpellPartDraggable>[5];

        private List<SpellPartDraggable> spellGrammerList = new List<SpellPartDraggable>();

        private string name { get; set; }

        public void SetSpellShapes(SavedShapeGroupArea<SpellPartDraggable>[] spellShapes)
        {
            this.spellShapes = spellShapes;
        }

        public SavedShapeGroupArea<SpellPartDraggable>[] GetSpellShapes()
        {
            return this.spellShapes;
        }

        public void SetSpellGrammerList(List<SpellPartDraggable> spellGrammerList)
        {
            this.spellGrammerList = spellGrammerList;
        }

        public List<SpellPartDraggable> GetSpellGrammerList()
        {
            return this.spellGrammerList;
        }

        public string GetName()
        {
            return name;
        }

        public void SetName(string name)
        {
            this.name = name;
        }
    }
}
