using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.ContentPacks;
using ArsVenefici.Framework.ContentPacks.DataModels;
using ArsVenefici.Framework.Spells;
using ItemExtensions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsVenefici.Framework.GUI
{
    public class SpellPartIconManager
    {
        public Dictionary<string, Texture2D> spellPartSprites = new Dictionary<string, Texture2D>();
        public Dictionary<string, Texture2D> contentPackSpellPartSprites = new Dictionary<string, Texture2D>();

        // Instance of ContentPackHelper
        private ContentPackHelper PackHelper;
        private ModEntry modEntry;

        public SpellPartIconManager(ModEntry modEntry, ContentPackHelper packHelper)
        {
            PackHelper = packHelper;
            this.modEntry = modEntry;
        }

        public void PoplulateSprites()
        {
            if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
            {
                foreach (KeyValuePair<string, ISpellPart> item in modEntry.spellPartManager.GetSpellParts())
                {
                    modEntry.Monitor.Log("Popluating spell icons", LogLevel.Info);
                    if (item.Value != null)
                        PoplulateSprites(item.Value, modEntry);
                }

                foreach (KeyValuePair<string, ISpellPart> item in modEntry.spellPartManager.GetContentPackSpellParts())
                {
                    modEntry.Monitor.Log("Popluating content pack spell icons", LogLevel.Info);
                    if (item.Value != null)
                        PoplulateContentPackSprites(item.Value, modEntry);
                }
            }
        }

        public virtual void PoplulateSprites(ISpellPart spellPart, ModEntry modEntry)
        {
            try
            {
                spellPartSprites.Add(spellPart.GetId(), modEntry.Helper.ModContent.Load<Texture2D>("assets/icon/spellpart/" + spellPart.GetId() + ".png"));
            }
            catch (ContentLoadException e)
            {
                modEntry.Monitor.Log("Failed to load icon for spell " + spellPart.GetId() + ": " + e, LogLevel.Warn);
            }
        }

        public virtual void PoplulateContentPackSprites(ISpellPart spellPart, ModEntry modEntry)
        {
            try
            {
                foreach (SpellIconModel spellIcons in PackHelper.SpellIconsList)
                {
                    foreach (Texture2D texture2D in spellIcons.Textures)
                    {
                        modEntry.Monitor.Log(texture2D.Name, LogLevel.Info);
                        modEntry.Monitor.Log(Path.GetFileNameWithoutExtension(texture2D.Name), LogLevel.Info);

                        if (spellPart.GetId() == Path.GetFileNameWithoutExtension(texture2D.Name))
                        {
                            contentPackSpellPartSprites.Add(spellPart.GetId(), texture2D);
                        }
                    }
                }
            }
            catch (ContentLoadException e)
            {
                modEntry.Monitor.Log("Failed to load icon for spell " + spellPart.GetId() + ": " + e, LogLevel.Warn);
            }
        }

        public Texture2D GetSprite(string name)
        {
            Texture2D sprite = null;
            return spellPartSprites.TryGetValue(name, out sprite) ? sprite : contentPackSpellPartSprites.TryGetValue(name, out sprite) ? sprite : null;
        }
    }
}
