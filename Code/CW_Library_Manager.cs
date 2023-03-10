using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public abstract class CW_Asset_Library<T> : AssetLibrary<T> where T : Asset
    {
        internal abstract void register();
        public override T get(string pID)
        {
            if (pID == null) return null;
            T asset;
            if (this.dict.TryGetValue(pID, out asset)) return asset;
            throw new KeyNotFoundException("No found '"+pID+"' in this library");
        }
        public void shallow_copy(string new_id, string from_id)
        {
            this.dict.Add(new_id, this.get(from_id));
        }
        public bool has_asset(string id)
        {
            return !string.IsNullOrEmpty(id) && this.dict.ContainsKey(id);
        }
    }
    public abstract class CW_Dynamic_Library<T> : CW_Asset_Library<T> where T : Asset
    {
        internal List<T> static_list = new List<T>();
        public abstract T add_to_static(T asset);
        internal virtual void reset()
        {
            this.dict.Clear();
            this.list.Clear();
            foreach (T asset in static_list)
            {
                add(asset);
            }
        }
        internal virtual void load_as(List<T> list)
        {
            this.list.Clear();
            this.dict.Clear();
            foreach (T asset in list)
            {
                this.add(asset);
            }
        }
    }
    public class CW_Library_Manager
    {
        public static CW_Library_Manager instance;
        public List<BaseAssetLibrary> extended_libraries;
        public Dictionary<string, BaseAssetLibrary> extended_libraries_dict;

        public CW_Library_ActorStats units;
        public CW_Library_Building buildings;
        public CW_Library_CultiBook cultibooks;
        public CW_Library_CultiSys cultisys;
        public CW_Library_Element elements;
        public CW_Library_Energy energy_types;
        internal CW_Library_Family families;
        public CW_Library_Item items;
        public CW_Library_ItemAccessoryMaterial item_accessory_materials;
        public CW_Library_ItemArmorMaterial item_armor_materials;
        public CW_Library_ItemWeaponMaterial item_weapon_materials;
        public CW_Library_ItemModifier item_modifiers;
        public CW_Library_Kingdom kingdoms;
        public CW_Library_NameGenerator name_generators;
        public CW_Library_Race races;
        public CW_Library_SpecialBody special_bodies;
        public CW_Library_Spell spells;
        public CW_Library_StatusEffect status_effects;
        public CW_Library_Trait traits;
        public CW_Library_WorldEvent events;
        public CW_Library_Words words_libraries;
        
        internal static CW_Library_Manager create()
        {
            instance = new CW_Library_Manager();
            return instance;
        }
        internal void init()
        {
            extended_libraries = new List<BaseAssetLibrary>();
            extended_libraries_dict = new Dictionary<string, BaseAssetLibrary>();
            units = new CW_Library_ActorStats();
            buildings = new CW_Library_Building();
            cultibooks = new CW_Library_CultiBook();
            cultisys = new CW_Library_CultiSys();
            elements = new CW_Library_Element();
            energy_types = new CW_Library_Energy();
            families = new CW_Library_Family();
            items = new CW_Library_Item();
            item_accessory_materials = new CW_Library_ItemAccessoryMaterial();
            item_armor_materials = new CW_Library_ItemArmorMaterial();
            item_weapon_materials = new CW_Library_ItemWeaponMaterial();
            item_modifiers = new CW_Library_ItemModifier();
            kingdoms = new CW_Library_Kingdom();
            name_generators = new CW_Library_NameGenerator();
            races = new CW_Library_Race();
            special_bodies = new CW_Library_SpecialBody();
            spells = new CW_Library_Spell();
            status_effects = new CW_Library_StatusEffect();
            traits = new CW_Library_Trait();
            events = new CW_Library_WorldEvent();
            words_libraries = new CW_Library_Words();
            units.init();
            buildings.init();
            cultibooks.init();
            cultisys.init();
            elements.init();
            energy_types.init();
            families.init();
            items.init();
            item_accessory_materials.init();
            item_armor_materials.init();
            item_weapon_materials.init();
            item_modifiers.init();
            kingdoms.init();
            name_generators.init();
            races.init();
            special_bodies.init();
            spells.init();
            status_effects.init();
            traits.init();
            events.init();
            words_libraries.init();
        }
        /// <summary>
        /// ??????AssetLibrary??????????????????
        /// </summary>
        /// <param name="id"></param>
        /// <param name="library"></param>
        /// <exception cref="Exception"></exception>
        public void add_library(string id, BaseAssetLibrary library)
        {
            if (extended_libraries_dict.ContainsKey(id)) throw new Exception(String.Format("Repeated Library, id:{0}", id));
            library.id = id;
            extended_libraries.Add(library);
            extended_libraries_dict.Add(id, library);
            library.init();
        }
        internal void register()
        {
            cultisys.register();
            elements.register();
            energy_types.register();
            name_generators.register();
            spells.register();
            /**
            units.register();
            buildings.register();
            cultibooks.register();
            families.register();
            items.register();
            item_accessory_materials.register();
            item_armor_materials.register();
            item_weapon_materials.register();
            kingdoms.register();
            races.register();
            special_bodies.register();
            status_effects.register();
            traits.register();
            events.register();
            words_libraries.register();
             */
        }
    }
}
