using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Utils
{
    public class CW_ItemTools
    {
        public static CW_BaseStats s_cw_stats = new CW_BaseStats();
        public static int s_value = 0;
        public static ItemQuality s_quality;
		public static HashSet<string> unique_legendary_names = new HashSet<string>(Others.CW_Constants.max_unique_legendary_names_count);
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
            //throw new NotImplementedException();
        }
		public static bool try_add_mod(ItemData item_data, ItemAsset item_asset)
        {
			foreach(string modifier in item_data.modifiers)
            {
				if (AssetManager.items_modifiers.get(modifier).mod_type == item_asset.mod_type) return false;
            }
			item_data.modifiers.Add(item_asset.id);
			return true;
        }
        public static CW_ItemData generate_item(ItemAsset pItemAsset, string pMaterial, int pYear, string pWhere, string pWho, int pTries, ActorBase pActor)
        {
            CW_ItemData item_data = new CW_ItemData();
			// 获取待选词缀
			List<ItemAsset> list;
			switch (pItemAsset.equipmentType)
			{
				case EquipmentType.Weapon:
					list = AssetManager.items_modifiers.pools["weapon"];
					break;
				case EquipmentType.Helmet:
				case EquipmentType.Armor:
				case EquipmentType.Boots:
					list = AssetManager.items_modifiers.pools["armor"];
					break;
				case EquipmentType.Ring:
				case EquipmentType.Amulet:
					list = AssetManager.items_modifiers.pools["accessory"];
					break;
				default:
					list = AssetManager.items_modifiers.pools["armor"];
					break;
			}
			bool legendary_modifier = false;
			for (int i = 0; i < 5; i++)
			{
				if (!Toolbox.randomBool())
				{
					ItemAsset random = list.GetRandom();
					if (!(random.id == "normal") && try_add_mod(item_data, random) && random.quality == ItemQuality.Legendary) legendary_modifier = true;
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
