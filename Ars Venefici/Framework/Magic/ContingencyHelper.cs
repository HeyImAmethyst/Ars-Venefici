using ArsVenefici.Framework.API;
using ArsVenefici.Framework.API.Magic;
using ArsVenefici.Framework.API.Spell;
using ArsVenefici.Framework.GUI.DragNDrop;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Spells;
using ArsVenefici.Framework.Spells.Registry;
using ArsVenefici.Framework.Spells.Shape;
using ArsVenefici.Framework.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Mods;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static SpaceCore.Skills;

namespace ArsVenefici.Framework.Magic
{
    /// <summary>
    /// Json object class for storing the spell part id
    /// </summary>
    public class ContingencySpellPartJson
    {
        [JsonProperty("Spell Part")]
        public string spellPartId { get; set; }
    }

    /// <summary>
    /// Json object class for storing shape group contents
    /// </summary>
    public class ContingencyShapeGroupJson
    {
        [JsonProperty("Contents")]
        public List<ContingencySpellPartJson> spellParts { get; set; }
    }

    /// <summary>
    /// Json object class for storing contingency spells
    /// </summary>
    public class ContingencySpellJson
    {
        [JsonProperty("Shape Groups")]
        public List<ContingencyShapeGroupJson> shapeGroups { get; set; }

        [JsonProperty("Spell Grammar")]
        public List<ContingencySpellPartJson> spellGrammerList { get; set; }

        [JsonProperty("Name")]
        public string name { get; set; }

        [JsonProperty("Type")]
        public string type { get; set; }

        [JsonProperty("Index")]
        public int index { get; set; }
    }

    public class ContingencyHelper : IContingencyHelper
    {
        private static ContingencyHelper INSTANCE = new ContingencyHelper();

        private const string Prefix = "HeyImAmethyst.ArsVenefici";
        public const string ContingencyKey = ContingencyHelper.Prefix + "/Contingency";

        private ContingencyHelper()
        {

        }

        public static ContingencyHelper Instance()
        {
            return INSTANCE;
        }

        /// <summary>
        /// Sets the contengency to a target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        /// <param name="spell"></param>
        public void SetContingency(Character target, ContingencyType type, ISpell spell)
        {
            if (target != null)
            {
                target.modData.SetCustom(ContingencyHelper.ContingencyKey, new Contingency(ModEntry.INSTANCE, type, spell, 1), SaveContingencySpell);
            }
        }

        /// <summary>
        /// Triggers a contengency
        /// </summary>
        /// <param name="target"></param>
        /// <param name="type"></param>
        public void TriggerContingency(Character target, ContingencyType type)
        {
            if (target != null)
            {
                if (!target.modData.ContainsKey(ContingencyHelper.ContingencyKey)) return;

                Contingency contingency = target.modData.GetCustom(ContingencyHelper.ContingencyKey, parse: (x) => ReadContingencySpell(x, ModEntry.INSTANCE), suppressError: false) ?? new Contingency();

                if (contingency != null)
                {
                    if (contingency.Type.Equals(ContingencyType.NONE)) return;

                    contingency.Execute(target.currentLocation, target);
                    ClearContingency(target);
                }
                else
                {
                    ModEntry.INSTANCE.Monitor.Log("contingency null", LogLevel.Info);
                }
            }

            if(target == null)
            {
                ModEntry.INSTANCE.Monitor.Log("target null", LogLevel.Info);
            }
        }

        /// <summary>
        /// Clears a contengency
        /// </summary>
        /// <param name="target"></param>
        public void ClearContingency(Character target)
        {
            if (target != null)
            {
                //target.modData.SetCustom(ContingencyHelper.ContingencyKey, new Contingency(), SaveContingencySpell);
                target.modData.Remove(ContingencyHelper.ContingencyKey);
            }
        }

        /// <summary>
        /// Gets a contengency type from a target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public ContingencyType GetContingencyType(Character target)
        {
            if(target != null && target.modData != null)
            {
                Contingency contingency = null;
                contingency = target.modData.GetCustom(ContingencyHelper.ContingencyKey, parse: (x) => ReadContingencySpell(x, ModEntry.INSTANCE), suppressError: false) ?? new Contingency();

                if(contingency != null)
                    return contingency.Type;
            }

            return ContingencyType.NONE;
        }

        /// <summary>
        /// Reads the Contingency Spell from a json file
        /// </summary>
        /// <param name="jsonString">The json string</param>
        /// <param name="modEntry">The mod entry point object</param>
        /// <returns></returns>
        public Contingency ReadContingencySpell(string jsonString, ModEntry modEntry)
        {
            //Converts the json string into an object
            ContingencySpellJson contingencySpellJson = JsonConvert.DeserializeObject<ContingencySpellJson>(jsonString);

            if (contingencySpellJson != null)
            {
                //modEntry.Monitor.Log("contingencySpellJson not null", StardewModdingAPI.LogLevel.Info);

                List<ShapeGroup> shapeGroups = new List<ShapeGroup>();

                //Creates the shapegroups
                for (int i = 0; i < contingencySpellJson.shapeGroups.Count; i++)
                {
                    shapeGroups.Add(ShapeGroup.EMPTY);
                }

                //Creates the spell grammar list
                List<ISpellPart> spellGrammarList = new List<ISpellPart>();

                //Builds the spell

                //Goes through each shape group json
                for (int i = 0; i < contingencySpellJson.shapeGroups.Count; i++)
                {
                    //Gets the current shape group json
                    ContingencyShapeGroupJson contingencyShapeGroup = contingencySpellJson.shapeGroups[i];

                    //Gets the shape group spell part jsons
                    List<ContingencySpellPartJson> contingencyShapeGroupSpellPartJsons = contingencyShapeGroup.spellParts;

                    //A list to hold the converted shape group spell part jsons to spell part objects
                    List<ISpellPart> contingencyShapeGroupSpellParts = new List<ISpellPart>();

                    //Goes through the spell part jsons
                    foreach (ContingencySpellPartJson contingencySpellPart in contingencyShapeGroupSpellPartJsons)
                    {

                        if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
                        {
                            //After the dictionaries are populated, checks if they have contingencySpellPart. If they do, the
                            //corresponding spell part added to the contingencyShapeGroupSpellParts list 

                            if (modEntry.spellPartManager.GetSpellParts().ContainsKey(contingencySpellPart.spellPartId))
                            {
                                //if (modEntry.spellPartManager.GetSpellParts()[contingencySpellPart.spellPartId] != null)
                                //{
                                //    ISpellPart spellPart = modEntry.spellPartManager.GetSpellParts()[contingencySpellPart.spellPartId];
                                //    contingencyShapeGroupSpellParts.Add(spellPart);
                                //}

                                if (modEntry.spellPartManager.GetSpellParts().TryGetValue(contingencySpellPart.spellPartId, out ISpellPart spellPart))
                                {
                                    contingencyShapeGroupSpellParts.Add(spellPart);
                                }
                            }

                            if (modEntry.spellPartManager.GetContentPackSpellParts().ContainsKey(contingencySpellPart.spellPartId))
                            {
                                //if(modEntry.spellPartManager.GetContentPackSpellParts()[contingencySpellPart.spellPartId] != null)
                                //{
                                //    ISpellPart spellPart = modEntry.spellPartManager.GetContentPackSpellParts()[contingencySpellPart.spellPartId];
                                //    contingencyShapeGroupSpellParts.Add(spellPart);
                                //}

                                if (modEntry.spellPartManager.GetContentPackSpellParts().TryGetValue(contingencySpellPart.spellPartId, out ISpellPart spellPart))
                                {
                                    contingencyShapeGroupSpellParts.Add(spellPart);
                                }
                            }
                        }
                    }

                    //Shape group is created from contingencyShapeGroupSpellParts list
                    ShapeGroup shapeGroup = ShapeGroup.Of(contingencyShapeGroupSpellParts);

                    //Created shape group is added to shape group list at the current index
                    shapeGroups[i] = shapeGroup;
                }

                //Goes through each spell part json
                foreach (ContingencySpellPartJson contingencySpellPartJson in contingencySpellJson.spellGrammerList)
                {
                    if (modEntry.spellPartManager.dictionariesPoplulated && modEntry.spellPartSkillManager.dictionariesPoplulated)
                    {
                        //After the dictionaries are populated, checks if they have contingencySpellPart. If they do, the
                        //corresponding spell part added to the spellGrammarList 

                        if (modEntry.spellPartManager.GetSpellParts().ContainsKey(contingencySpellPartJson.spellPartId))
                        {

                            //if (modEntry.spellPartManager.GetSpellParts()[contingencySpellPartJson.spellPartId] != null)
                            //{
                            //    ISpellPart spellPart = modEntry.spellPartManager.GetSpellParts()[contingencySpellPartJson.spellPartId];
                            //    spellGrammerList.Add(spellPart);
                            //}

                            if (modEntry.spellPartManager.GetSpellParts().TryGetValue(contingencySpellPartJson.spellPartId, out ISpellPart spellPart))
                            {
                                spellGrammarList.Add(spellPart);
                            }
                        }

                        if (modEntry.spellPartManager.GetContentPackSpellParts().ContainsKey(contingencySpellPartJson.spellPartId))
                        {
                            //if (modEntry.spellPartManager.GetContentPackSpellParts()[contingencySpellPartJson.spellPartId] != null)
                            //{
                            //    ISpellPart spellPart = modEntry.spellPartManager.GetContentPackSpellParts()[contingencySpellPartJson.spellPartId];
                            //    spellGrammerList.Add(spellPart);
                            //}

                            if (modEntry.spellPartManager.GetContentPackSpellParts().TryGetValue(contingencySpellPartJson.spellPartId, out ISpellPart spellPart))
                            {
                                spellGrammarList.Add(spellPart);
                            }
                        }
                        
                    }
                }

                //Spell is created from the populated shape group and grammar list
                Spells.Spell spell = new Spells.Spell(modEntry, shapeGroups, SpellStack.Of(spellGrammarList));

                //Sets the contingency type
                ContingencyType cType = ContingencyType.NONE;

                if (contingencySpellJson.type == "NONE")
                {
                    cType = ContingencyType.NONE;
                }
                else if (contingencySpellJson.type == "HEALTH")
                {
                    cType = ContingencyType.HEALTH;
                }
                else if (contingencySpellJson.type == "DAMAGE")
                {
                    cType = ContingencyType.DAMAGE;
                }

                //Creates the contingency
                Contingency contingency = new Contingency(modEntry, cType, spell, contingencySpellJson.index);
                //Contingency contingency = new Contingency();

                return contingency;
            }
            else
            {
                //modEntry.Monitor.Log("contingencySpellJson null", StardewModdingAPI.LogLevel.Info);
            }

            //If the initial contingency creation fails
            return new Contingency();
        }

        /// <summary>
        /// Saves the contingency to a json string
        /// </summary>
        /// <param name="contingency">The contengency object</param>
        /// <returns>A json string</returns>
        public string SaveContingencySpell(Contingency contingency)//, ISpell spell, ContingencyType type, int index)
        {
            //Create a list of shape group jsons to store
            List<ContingencyShapeGroupJson> shapeGroups = new List<ContingencyShapeGroupJson>();

            ISpell spell = contingency.Spell;
            ContingencyType type = contingency.Type;
            int index = contingency.Index;

            //Populate the shape group jsons
            for (int i = 0; i < spell.ShapeGroups().Count; i++)
            {
                shapeGroups.Add(new ContingencyShapeGroupJson());
            }

            //Create a grammar list json to store
            List<ContingencySpellPartJson> spellGrammerList = new List<ContingencySpellPartJson>();

            //spellPage.GetSpellShapes().Count
            //Goes through the spell's shape groups
            for (int i = 0; i < spell.ShapeGroups().Count; i++)
            {
                //Gets the shape group at the current index
                ShapeGroup shapeGroup = spell.ShapeGroups()[i];

                //Get the shape group spell parts
                List<ISpellPart> shapeGroupSpellParts = shapeGroup.Parts();

                //Create a list for spell part jsons
                List<ContingencySpellPartJson> contingencySpellParts = new List<ContingencySpellPartJson>();

                //Go through the spell parts in shapeGroupSpellParts
                foreach (ISpellPart spellParts in shapeGroupSpellParts)
                {
                    //Populate contingencySpellParts

                    ContingencySpellPartJson contingencySpellPart = new ContingencySpellPartJson
                    {
                        spellPartId = spellParts.GetId()
                    };

                    contingencySpellParts.Add(contingencySpellPart);
                }

                //Create a ContingencyShapeGroupJson object
                ContingencyShapeGroupJson contingencyShapeGroupJson = new ContingencyShapeGroupJson
                {
                    spellParts = contingencySpellParts
                };

                //Add the ContingencyShapeGroupJson object to the shapeGroups list
                shapeGroups[i] = contingencyShapeGroupJson;
            }

            //foreach (ISpellPart spellDraggable in spell.Parts().Where(x => x.GetType() == SpellPartType.COMPONENT || x.GetType() == SpellPartType.MODIFIER))
            //Go through the spell parts in spell's spell stack (gammar list)
            foreach (ISpellPart spellPart in spell.spellStack().Parts)
            {
                //Populate spellGrammerList

                ContingencySpellPartJson contingencySpellPartJson = new ContingencySpellPartJson
                {
                    spellPartId = spellPart.GetId()
                };

                spellGrammerList.Add(contingencySpellPartJson);
            }

            //Create the ContingencySpellJson object
            ContingencySpellJson contingencySpellJson = new ContingencySpellJson
            {
                shapeGroups = shapeGroups,
                spellGrammerList = spellGrammerList,
                name = spell.GetName(),
                type = type.ToString(),
                index = index
            };

            //Serialize the ContingencySpellJson object to a json string
            var jsonString = JsonConvert.SerializeObject(contingencySpellJson);

            //ModEntry.INSTANCE.Monitor.Log(jsonString, StardewModdingAPI.LogLevel.Info);

            return jsonString;
        }


        public record Contingency(ModEntry modEntry, ContingencyType Type, ISpell Spell, int Index)
        {
            //public static readonly Codec<Contingency> CODEC = RecordCodecBuilder.Create(inst => inst.Group(
            //        ContingencyType.CODEC.Xmap(t => ArsMagicaAPI.Get().GetContingencyTypeRegistry().GetKey(t),
            //            rl => ArsMagicaAPI.Get().GetContingencyTypeRegistry().Get(rl)).FieldOf("type").ForGetter(Contingency => Contingency.Type),
            //        ISpell.CODEC.FieldOf("spell").ForGetter(Contingency => Contingency.Spell),
            //        Codec.INT.FieldOf("index").ForGetter(Contingency => Contingency.Index)
            //    ).Apply(inst, Contingency.New));

            public Contingency() : this(ModEntry.INSTANCE, ContingencyType.NONE, ModEntry.INSTANCE.arsVeneficiAPILoader.GetAPI().MakeSpell(SpellStack.Empty), 0) { }

            public void Execute(GameLocation level, Character target)
            {
                modEntry.arsVeneficiAPILoader.GetAPI().GetSpellHelper().Invoke(modEntry, Spell, new CharacterEntityWrapper(target), level, new CharacterHitResult(target), 0, Index, true);
            }
        }
    }
}
