using System;
using System.Collections.Generic;
using System.IO;
using Assets.SimpleZip;
using Cultivation_Way.Core;
using Cultivation_Way.Library;
using Newtonsoft.Json;

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
    public List<BloodNodeAsset> bloods = new();
    public List<Cultibook> cultibooks = new();
    public Dictionary<string, EnergyTileData[,]> energy_tiles = new();

    public void initialize()
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
    }

    public string to_json()
    {
        return JsonConvert.SerializeObject(this);
    }

    public void save_to_as_json(string path)
    {
        File.WriteAllText(path, to_json());
    }

    public void save_to_as_zip(string path)
    {
        File.WriteAllBytes(path, Zip.Compress(to_json()));
    }

    public override void load_to_world(SaveManager save_manager, SavedMap origin_data)
    {
        save_manager.data = origin_data;
        if (save_manager.data.saveVersion < 12)
        {
            save_manager.convertOldAges();
        }

        if (save_manager.data.saveVersion < 13)
        {
            save_manager.checkOldBuildingID();
        }

        World.world.addClearWorld(origin_data.width, origin_data.height);
        SmoothLoader.add(delegate { clear_cw_world(); }, "Clearing CW Map");
        SmoothLoader.add(delegate
        {
            World.world.setMapSize(origin_data.width, origin_data.height);
            World.world.mapStats = origin_data.mapStats;
            World.world.mapStats.load();
            World.world.worldLaws = origin_data.worldLaws;
            World.world.eraManager.loadEra(origin_data.mapStats.era_id, origin_data.mapStats.era_next_id);
        }, "Setting Map Size");
        if (origin_data.saveVersion > 0 && origin_data.saveVersion < 8)
        {
            SmoothLoader.add(delegate { save_manager.loadTiles(origin_data.tileString); },
                "Loading Very Old Tiles. Like super old. Maybe you should like, re-save your world?");
        }
        else if (origin_data.saveVersion > 7)
        {
            SmoothLoader.add(delegate { save_manager.loadTileArray(origin_data); }, "Loading Tiles");
            SmoothLoader.add(delegate { save_manager.loadFrozen(origin_data.frozen_tiles); }, "Loading Frozen");
            SmoothLoader.add(delegate { save_manager.loadFire(origin_data.fire); }, "Loading Fires");
            SmoothLoader.add(delegate { save_manager.loadConway(origin_data.conwayEater, origin_data.conwayCreator); },
                "Loading Conway");
        }
        else
        {
            SmoothLoader.add(save_manager.loadOldTiles, "Loading Old Tiles");
        }

        SmoothLoader.add(delegate
        {
            CW_EnergyMapManager manager = CW_Core.mod_state.energy_map_manager;
            int width = World.world.tilesMap.GetLength(0);
            int height = World.world.tilesMap.GetLength(1);
            foreach (string energy_id in manager.maps.Keys)
            {
                manager.maps[energy_id].init(width, height);
                if (!energy_tiles.ContainsKey(energy_id)) continue;
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        manager.maps[energy_id].map[x, y].value = energy_tiles[energy_id][x, y].value;
                        manager.maps[energy_id].map[x, y].density = energy_tiles[energy_id][x, y].density;
                    }
                }
            }
        }, "Loading Energy Maps");
        SmoothLoader.add(delegate
        {
            foreach (BloodNodeAsset blood in bloods)
            {
                Manager.bloods.add(blood);
            }
        }, "Loading Blood Nodes");
        SmoothLoader.add(delegate
        {
            foreach (Cultibook cultibook in cultibooks)
            {
                Manager.cultibooks.add(cultibook);
            }
        }, "Loading Cultibooks");
        SmoothLoader.add(save_manager.loadCultures, "Loading Cultures");
        SmoothLoader.add(save_manager.loadKingdoms, "Loading Kingdoms");
        SmoothLoader.add(save_manager.loadCities, "Loading Cities");
        SmoothLoader.add(save_manager.loadActors, "Finish Loading Actors");
        SmoothLoader.add(save_manager.checkOldCityZones, "Check Old City Zones");
        SmoothLoader.add(save_manager.loadBuildings, "Load Buildings");
        SmoothLoader.add(save_manager.setHomeBuildings, "Set Home Buildings");
        SmoothLoader.add(save_manager.loadCivs02, "Loading Civs");
        SmoothLoader.add(save_manager.loadLeaders, "Loading Leaders");
        SmoothLoader.add(save_manager.loadClans, "Loading Clans");
        SmoothLoader.add(save_manager.loadAlliances, "Loading Alliances");
        SmoothLoader.add(save_manager.loadWars, "Loading Wars");
        SmoothLoader.add(save_manager.loadPlots, "Loading Plots");
        SmoothLoader.add(save_manager.loadDiplomacy, "Loading Diplomacy");
        World.world.addUnloadResources();
        SmoothLoader.add(delegate { World.world.mapChunkManager.allDirty(); }, "Map Chunk Manager (1/2)");
        SmoothLoader.add(delegate { World.world.mapChunkManager.update(0f, true); }, "Map Chunk Manager (2/2)");
        SmoothLoader.add(save_manager.loadBoatStates, "Loading Boats. Ahoy Ahoy");
        SmoothLoader.add(delegate { World.world.finishMakingWorld(); }, "Tidying Up the World");
        World.world.addKillAllUnits();
        SmoothLoader.add(delegate { save_manager.data = null; }, "Finishing up...", false, 0.2f);
    }
}