using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_Item : ItemAsset
    {
        public CW_BaseStats addition_stats;
        public CW_Asset_Item() { }
        public CW_Asset_Item(ItemAsset origin_asset)
        {
            this.id = origin_asset.id;
            this.addition_stats = new CW_BaseStats();
            this.attackAction = origin_asset.attackAction;
            this.attackType = origin_asset.attackType;
            this.baseStats = origin_asset.baseStats;
            this.cached_sprite = origin_asset.cached_sprite;
            this.cost_gold = origin_asset.cost_gold;
            this.cost_resource_1 = origin_asset.cost_resource_1;
            this.cost_resource_2 = origin_asset.cost_resource_2;
            this.cost_resource_id_1 = origin_asset.cost_resource_id_1;
            this.cost_resource_id_2 = origin_asset.cost_resource_id_2;
            this.equipmentType = origin_asset.equipmentType;
            this.equipment_value = origin_asset.equipment_value;
            this.materials = origin_asset.materials;
            this.minimum_city_storage_resource_1 = origin_asset.minimum_city_storage_resource_1;
            this.mod_rank = origin_asset.mod_rank;
            this.mod_translation = origin_asset.mod_translation;
            this.mod_type = origin_asset.mod_type;
            this.name_class = origin_asset.name_class;
            this.name_templates = origin_asset.name_templates;
            this.path_icon = origin_asset.path_icon;
            this.pool = origin_asset.pool;
            this.projectile = origin_asset.projectile;
            this.quality = origin_asset.quality;
            this.rarity = origin_asset.rarity;
            this.slash = origin_asset.slash;
            this.tech_needed = origin_asset.tech_needed;
        }
        public CW_Asset_Item(ItemAsset origin_asset, CW_BaseStats addition_stats)
        {
            this.id = origin_asset.id;
            this.addition_stats = addition_stats;
            this.attackAction = origin_asset.attackAction;
            this.attackType = origin_asset.attackType;
            this.baseStats = origin_asset.baseStats;
            this.cached_sprite = origin_asset.cached_sprite;
            this.cost_gold = origin_asset.cost_gold;
            this.cost_resource_1 = origin_asset.cost_resource_1;
            this.cost_resource_2 = origin_asset.cost_resource_2;
            this.cost_resource_id_1 = origin_asset.cost_resource_id_1;
            this.cost_resource_id_2 = origin_asset.cost_resource_id_2;
            this.equipmentType = origin_asset.equipmentType;
            this.equipment_value = origin_asset.equipment_value;
            this.materials = origin_asset.materials;
            this.minimum_city_storage_resource_1 = origin_asset.minimum_city_storage_resource_1;
            this.mod_rank = origin_asset.mod_rank;
            this.mod_translation = origin_asset.mod_translation;
            this.mod_type = origin_asset.mod_type;
            this.name_class = origin_asset.name_class;
            this.name_templates = origin_asset.name_templates;
            this.path_icon = origin_asset.path_icon;
            this.pool = origin_asset.pool;
            this.projectile = origin_asset.projectile;
            this.quality = origin_asset.quality;
            this.rarity = origin_asset.rarity;
            this.slash = origin_asset.slash;
            this.tech_needed = origin_asset.tech_needed;
        }
    }
}
