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
    }
    public abstract class CW_Dynamic_Library<T> : CW_Asset_Library<T> where T : Asset
    {
        internal List<T> static_list;
        internal abstract void reset();
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
        internal CW_Library_Family families;
        public CW_Library_Item items;
        public CW_Library_ItemAccessoryMaterial item_accessory_materials;
        public CW_Library_ItemArmorMaterial item_armor_materials;
        public CW_Library_ItemWeaponMaterial item_weapon_materials;
        public CW_Library_ItemModifier item_modifiers;
        public CW_Library_Kingdom kingdoms;
        public CW_Library_Race races;
        public CW_Library_SpecialBody special_bodies;
        public CW_Library_Spell spells;
        public CW_Library_Trait traits;
        public CW_Library_WorldEvent events;
        
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
            families = new CW_Library_Family();
            items = new CW_Library_Item();
            item_accessory_materials = new CW_Library_ItemAccessoryMaterial();
            item_armor_materials = new CW_Library_ItemArmorMaterial();
            item_weapon_materials = new CW_Library_ItemWeaponMaterial();
            item_modifiers = new CW_Library_ItemModifier();
            kingdoms = new CW_Library_Kingdom();
            races = new CW_Library_Race();
            special_bodies = new CW_Library_SpecialBody();
            spells = new CW_Library_Spell();
            traits = new CW_Library_Trait();
            events = new CW_Library_WorldEvent();
            units.init();
            buildings.init();
            cultibooks.init();
            cultisys.init();
            elements.init();
            families.init();
            items.init();
            item_accessory_materials.init();
            item_armor_materials.init();
            item_weapon_materials.init();
            item_modifiers.init();
            kingdoms.init();
            races.init();
            special_bodies.init();
            spells.init();
            traits.init();
            events.init();
        }
        /// <summary>
        /// 添加AssetLibrary并进行初始化
        /// </summary>
        /// <param name="id"></param>
        /// <param name="library"></param>
        /// <exception cref="Exception"></exception>
        public void add_library(string id, BaseAssetLibrary library)
        {
            if (extended_libraries_dict.ContainsKey(id)) throw new Exception(String.Format("Repeated Library, id:{0}", id));
            extended_libraries.Add(library);
            extended_libraries_dict.Add(id, library);
            library.init();
        }
        internal void register()
        {
            /**
            units.register();
            buildings.register();
            cultibooks.register();*/
            cultisys.register();/**
            elements.register();
            families.register();
            items.register();
            item_accessory_materials.register();
            item_armor_materials.register();
            item_weapon_materials.register();
            kingdoms.register();
            races.register();
            special_bodies.register();
            spells.register();
            traits.register();
            events.register();*/
        }
    }
}
