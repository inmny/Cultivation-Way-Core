using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
namespace Cultivation_Way.Utils
{
    public class CW_ItemTools
    {
        public static CW_BaseStats s_cw_stats = new CW_BaseStats();
        public static int s_value = 0;
        public static ItemQuality s_quality;
		public static HashSet<string> unique_legendary_names = new HashSet<string>(Others.CW_Constants.max_unique_legendary_names_count);
		public static CW_Library_ItemAccessoryMaterial accessory_materials;
		public static CW_Library_ItemArmorMaterial armor_materials;
		public static CW_Library_ItemModifier modifiers;
		public static CW_Library_ItemWeaponMaterial weapon_materials;
		public static CW_Library_Item templates;
		internal static void init()
        {
			accessory_materials = CW_Library_Manager.instance.item_accessory_materials;
			armor_materials = CW_Library_Manager.instance.item_armor_materials;
			modifiers = CW_Library_Manager.instance.item_modifiers;
			weapon_materials = CW_Library_Manager.instance.item_weapon_materials;
			templates = CW_Library_Manager.instance.items;
        }
		public static string item_to_string(ItemData pData)
        {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("ID:\t" + pData.id);
			stringBuilder.AppendLine("Name:\t"+pData.name);
			//TODO
			return stringBuilder.ToString();
        }
		public static void calc_item_values(CW_ItemData pData)
        {
			s_cw_stats.clear();
			s_value = 0;
			s_quality = ItemQuality.Normal;
			checkStat(getItemMaterialLibrary(templates.get(pData.id).origin_asset.equipmentType, pData.material));
			checkStat(templates.get(pData.id));
			foreach (string text in pData.modifiers)
			{
				checkStat(modifiers.get(text));
			}
			//throw new NotImplementedException();
		}

        private static CW_Asset_Item getItemMaterialLibrary(EquipmentType equipment_type, string material)
        {
			switch (equipment_type)
			{
				case EquipmentType.Weapon:
					return weapon_materials.get(material);
				case EquipmentType.Helmet:
				case EquipmentType.Armor:
				case EquipmentType.Boots:
					return armor_materials.get(material);
				case EquipmentType.Ring:
				case EquipmentType.Amulet:
					return accessory_materials.get(material);
			}
			throw new Exception("No match Equipment Type for '" + (int)equipment_type + "'");
		}

        private static void checkStat(CW_Asset_Item cw_item_asset)
        {
			if (cw_item_asset.origin_asset.quality > s_quality)
			{
				s_quality = cw_item_asset.origin_asset.quality;
			}
			s_cw_stats.addStats(cw_item_asset.cw_base_stats);
			s_value += cw_item_asset.origin_asset.equipment_value + cw_item_asset.origin_asset.mod_rank * 5;
		}

        public static bool try_add_mod(ItemData item_data, CW_Asset_Item item_asset)
        {
			foreach(string modifier in item_data.modifiers)
            {
				if (AssetManager.items_modifiers.get(modifier).mod_type == item_asset.origin_asset.mod_type) return false;
            }
			item_data.modifiers.Add(item_asset.id);
			return true;
        }
        public static CW_ItemData generate_item(ItemAsset pItemAsset, string pMaterial, int pYear, string pWhere, string pWho, int pTries, ActorBase pActor)
        {
            CW_ItemData item_data = new CW_ItemData();
			// 获取待选词缀
			List<CW_Asset_Item> list;
			switch (pItemAsset.equipmentType)
			{
				case EquipmentType.Weapon:
					list = modifiers.pools["weapon"];
					break;
				case EquipmentType.Helmet:
				case EquipmentType.Armor:
				case EquipmentType.Boots:
					list = modifiers.pools["armor"];
					break;
				case EquipmentType.Ring:
				case EquipmentType.Amulet:
					list = modifiers.pools["accessory"];
					break;
				default:
					list = modifiers.pools["armor"];
					break;
			}
			bool legendary_modifier = false;
			for (int i = 0; i < 5; i++)
			{
				if (!Toolbox.randomBool())
				{
					CW_Asset_Item random = list.GetRandom();
					if (!(random.id == "normal") && try_add_mod(item_data, random) && random.origin_asset.quality == ItemQuality.Legendary) legendary_modifier = true;
				}
			}
			if (legendary_modifier)
			{
				int num = 0; item_data.name = null;
				while (string.IsNullOrEmpty(item_data.name) || unique_legendary_names.Contains(item_data.name))
				{
					item_data.name = NameGenerator.generateNameFromTemplate(AssetManager.nameGenerator.get(pItemAsset.getRandomNameTemplate(pActor)), pActor);
					if (++num > Others.CW_Constants.max_unique_legendary_names_count) unique_legendary_names.Clear();
				}
			}
			item_data.id = pItemAsset.id;
            item_data.material = pMaterial;
            item_data.year = pYear;
            item_data.by = pWho;
            item_data.from = pWhere;
			return item_data;
        }
    }
}
