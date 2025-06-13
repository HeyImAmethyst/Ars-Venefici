using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.Spells.Components;
using ArsVenefici.Framework.Spells.Modifiers;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Util;

namespace ArsVenefici.Framework.Spells
{
    public class SpellPartManager
    {
        private Dictionary<string, ISpellPart> spellParts = new Dictionary<string, ISpellPart>();
        private ModEntry modEntry;

        public SpellPartManager(ModEntry modEntry) 
        {
            this.modEntry = modEntry;
            PopluateDictionary();
        }

        private void PopluateDictionary()
        {
            AddShapes();
            AddComponents();
            AddModifiers();
        }

        private void AddShapes()
        {
            foreach (ObjectHolder<AbstractShape> holder in ArsShapes.SHAPES.GetObjectList())
            {
                AddShape(holder.Get());
            }
        }

        private void AddComponents()
        {
            foreach (ObjectHolder<AbstractComponent> holder in ArsComponents.COMPONENTS.GetObjectList())
            {
                AddComponent(holder.Get());
            }
        }

        private void AddModifiers()
        {
            foreach (ObjectHolder<AbstractModifier> holder in ArsModifiers.MODIFIERS.GetObjectList())
            {
                AddModifier(holder.Get());
            }
        }

        public void AddShape(AbstractShape shape)
        {
            spellParts.Add(shape.GetId(), shape);
        }

        public void AddComponent(AbstractComponent component)
        {
            spellParts.Add(component.GetId(), component);
        }

        public void AddModifier(AbstractModifier modifier)
        {
            spellParts.Add(modifier.GetId(), modifier);
        }

        public Dictionary<string, ISpellPart> GetSpellParts()
        {
            return spellParts;
        }
    }
}
