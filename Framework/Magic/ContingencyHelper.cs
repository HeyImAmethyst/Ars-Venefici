using ArsVenefici.Framework.GUI.DragNDrop;
using ArsVenefici.Framework.Interfaces;
using ArsVenefici.Framework.Interfaces.Magic;
using ArsVenefici.Framework.Interfaces.Spells;
using ArsVenefici.Framework.Spell;
using ArsVenefici.Framework.Spell.Shape;
using ArsVenefici.Framework.Util;
using ItemExtensions;
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

    public class ContingencySpellPartJson
    {
        [JsonProperty("Spell Part")]
        public string spellPartId { get; set; }
    }

    public class ContingencyShapeGroupJson
    {
        [JsonProperty("Contents")]
        public List<ContingencySpellPartJson> spellParts { get; set; }
    }

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

        public void SetContingency(Character target, ContingencyType type, ISpell spell)
        {
            if (target != null)
            {
                target.modData.SetCustom(ContingencyHelper.ContingencyKey, new Contingency(ModEntry.INSTANCE, type, spell, 1), SaveContingencySpell);
            }
        }


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

        public void ClearContingency(Character target)
        {
            if (target != null)
            {
                //target.modData.SetCustom(ContingencyHelper.ContingencyKey, new Contingency(), SaveContingencySpell);
                target.modData.Remove(ContingencyHelper.ContingencyKey);
            }
        }

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

        public Contingency ReadContingencySpell(string jsonString, ModEntry modEntry)
        {
            //modEntry.Monitor.Log(jsonString, StardewModdingAPI.LogLevel.Info);

            //ContingencySpellJson contingencySpellJson = (ContingencySpellJson)JsonConvert.DeserializeObject(jsonString);

            //JObject jobject = JObject.Parse(jsonString);
            //ContingencySpellJson contingencySpellJson = jobject.SelectToken(ContingencyHelper.ContingencyKey)?.ToObject<ContingencySpellJson>();

            //dynamic contingencySpellJson = JsonConvert.DeserializeObject(jsonString);

            ContingencySpellJson contingencySpellJson = JsonConvert.DeserializeObject<ContingencySpellJson>(jsonString);

            if (contingencySpellJson != null)
            {
                //modEntry.Monitor.Log("contingencySpellJson not null", StardewModdingAPI.LogLevel.Info);

                List<ShapeGroup> shapeGroups = new List<ShapeGroup>();

                for (int i = 0; i < contingencySpellJson.shapeGroups.Count; i++)
                {
                    shapeGroups.Add(ShapeGroup.EMPTY);
                }

                List<ISpellPart> spellGrammerList = new List<ISpellPart>();

                for (int i = 0; i < contingencySpellJson.shapeGroups.Count; i++)
                {
                    ContingencyShapeGroupJson contingencyShapeGroup = contingencySpellJson.shapeGroups[i];

                    List<ContingencySpellPartJson> contingencyShapeGroupSpellPartJsons = contingencyShapeGroup.spellParts;
                    List<ISpellPart> contingencyShapeGroupSpellParts = new List<ISpellPart>();

                    //modEntry.Monitor.Log(JsonConvert.SerializeObject(shapeGroupAreaJson, Formatting.Indented), LogLevel.Info);

                    foreach (ContingencySpellPartJson contingencySpellPart in contingencyShapeGroupSpellPartJsons)
                    {
                        ISpellPart spellPart = modEntry.spellPartManager.spellParts[contingencySpellPart.spellPartId];
                        contingencyShapeGroupSpellParts.Add(spellPart);
                    }

                    ShapeGroup shapeGroup = ShapeGroup.Of(contingencyShapeGroupSpellParts);

                    shapeGroups[i] = shapeGroup;
                }

                foreach (ContingencySpellPartJson contingencySpellPartJson in contingencySpellJson.spellGrammerList)
                {
                    ISpellPart spellPart = modEntry.spellPartManager.spellParts[contingencySpellPartJson.spellPartId];
                    spellGrammerList.Add(spellPart);
                }

                Spell.Spell spell = new Spell.Spell(modEntry, shapeGroups, SpellStack.Of(spellGrammerList));

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

                Contingency contingency = new Contingency(modEntry, cType, spell, contingencySpellJson.index);
                //Contingency contingency = new Contingency();

                return contingency;
            }
            else
            {
                //modEntry.Monitor.Log("contingencySpellJson null", StardewModdingAPI.LogLevel.Info);
            }

            return new Contingency();
        }

        public string SaveContingencySpell(Contingency contingency)//, ISpell spell, ContingencyType type, int index)
        {
            List<ContingencyShapeGroupJson> shapeGroups = new List<ContingencyShapeGroupJson>();

            ISpell spell = contingency.Spell;
            ContingencyType type = contingency.Type;
            int index = contingency.Index;

            for (int i = 0; i < spell.ShapeGroups().Count; i++)
            {
                shapeGroups.Add(new ContingencyShapeGroupJson());
            }

            List<ContingencySpellPartJson> spellGrammerList = new List<ContingencySpellPartJson>();

            //spellPage.GetSpellShapes().Count
            for (int i = 0; i < spell.ShapeGroups().Count; i++)
            {
                ShapeGroup shapeGroup = spell.ShapeGroups()[i];

                List<ISpellPart> shapeGroupSpellParts = shapeGroup.Parts();

                List<ContingencySpellPartJson> contingencySpellParts = new List<ContingencySpellPartJson>();

                foreach (ISpellPart spellParts in shapeGroupSpellParts)
                {
                    ContingencySpellPartJson contingencySpellPart = new ContingencySpellPartJson
                    {
                        spellPartId = spellParts.GetId()
                    };

                    contingencySpellParts.Add(contingencySpellPart);
                }

                ContingencyShapeGroupJson contingencyShapeGroupJson = new ContingencyShapeGroupJson
                {
                    spellParts = contingencySpellParts
                };

                shapeGroups[i] = contingencyShapeGroupJson;
            }

            //foreach (ISpellPart spellDraggable in spell.Parts().Where(x => x.GetType() == SpellPartType.COMPONENT || x.GetType() == SpellPartType.MODIFIER))
            foreach (ISpellPart spellPart in spell.spellStack().Parts)
            {

                ContingencySpellPartJson contingencySpellPartJson = new ContingencySpellPartJson
                {
                    spellPartId = spellPart.GetId()
                };

                spellGrammerList.Add(contingencySpellPartJson);
            }

            ContingencySpellJson contingencySpellJson = new ContingencySpellJson
            {
                shapeGroups = shapeGroups,
                spellGrammerList = spellGrammerList,
                name = spell.GetName(),
                type = type.ToString(),
                index = index
            };

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

            public Contingency() : this(ModEntry.INSTANCE, ContingencyType.NONE, SpellHelper.Instance().makeSpell(SpellStack.Empty), 0) { }

            public void Execute(GameLocation level, Character target)
            {
                SpellHelper.Instance().Invoke(modEntry, Spell, new CharacterEntityWrapper(target), level, new CharacterHitResult(target), 0, Index, true);
            }
        }
    }
}
