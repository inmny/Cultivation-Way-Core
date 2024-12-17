using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.SimpleZip;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Utils;

namespace Cultivation_Way.Save;

internal class EnergyTileData
{
    public float density;
    public float value;
}

// 在0.22中, 只需要存储额外的数据
[Serializable]
internal class SavedDataVerCur : AbstractSavedData
{
    public Dictionary<string, ActorAdditionSave> actor_addition_savedata = new();
    public List<BloodNodeAsset>                  bloods                 = new();
    public Dictionary<string, CityAdditionSave>  city_addition_savedata = new();
    public List<Cultibook>                       cultibooks             = new();
    public Dictionary<string, EnergyTileData[,]> energy_tiles           = new();

    public void Initialize(SavedMap pSavedMap)
    {
        bloods.AddRange(Manager.bloods.list);
        cultibooks.AddRange(Manager.cultibooks.list);
        foreach (var energy_map in CW_Core.mod_state.energy_map_manager.maps.Values)
        {
            energy_tiles.Add(energy_map.energy.id, new EnergyTileData[CW_Core.mod_state.energy_map_manager.width,
                CW_Core.mod_state.energy_map_manager.height]);
            for (int x = 0; x < CW_Core.mod_state.energy_map_manager.width; x++)
            {
                for (int y = 0; y < CW_Core.mod_state.energy_map_manager.height; y++)
                {
                    energy_tiles[energy_map.energy.id][x, y] = new EnergyTileData
                    {
                        density = energy_map.map[x, y].density,
                        value = energy_map.map[x, y].value
                    };
                }
            }
        }

        foreach (var data in pSavedMap.actors_data.Where(data => data.items != null && data.items.Count != 0))
            actor_addition_savedata.Add(data.id, new ActorAdditionSave
            {
                CW_Items = data.items.Where(item => item is CW_ItemData).Cast<CW_ItemData>().ToList()
            });

        foreach (var data in pSavedMap.cities.Where(data => data.storage != null))
        {
            city_addition_savedata.Add(data.id, new CityAdditionSave());
            foreach (var equipment_type in Enum.GetValues(typeof(EquipmentType)))
                city_addition_savedata[data.id].CW_Equipments.Add((EquipmentType)equipment_type,
                    data.storage.items_dicts[(EquipmentType)equipment_type]
                        .Where(item => item is CW_ItemData).Cast<CW_ItemData>().ToList());
        }

        foreach (var blood in bloods
                     .Where(blood => blood.ancestor_data.items != null && blood.ancestor_data.items.Count != 0)
                     .Where(blood => !actor_addition_savedata.ContainsKey(blood.ancestor_data.id)))
            actor_addition_savedata.Add(blood.ancestor_data.id, new ActorAdditionSave
            {
                CW_Items = blood.ancestor_data.items.Where(item => item is CW_ItemData).Cast<CW_ItemData>().ToList()
            });
    }

    public string to_json()
    {
        return GeneralHelper.to_json(this, true);
    }

    public void save_to_as_json(string path)
    {
        File.WriteAllText(path, to_json());
    }

    public void save_to_as_zip(string path)
    {
        File.WriteAllBytes(path, Zip.Compress(to_json()));
    }

    public override void LoadWorld()
    {
        foreach (var blood in bloods)
        {
            Manager.bloods.add(blood);
        }

        bloods = null;
        foreach (var cultibook in cultibooks) Manager.cultibooks.add(cultibook);

        cultibooks = null;

        CW_Core.mod_state.energy_map_manager.replace_new_map(energy_tiles, MapBox.width, MapBox.height);

        energy_tiles = null;
        actor_addition_savedata = null;
        city_addition_savedata = null;
    }

    public override void BeforeAll(SaveManager pSaveManager, SavedMap pVanillaData)
    {
        base.BeforeAll(pSaveManager, pVanillaData);

        AllStatsAfterDeserialization();

        // Convert Actor Equipment and City Equipments
        foreach (var data in pVanillaData.actors_data)
        {
            if (data.items == null || data.items.Count == 0) continue;
            UpdateActorEquipment(data);
        }

        foreach (var blood in bloods)
        {
            if (blood.ancestor_data.items == null || blood.ancestor_data.items.Count == 0) continue;
            UpdateActorEquipment(blood.ancestor_data);
        }

        foreach (var data in pVanillaData.cities)
        {
            if (!city_addition_savedata.TryGetValue(data.id, out var addition_save)) continue;

            data.storage.init(null);
            foreach (var equipment_type in Enum.GetValues(typeof(EquipmentType)))
            {
                if (!addition_save.CW_Equipments.TryGetValue((EquipmentType)equipment_type, out var equipments))
                    continue;
                foreach (var item in equipments.Where(item => Manager.items.contains(item.id)))
                    ReplaceItemData(data.storage.items_dicts[(EquipmentType)equipment_type], item);
            }
        }
    }

    public override void AfterAll(SaveManager pSaveManager)
    {
        base.AfterAll(pSaveManager);
        var actors = World.world.units.getSimpleList();
        foreach (var actor in actors)
        {
            CW_Actor cw_actor = actor.CW();
            cw_actor.data_spells.Clear();
            cw_actor.data_spells.UnionWith(cw_actor.data.GetSpells());
            CountCultisysLevel(actor, CultisysType.BODY);
            CountCultisysLevel(actor, CultisysType.SOUL);
            CountCultisysLevel(actor, CultisysType.BLOOD);
            CountCultisysLevel(actor, CultisysType.WAKAN);
            CountCultisysLevel(actor, CultisysType.HIDDEN);
        }

        void CountCultisysLevel(Actor pActor, CultisysType pType)
        {
            var cultisys = pActor.data.GetCultisys(pType);
            if (cultisys == null) return;
            pActor.data.get(cultisys.id, out int level);
            cultisys.number_per_level[level]++;
        }
    }

    private void UpdateActorEquipment(ActorData pData)
    {
        if (!actor_addition_savedata.TryGetValue(pData.id, out var addition_save)) return;

        foreach (var item in addition_save.CW_Items)
        {
            if (!Manager.items.contains(item.id)) continue;
            ReplaceItemData(pData.items, item);
        }
    }

    private void AllStatsAfterDeserialization()
    {
        foreach (var blood in bloods) blood.ancestor_stats.AfterDeserialize();

        foreach (var cultibook in cultibooks) cultibook.bonus_stats.AfterDeserialize();

        foreach (var item in actor_addition_savedata.Values.SelectMany(addition_data => addition_data.CW_Items))
            item.addition_stats.AfterDeserialize();

        foreach (var item in from addition_data in city_addition_savedata.Values
                             from equipments in addition_data.CW_Equipments.Values
                             from item in equipments
                             select item)
            item.addition_stats.AfterDeserialize();
    }

    private static void ReplaceItemData(IList<ItemData> pItems, ItemData pNewData)
    {
        for (var i = 0; i < pItems.Count; i++)
        {
            if (pItems[i].id != pNewData.id || pItems[i].material != pNewData.material ||
                pItems[i] is CW_ItemData) continue;
            pItems[i] = pNewData;
            return;
        }
    }

    public class ActorAdditionSave
    {
        public List<CW_ItemData> CW_Items = new();
    }

    public class CityAdditionSave
    {
        public Dictionary<EquipmentType, List<CW_ItemData>> CW_Equipments = new();
    }
}