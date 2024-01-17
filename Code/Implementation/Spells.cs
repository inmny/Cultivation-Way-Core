using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Cultivation_Way.Animation;
using Cultivation_Way.Constants;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Cultivation_Way.Utils;
using Cultivation_Way.Utils.General.AboutSpell;
using NeoModLoader.api.attributes;
using Newtonsoft.Json;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Cultivation_Way.Implementation;

internal static class Spells
{
    public static void init()
    {
        // TODO: 调整法术消耗
        add_track_projectile_spells();
        add_give_self_status_spells();
        add_blade_spells();
        add_escape_spells();
        //add_call_spells();

        add_regen_spell();
        add_tornado_spell();

        add_fall_rock();
        add_fall_wood();
        add_fall_mountain("heng1");
        add_fall_mountain("heng2");
        add_fall_mountain("hua");
        add_fall_mountain("tai");
        add_fall_mountain("song");


        add_fen_fire_spell();
        add_loltus_fire_spell();
        add_samadhi_fire_spell();
        add_void_fire_spell();

        add_fire_polo_spell();
        add_water_polo_spell();
        add_wind_polo_spell();
        add_lightning_polo_spell();

        add_wood_thorn_spell();
        add_ground_thorn_spell();

        add_ice_bound_spell();
        add_landificate_spell();
        add_vine_bound_spell();

        add_default_lightning_spell();
        add_positive_quintuple_lightning_spell();
        add_negative_quintuple_lightning_spell();

        add_violet_gold_gourd_spell();
    }

    private static void add_violet_gold_gourd_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.NO_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 8f,
            free_val = 40f,
            layer_name = "Objects",
            frame_action = [Hotfixable](int idx, ref Vector2 vec, ref Vector2 dst_vec,
                Animation.SpriteAnimation anim) =>
            {
                if (anim.data.hasFlag("go_back")) return;
                if (anim.dst_object == null || !anim.dst_object.isActor() || !anim.dst_object.isAlive())
                {
                    anim.data.addFlag("go_back");
                    return;
                }

                var dst_actor = (CW_Actor)anim.dst_object;
                var curr_pos = anim.gameObject.transform.localPosition;
                if (!anim.data.hasFlag("stop_scale_up"))
                {
                    anim.change_scale(1.02f);
                    if (anim.get_scale() > 1.07f) anim.data.addFlag("stop_scale_up");
                }

                if (!anim.data.hasFlag("stop_rotate"))
                {
                    var target_z_angle =
                        Toolbox.getAngle(curr_pos.x, curr_pos.y, dst_actor.currentPosition.x,
                            dst_actor.currentPosition.y + dst_actor.zPosition.y) *
                        57.29578f - 45;
                    if (target_z_angle < 0) target_z_angle += 360;
                    var current_z_angle = anim.gameObject.transform.rotation.eulerAngles.z;
                    var delta_z_angle = target_z_angle - current_z_angle;
                    if (delta_z_angle > 180) delta_z_angle -= 360;
                    if (delta_z_angle < -180) delta_z_angle += 360;
                    if (Mathf.Abs(delta_z_angle) < 0.1f)
                        anim.data.addFlag("stop_rotate");
                    else
                        anim.gameObject.transform.Rotate(0, 0, delta_z_angle * 0.1f);
                }

                if (!anim.data.hasFlag("started")) return;

                dst_actor.is_in_magnet = true;

                dst_actor.currentPosition =
                    new Vector2(Mathf.Lerp(anim.src_object.currentPosition.x, curr_pos.x, anim.cur_elapsed * 10),
                        Mathf.Lerp(anim.src_object.currentPosition.y, curr_pos.y - anim.setting.free_val,
                            anim.cur_elapsed * 10));
                dst_actor.zPosition.y = Mathf.Lerp(dst_actor.zPosition.y, anim.setting.free_val, anim.cur_elapsed * 10);

                dst_actor.findCurrentTile();
                dst_actor.transform.localPosition = new Vector3(dst_actor.currentPosition.x,
                    dst_actor.currentPosition.y + dst_actor.zPosition.y, dst_actor.zPosition.y);

                if (anim.data.hasFlag("refining"))
                {
                    anim.data.get(DataS.spell_cost, out var spell_cost, 1f);
                    spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);


                    anim.data.get("shake_time", out float shake_time, -1);
                    anim.data.get("shake_dir", out var shake_dir, 1);
                    if (shake_time <= 0)
                    {
                        shake_dir = -shake_dir;
                        shake_time = 1;
                    }

                    shake_time -= anim.cur_elapsed;
                    anim.data.set("shake_time", shake_time);
                    anim.data.set("shake_dir", shake_dir);
                    anim.gameObject.transform.Rotate(0, 0, shake_time * shake_dir);

                    dst_actor.getHit(spell_cost, false, (AttackType)CW_AttackType.Spell, anim.src_object, false);

                    anim.data.get("refining_time", out float time);
                    time += anim.cur_elapsed;
                    if (time > 10)
                    {
                        anim.data.addFlag("go_back");
                        return;
                    }

                    anim.data.set("refining_time", time);
                    return;
                }

                if (Toolbox.Dist(curr_pos.x, curr_pos.y, dst_actor.currentPosition.x,
                        dst_actor.currentPosition.y + dst_actor.zPosition.y) < 3f)
                {
                    anim.data.addFlag("refining");
                    anim.data.addFlag("stop_rotate");
                    anim.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    CW_Core.LogInfo($"refining started {dst_actor.data.id}");
                }
            },
            end_action = [Hotfixable](int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.dst_object == null || !anim.dst_object.isActor() || !anim.dst_object.isAlive()) return;
                anim.dst_object.a.is_in_magnet = false;
            }
        };
        anim_setting.set_trace([Hotfixable](ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim,
            ref float delta_x, ref float delta_y) =>
        {
            var curr_pos = anim.gameObject.transform.position;
            if (anim.data.hasFlag("go_back"))
            {
                anim.change_scale(0.9f);
                if (anim.get_scale() < 0.01f) anim.force_stop();
                delta_x = (anim.src_object.currentPosition.x - curr_pos.x) * 3;
                delta_y = (anim.src_object.currentPosition.y - curr_pos.y) * 3;
                return;
            }

            dst_vec.x = anim.src_object.currentPosition.x;
            dst_vec.y = anim.src_object.currentPosition.y + anim_setting.free_val;

            if (!anim.data.hasFlag("prepare"))
            {
                anim.set_position(anim.src_object.currentPosition);
                anim.data.addFlag("prepare");
                return;
            }

            var dist = Toolbox.DistVec2Float(dst_vec, curr_pos);
            if (dist < 1f)
            {
                anim.data.addFlag("started");
                return;
            }

            delta_x = (dst_vec.x - curr_pos.x) * 3;
            delta_y = (dst_vec.y - curr_pos.y) * 3;
        });

        EffectManager.instance.load_as_controller("violet_gold_gourd_anim", "effects/violet_gold_gourd/",
            controller_setting: anim_setting, base_scale: 0.25f);
        CW_SpellAsset spell_asset = new()
        {
            id = "violet_gold_gourd",
            rarity = 99,
            element = new CW_Element(new[]
            {
                30, 30, 0, 40, 0
            }),
            anim_id = "violet_gold_gourd_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.fall_to_ground,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell_asset.add_trigger_tags(new[]
        {
            SpellTriggerTag.ATTACK, SpellTriggerTag.NAMED_DEFEND
        });
        Library.Manager.spells.add(spell_asset);
    }

    private static void add_call_spells()
    {
        CW_SpellAsset call_ancestor = new()
        {
            id = "call_ancestor",
            rarity = 99,
            anim_action = null,
            anim_id = "",
            anim_type = SpellAnimType.CUSTOM,
            cultisys_require = 0,
            element = new CW_Element(new[]
            {
                20, 20, 20, 20, 20
            }),
            spell_action = (spell_asset, user, target, tile, cost) =>
            {
                if (user.objectType != MapObjectType.Actor || user.city == null) return;
                CW_Actor cw_actor = (CW_Actor)user;
                cw_actor.data.get(DataS.main_blood_purity, out float purity);

                if (purity < Content_Constants.call_ancestor_min_purity) return;
                BloodNodeAsset main_blood = cw_actor.data.GetMainBlood();
                if (main_blood.id == cw_actor.data.id) return;
                CW_Actor ancestor_actor = (CW_Actor)World.world.units.get(main_blood.id);
                if (ancestor_actor != null)
                {
                    // 竞争权限
                }
                else
                {
                    // 此处不提取函数, 仅在此处使用
                    CW_ActorAsset cw_asset = Library.Manager.actors.get(main_blood.ancestor_data.asset_id);
                    if (cw_asset == null) return;
                    ActorAsset asset = cw_asset.vanllia_asset;
                    CW_Actor prefab = FastVisit.get_actor_prefab("actors/" + asset.prefab).GetComponent<CW_Actor>();
                    ancestor_actor = (CW_Actor)World.world.units.newObject(prefab);
                    ancestor_actor.setData(
                        JsonConvert.DeserializeObject<ActorData>(
                            JsonConvert.SerializeObject(main_blood.ancestor_data)));

                    ancestor_actor.cw_asset = cw_asset;
                    World.world.units.finalizeActor(asset.id, ancestor_actor, cw_actor.currentTile);
                    ancestor_actor.AddStatus("status_ancestor_called", user);
                }

                cw_actor.city.addNewUnit(ancestor_actor);
                if (!cw_actor.is_group_leader)
                {
                    if (cw_actor.unit_group != null)
                    {
                        cw_actor.removeFromGroup();
                    }

                    // 创建队伍, 并将祖先加入队伍
                    World.world.unitGroupManager.createNewGroup(cw_actor.city).addUnit(cw_actor);
                    Debug.Assert(cw_actor.unit_group != null, "cw_actor.unit_group != null");

                    cw_actor.unit_group.setGroupLeader(cw_actor);
                    cw_actor.unit_group.addUnit(ancestor_actor);
                }
            },
            spell_cost_action = (asset, user, target) => { return -1; },
            spell_learn_check = (asset, user, target) => { return 0; }
        };
        call_ancestor.add_trigger_tags(new[]
        {
            SpellTriggerTag.ATTACK, SpellTriggerTag.NAMED_DEFEND, SpellTriggerTag.UNNAMED_DEFEND
        });
        Library.Manager.spells.add(call_ancestor);
    }

    /// <summary>
    ///     添加简单的遁术
    /// </summary>
    private static void add_escape_spell(
        string spell_id, int rarity,
        int[] element,
        string anim_id, string anim_path,
        KeyValuePair<string, float>[] spell_cost_list,
        int tran_frame_idx
    )
    {
        CW_SpellAsset spell_asset = new()
        {
            id = spell_id,
            rarity = rarity,
            element = new CW_Element(element),
            anim_id = anim_id,
            anim_type = SpellAnimType.ON_USER,
            anim_action = AnimActions.on_something_auto_scale,
            target_camp = SpellTargetCamp.ALIAS,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(spell_cost_list),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell_asset.add_trigger_tags(new[]
        {
            SpellTriggerTag.NAMED_DEFEND, SpellTriggerTag.UNNAMED_DEFEND
        });
        spell_asset.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell_asset);

        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT,
            loop_nr_limit = 1,
            frame_interval = 0.05f,
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                if (idx != tran_frame_idx) return;
                WorldTile target = null;
                if (anim.src_object.city != null)
                {
                    // 优先回城
                    target = anim.src_object.city.getTile();
                }
                else
                {
                    int time_limit = 5;
                    while (time_limit-- > 0 && target == null)
                    {
                        TileIsland ground_island = World.world.islandsCalculator.getRandomIslandGround();
                        if (ground_island == null) continue;
                        MapRegion random = ground_island.regions.GetRandom();
                        target = random?.tiles.GetRandom();
                        if (target == null || target.Type.block || !target.Type.ground) target = null;
                    }
                }

                if (target == null) return;
                CW_Actor actor = (CW_Actor)anim.src_object;
                actor.spawnOn(target);
                actor.updatePos();
                anim.set_position(anim.src_object.currentPosition);
            }
        };
        anim_setting.set_trace(AnimationTraceType.ATTACH);
        CW_Core.mod_state.anim_manager.load_as_controller(
            anim_id, anim_path, controller_setting: anim_setting, base_scale: 1f
        );
    }


    /// <summary>
    ///     添加金刃相似法术
    /// </summary>
    private static void add_blade_spell(
        string spell_id, int rarity,
        int[] element,
        string anim_id, string anim_path,
        KeyValuePair<string, float>[] spell_cost_list,
        float range, AnimFrameAction anim_frame_action)
    {
        CW_SpellAsset spell_asset = new()
        {
            id = spell_id,
            rarity = rarity,
            element = new CW_Element(element),
            anim_id = anim_id,
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(spell_cost_list),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell_asset.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell_asset.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell_asset);

        AnimationSetting anim_setting = new()
        {
            point_to_dst = true,
            always_point_to_dst = false,
            loop_limit_type = AnimationLoopLimitType.TRACE_LIMIT,
            loop_trace_limit = range,
            anim_froze_frame_idx = 3,
            frame_interval = 0.05f,
            trace_grad = 30f,
            frame_action = anim_frame_action
        };
        anim_setting.set_trace(AnimationTraceType.LINE);
        CW_Core.mod_state.anim_manager.load_as_controller(
            anim_id, anim_path, controller_setting: anim_setting, base_scale: 0.08f
        );
    }

    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    private static void add_escape_spells()
    {
        add_escape_spell(
            "gold_escape", 5,
            new[]
            {
                0, 0, 0, 100, 0
            },
            "gold_escape_anim", "effects/gold_escape",
            new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            9
        );
        add_escape_spell(
            "ground_escape", 5,
            new[]
            {
                0, 0, 0, 0, 100
            },
            "ground_escape_anim", "effects/ground_escape",
            new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            9
        );
        add_escape_spell(
            "wood_escape", 5,
            new[]
            {
                0, 0, 100, 0, 0
            },
            "wood_escape_anim", "effects/wood_escape",
            new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            7
        );
        add_escape_spell(
            "water_escape", 5,
            new[]
            {
                100, 0, 0, 0, 0
            },
            "water_escape_anim", "effects/water_escape",
            new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            9
        );
        add_escape_spell(
            "fire_escape", 5,
            new[]
            {
                0, 100, 0, 0, 0
            },
            "fire_escape_anim", "effects/fire_escape",
            new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost)
            },
            5
        );
    }

    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    private static void add_blade_spells()
    {
        add_blade_spell(
            "gold_blade", 3, new[]
            {
                0, 0, 100, 0, 0
            },
            "gold_blade_anim", "effects/gold_blade", new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 3f)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, MiscUtils.WakanCostToDamage(spell_cost, anim.src_object),
                        anim.src_object, CW_AttackType.Spell);
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
        add_blade_spell(
            "water_blade", 3, new[]
            {
                100, 0, 0, 0, 0
            },
            "water_blade_anim", "effects/water_blade", new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 3f)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, MiscUtils.WakanCostToDamage(spell_cost, anim.src_object),
                        anim.src_object, CW_AttackType.Spell);

                    float dist = Toolbox.DistVec2Float(vec, dst_vec);
                    float force_x = (dst_vec.x - vec.x) / dist * 0.4f;
                    float force_y = (dst_vec.y - vec.y) / dist * 0.4f;
                    float force_z = 0.1f;
                    foreach (Actor target_unit in tiles.SelectMany(target_tile => target_tile._units))
                    {
                        target_unit.addForce(force_x, force_y, force_z);
                    }
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
        // TODO: 补充火刃燃烧效果
        add_blade_spell(
            "fire_blade", 3, new[]
            {
                0, 100, 0, 0, 0
            },
            "fire_blade_anim", "effects/fire_blade", new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 3f)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, MiscUtils.WakanCostToDamage(spell_cost, anim.src_object),
                        anim.src_object, CW_AttackType.Spell);
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
        add_blade_spell(
            "wind_blade", 3, new[]
            {
                40, 40, 20, 0, 0
            },
            "wind_blade_anim", "effects/wind_blade", new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 3f)
            }, 33f, (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx <= 2) return;
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    List<WorldTile> tiles = GeneralHelper.get_tiles_in_square(tile, 1);
                    GeneralHelper.damage_to_tiles(tiles, MiscUtils.WakanCostToDamage(spell_cost, anim.src_object),
                        anim.src_object, CW_AttackType.Spell);

                    foreach (Actor target_unit in tiles.SelectMany(target_tile => target_tile._units))
                    {
                        target_unit.addForce(0, 0, 0.8f);
                    }
                }
                else
                {
                    anim.force_stop();
                }
            }
        );
    }

    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    private static void add_give_self_status_spells()
    {
        CW_SpellAsset spell_asset;
        EffectController effect_controller;
        AnimationSetting anim_setting;

        // 玄武之甲
        FormatSpells.create_give_self_status_spell(
            "basalt_armor", "status_basalt_armor",
            rarity: 3, element_container: new[]
            {
                100, 0, 0, 0, 0
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.NAMED_DEFEND
            },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 2f)
            }
        );
        // 青龙之鳞
        FormatSpells.create_give_self_status_spell(
            "gloong_scale", "status_gloong_scale",
            rarity: 3, element_container: new[]
            {
                0, 0, 100, 0, 0
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.NAMED_DEFEND
            },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 2f)
            }
        );
        // 朱雀之羽
        FormatSpells.create_give_self_status_spell(
            "rosefinch_feather", "status_rosefinch_feather",
            rarity: 3, element_container: new[]
            {
                0, 100, 0, 0, 0
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.ATTACK
            },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 2f)
            }
        );
        // 麒麟之角
        FormatSpells.create_give_self_status_spell(
            "unicorn_horn", "status_unicorn_horn",
            rarity: 3, element_container: new[]
            {
                0, 0, 0, 0, 100
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.NAMED_DEFEND
            },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 2f)
            }
        );
        // 白虎之牙
        FormatSpells.create_give_self_status_spell(
            "wtiger_tooth", "status_wtiger_tooth",
            rarity: 3, element_container: new[]
            {
                0, 0, 0, 100, 0
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.ATTACK
            },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 2f)
            }
        );
        // 金刚护体
        FormatSpells.create_give_self_status_spell(
            "gold_shield", "status_gold_shield",
            rarity: 3, element_container: new[]
            {
                0, 0, 0, 100, 0
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.NAMED_DEFEND, SpellTriggerTag.UNNAMED_DEFEND
            },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 2f)
            }
        );
        // 水甲
        FormatSpells.create_give_self_status_spell(
            "water_shield", "status_water_shield",
            rarity: 3, element_container: new[]
            {
                100, 0, 0, 0, 0
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.NAMED_DEFEND, SpellTriggerTag.UNNAMED_DEFEND
            },
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 2f)
            }
        );
        // 兽化
        spell_asset = FormatSpells.create_give_self_status_spell(
            "brutalize", "status_brutalize",
            rarity: 1, element_container: new[]
            {
                20, 20, 20, 20, 20
            },
            trigger_tags: new[]
            {
                SpellTriggerTag.ATTACK, SpellTriggerTag.NAMED_DEFEND, SpellTriggerTag.UNNAMED_DEFEND
            },
            spell_cost_list: new KeyValuePair<string, float>[0]
        );
        spell_asset.spell_learn_check = (asset, user, target) => -1;
    }

    [SuppressMessage("ReSharper", "JoinDeclarationAndInitializer")]
    private static void add_track_projectile_spells()
    {
        CW_SpellAsset spell_asset;
        EffectController effect_controller;
        AnimationSetting anim_setting;

        #region 金剑

        spell_asset = FormatSpells.create_track_projectile_attack_spell(
            "gold_sword",
            "single_gold_sword_anim", "effects/single_gold_sword", 0.15f,
            rarity: 3,
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 6f)
            },
            element_container: new[]
            {
                0, 0, 0, 100, 0
            }
        );
        effect_controller = CW_Core.mod_state.anim_manager.get_controller(spell_asset.anim_id);
        effect_controller.anim_limit = 1000;
        anim_setting = effect_controller.default_setting;
        anim_setting.frame_interval = 1;
        anim_setting.trace_grad = 40;

        #endregion

        #region 木剑

        spell_asset = FormatSpells.create_track_projectile_attack_spell(
            "wood_sword",
            "single_wood_sword_anim", "effects/single_wood_sword", 0.15f,
            rarity: 3,
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 6f)
            },
            element_container: new[]
            {
                0, 0, 100, 0, 0
            }
        );
        effect_controller = CW_Core.mod_state.anim_manager.get_controller(spell_asset.anim_id);
        effect_controller.base_scale = 0.15f;
        effect_controller.anim_limit = 1000;
        anim_setting = effect_controller.default_setting;
        anim_setting.frame_interval = 1;
        anim_setting.trace_grad = 40;

        #endregion

        #region 水剑

        spell_asset = FormatSpells.create_track_projectile_attack_spell(
            "water_sword",
            "single_water_sword_anim", "effects/single_water_sword", 0.15f,
            rarity: 3,
            spell_cost_list: new KeyValuePair<string, float>[]
            {
                new(DataS.wakan, Content_Constants.default_spell_cost / 6f)
            },
            element_container: new[]
            {
                100, 0, 0, 0, 0
            }
        );
        effect_controller = CW_Core.mod_state.anim_manager.get_controller(spell_asset.anim_id);
        effect_controller.base_scale = 0.15f;
        effect_controller.anim_limit = 1000;
        anim_setting = effect_controller.default_setting;
        anim_setting.frame_interval = 1;
        anim_setting.trace_grad = 40;

        #endregion
    }

    // 飓风术
    private static void add_tornado_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.TIME_LIMIT,
            loop_time_limit = 5f,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            layer_name = "Objects",
            trace_grad = 20f,
            free_val = 0.2f,
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec,
                Animation.SpriteAnimation anim) =>
            {
                float cur_scale = anim.get_scale();
                if (anim.setting.loop_time_limit - anim.play_time > 32 * anim.setting.frame_interval &&
                    anim.setting.free_val > cur_scale)
                {
                    anim.change_scale(0.3f * (anim.setting.free_val - cur_scale) + 1);
                }
                else
                {
                    anim.change_scale(1 - cur_scale * 0.1f);
                    if (cur_scale < 0.1f)
                    {
                        anim.force_stop();
                        return;
                    }
                }

                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.change_scale(1 - cur_scale * 0.1f);
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)(anim.gameObject.transform.position.x - 0.5f),
                    (int)(anim.gameObject.transform.position.y - 0.5f));
                if (center == null) return;

                BrushData brush = Brush.get((int)(cur_scale * 6f));
                for (int i = 0; i < brush.pos.Length; i++)
                {
                    int num = center.x + brush.pos[i].x;
                    int num2 = center.y + brush.pos[i].y;
                    if (num < 0 || num >= MapBox.width || num2 < 0 || num2 >= MapBox.height) continue;

                    WorldTile tileSimple = MapBox.instance.GetTileSimple(num, num2);
                    if (tileSimple.Type.ocean && Toolbox.randomBool())
                    {
                        Tornado.spawnBurst(tileSimple, "rain", cur_scale);
                    }

                    if (!tileSimple.Type.lava) continue;
                    LavaHelper.removeLava(tileSimple);
                    if (Toolbox.randomBool())
                    {
                        Tornado.spawnBurst(tileSimple, "lava", cur_scale);
                    }
                }

                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
                foreach (BaseSimObject enemy in enemies)
                {
                    if (enemy.objectType == MapObjectType.Actor) ((CW_Actor)enemy).addForce(0, 0, 2f);
                    enemy.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.LINE);

        EffectManager.instance.load_as_controller("simple_tornado_anim", "effects/simple_tornado/",
            controller_setting: anim_setting, base_scale: 0.25f);
        CW_SpellAsset spell = new()
        {
            id = "tornado",
            rarity = 16,
            element = new CW_Element(new[]
            {
                40, 40, 20, 0, 0
            }),
            anim_id = "simple_tornado_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 2f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 复苏
    private static void add_regen_spell()
    {
        CW_SpellAsset spell = new()
        {
            id = "regen",
            rarity = 5,
            element = new CW_Element(new[]
            {
                0, 0, 100, 0, 0
            }),
            anim_id = "",
            anim_type = SpellAnimType.CUSTOM,
            anim_action = (asset, user, target, tile, cost) => { user.a.spawnParticle(Color.green); },
            target_camp = SpellTargetCamp.ALIAS,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 6f)
            }),
            spell_learn_check = LearnChecks.default_learn_check,
            spell_action = (asset, user, target, tile, cost) => { ((CW_Actor)user).Regenerate(S.health, cost); }
        };
        spell.add_trigger_tag(SpellTriggerTag.UNNAMED_DEFEND);
        spell.add_trigger_tag(SpellTriggerTag.NAMED_DEFEND);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 落木
    private static void add_fall_wood()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 25f,
            froze_time_after_end = 0.3f,
            free_val = 15f,
            always_roll = true,
            always_roll_axis = new Vector3(0, 0, 1),
            roll_angle_per_frame = 1000,
            layer_name = "Objects",
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec,
                Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
                if (center == null) return;
                List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 5);
                float force = 5f;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                //CW_EffectManager.instance.spawn_anim("bushido_base_anim", dst_vec, dst_vec, sprites.src_object, null, 1f);
                foreach (BaseSimObject actor in enemies)
                {
                    actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                    if (actor.objectType != MapObjectType.Actor) continue;
                    ((CW_Actor)actor).addForce((actor.currentPosition.x - dst_vec.x) / force,
                        (actor.currentPosition.y - dst_vec.y) / force, 1 / force);
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.LINE);

        EffectManager.instance.load_as_controller("fall_wood_anim", "effects/fall_wood/",
            controller_setting: anim_setting, base_scale: 0.25f);
        CW_SpellAsset spell = new()
        {
            id = "fall_wood",
            rarity = 3,
            element = new CW_Element(new[]
            {
                0, 0, 100, 0, 0
            }),
            anim_id = "fall_wood_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.fall_to_ground,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 3f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };

        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 落石
    private static void add_fall_rock()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 25f,
            free_val = 15f,
            froze_time_after_end = 0.3f,
            layer_name = "Objects",
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
                if (center == null) return;
                List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 5);
                float force = 5f;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                //CW_EffectManager.instance.spawn_anim("bushido_base_anim", dst_vec, dst_vec, sprites.src_object, null, 1f);
                foreach (BaseSimObject actor in enemies)
                {
                    actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                    if (actor.objectType != MapObjectType.Actor) continue;
                    ((CW_Actor)actor).addForce((actor.currentPosition.x - dst_vec.x) / force,
                        (actor.currentPosition.y - dst_vec.y) / force, 1 / force);
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.LINE);

        EffectManager.instance.load_as_controller("fall_rock_anim", "effects/fall_rock/",
            controller_setting: anim_setting, base_scale: 0.25f);
        CW_SpellAsset spell = new()
        {
            id = "fall_rock",
            rarity = 3,
            element = new CW_Element(new[]
            {
                0, 0, 0, 0, 100
            }),
            anim_id = "fall_rock_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.fall_to_ground,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 3f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    private static void add_fall_mountain(string name)
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 8f,
            free_val = 60f,
            froze_time_after_end = 0.3f,
            layer_name = "Objects",
            frame_action = [Hotfixable](int idx, ref Vector2 vec, ref Vector2 dst_vec,
                Animation.SpriteAnimation anim) =>
            {
                if (anim.data.hasFlag("end"))
                {
                    anim.data.get("frozen_time", out float frozen_time);

                    if (frozen_time > 0)
                    {
                        frozen_time -= anim.cur_elapsed;
                        anim.data.set("frozen_time", frozen_time);
                        return;
                    }

                    anim.change_scale(0.9f);
                    if (anim.get_scale() < 0.01f) anim.force_stop();
                }
            },
            end_action = [Hotfixable](int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                if (anim.data.hasFlag("end"))
                {
                    anim.has_end = false;
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
                if (center == null) return;
                float radius = 20 * anim.get_scale();
                List<BaseSimObject> enemies =
                    GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, radius);
                float force = 5f;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object) * 10;
                //CW_EffectManager.instance.spawn_anim("bushido_base_anim", dst_vec, dst_vec, sprites.src_object, null, 1f);
                foreach (BaseSimObject actor in enemies)
                {
                    actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                    if (actor.objectType != MapObjectType.Actor) continue;
                    ((CW_Actor)actor).addForce((actor.currentPosition.x - dst_vec.x) / force,
                        (actor.currentPosition.y - dst_vec.y) / force, 1 / force);
                }

                EffectsLibrary.spawnExplosionWave(dst_vec, radius / 2);
                MapAction.damageWorld(center, (int)radius, AssetManager.terraform.get("cw_fall_mountain"));

                anim.has_end = false;
                anim.data.addFlag("end");
                anim.data.set("frozen_time", anim.setting.froze_time_after_end);
            }
        };
        anim_setting.set_trace([Hotfixable](ref Vector2 src_vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim,
            ref float delta_x, ref float delta_y) =>
        {
            if (anim.has_end) return;
            if (!anim.data.hasFlag("started"))
            {
                anim.set_position(anim.src_object.currentPosition);
                anim.data.addFlag("started");
                return;
            }

            if (!anim.data.hasFlag("fall"))
            {
                var current_position = anim.gameObject.transform.position;

                float dist = Toolbox.DistVec2Float(src_vec, current_position);
                if (dist < 0.1f)
                {
                    anim.data.addFlag("fall");
                    return;
                }

                delta_x = (src_vec.x - current_position.x) / anim.setting.froze_time_after_end;
                delta_y = (src_vec.y - current_position.y) / anim.setting.froze_time_after_end;

                if (anim.get_scale() <= 1.07f)
                {
                    anim.change_scale(1.02f);
                }

                return;
            }

            if (anim.data.hasFlag("end") && anim.src_object != null)
            {
                anim.data.get("frozen_time", out float frozen_time);
                if (frozen_time > 0) return;
                var current_position = anim.gameObject.transform.position;
                delta_x = (anim.src_object.currentPosition.x - current_position.x) / anim.setting.froze_time_after_end;
                delta_y = (anim.src_object.currentPosition.y - current_position.y) / anim.setting.froze_time_after_end;
                return;
            }

            if (anim.get_scale() > 1.07f)
            {
                delta_y = -9.8f * anim.play_time * anim.setting.trace_grad * 10;
                var current_position = anim.gameObject.transform.position;
                if (current_position.y <= dst_vec.y)
                {
                    delta_y = 0;
                    return;
                }

                if (current_position.y + delta_y * anim.cur_elapsed < dst_vec.y)
                {
                    delta_y = (dst_vec.y - current_position.y) / anim.cur_elapsed;
                }

                return;
            }

            anim.change_scale(1.04f);
        });

        EffectManager.instance.load_as_controller($"fall_{name}_anim", $"effects/fall_{name}_mountain/",
            controller_setting: anim_setting, base_scale: 0.25f);
        CW_SpellAsset spell = new()
        {
            id = $"fall_{name}_mountain",
            rarity = 95,
            element = new CW_Element(new[]
            {
                0, 0, 0, 0, 100
            }),
            anim_id = $"fall_{name}_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.fall_to_ground,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 3f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 地刺
    private static void add_ground_thorn_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.NUMBER_LIMIT,
            loop_nr_limit = 1,
            frame_interval = 0.05f,
            layer_name = "Objects",
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx != 3) return;
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                var position = anim.gameObject.transform.position;
                int x = (int)position.x;
                int y = (int)position.y;
                WorldTile tile = MapBox.instance.GetTile(x, y);
                if (tile != null)
                {
                    List<BaseSimObject> targets =
                        GeneralHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 3);
                    float force_z = 1.0f;
                    anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                    spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                    foreach (BaseSimObject actor in targets)
                    {
                        actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                        if (actor.objectType == MapObjectType.Actor) ((CW_Actor)actor).addForce(0, 0, force_z);
                    }
                }
                else
                {
                    anim.force_stop();
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.NONE);

        EffectManager.instance.load_as_controller("ground_thorn_anim", "effects/ground_thorn/",
            controller_setting: anim_setting, base_scale: 0.3f);
        CW_SpellAsset spell = new()
        {
            id = "ground_thorn",
            rarity = 3,
            element = new CW_Element(new[]
            {
                0, 0, 0, 0, 100
            }),
            anim_id = "ground_thorn_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.simple_on_something,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 木刺
    private static void add_wood_thorn_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.TIME_LIMIT,
            loop_time_limit = 12,
            loop_nr_limit = -1,
            anim_froze_frame_idx = 6,
            frame_interval = 0.05f,
            layer_name = "Objects",
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx > 2)
                {
                    if (anim.src_object == null || !anim.src_object.isAlive())
                    {
                        anim.force_stop();
                        return;
                    }

                    var position = anim.gameObject.transform.position;
                    int x = (int)position.x;
                    int y = (int)position.y;
                    WorldTile tile = MapBox.instance.GetTile(x, y);
                    if (tile != null)
                    {
                        List<BaseSimObject> targets =
                            GeneralHelper.find_enemies_in_square(tile, anim.src_object.kingdom, 2);
                        anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                        spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                        foreach (BaseSimObject actor in targets)
                        {
                            actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell,
                                pAttacker: anim.src_object);
                        }
                    }
                    else
                    {
                        anim.force_stop();
                    }
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.NONE);

        EffectManager.instance.load_as_controller("wood_thorn_anim", "effects/wood_thorn/",
            controller_setting: anim_setting, base_scale: 0.3f);
        CW_SpellAsset spell = new()
        {
            id = "wood_thorn",
            rarity = 3,
            element = new CW_Element(new[]
            {
                0, 0, 100, 0, 0
            }),
            anim_id = "wood_thorn_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.simple_on_something,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 藤缚
    private static void add_vine_bound_spell()
    {
        CW_SpellAsset spell = new()
        {
            id = "vine_bound",
            rarity = 5,
            element = new CW_Element(new[]
            {
                0, 0, 100, 0, 0
            }),
            anim_id = "",
            anim_type = SpellAnimType.CUSTOM,
            anim_action = null,
            spell_action = SpellActions.generate_give_status_spell_action("status_vine_bound"),
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 1.5f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 石化
    private static void add_landificate_spell()
    {
        CW_SpellAsset spell = new()
        {
            id = "landificate",
            rarity = 30,
            element = new CW_Element(new[]
            {
                0, 0, 0, 0, 100
            }),
            anim_id = "",
            anim_type = SpellAnimType.CUSTOM,
            anim_action = null,
            spell_action = SpellActions.generate_give_status_spell_action("status_landificate"),
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 1.5f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 冰封
    private static void add_ice_bound_spell()
    {
        CW_SpellAsset spell = new()
        {
            id = "ice_bound",
            rarity = 5,
            element = new CW_Element(new[]
            {
                100, 0, 0, 0, 0
            }),
            anim_id = "",
            anim_type = SpellAnimType.CUSTOM,
            anim_action = null,
            spell_action = SpellActions.generate_give_status_spell_action("status_ice_bound"),
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 1.5f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 风丸
    private static void add_wind_polo_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 20f,
            point_to_dst = true,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
                if (center == null) return;
                List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
                float force_z = 0.8f;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                foreach (BaseSimObject actor in enemies)
                {
                    actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                    if (actor.objectType != MapObjectType.Actor) continue;
                    ((CW_Actor)actor).addForce(0, 0, force_z);
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.LINE);

        EffectManager.instance.load_as_controller("wind_polo_anim", "effects/wind_polo/",
            controller_setting: anim_setting, base_scale: 0.08f);
        CW_SpellAsset spell = new()
        {
            id = "wind_polo",
            rarity = 3,
            element = new CW_Element(new[]
            {
                40, 40, 20, 0, 0
            }),
            anim_id = "wind_polo_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 10f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 雷丸
    private static void add_lightning_polo_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 20f,
            point_to_dst = true,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
                if (center == null) return;
                List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                foreach (BaseSimObject actor in enemies)
                {
                    actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.LINE);

        EffectManager.instance.load_as_controller("lightning_polo_anim", "effects/lightning_polo/",
            controller_setting: anim_setting, base_scale: 0.08f);
        CW_SpellAsset spell = new()
        {
            id = "lightning_polo",
            rarity = 3,
            element = new CW_Element(new[]
            {
                40, 40, 0, 20, 0
            }),
            anim_id = "lightning_polo_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 5f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 水球
    private static void add_water_polo_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 20f,
            point_to_dst = true,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
                if (center == null) return;
                List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
                float dist = Toolbox.DistVec2Float(vec, dst_vec);
                float force_x = (dst_vec.x - vec.x) / dist * 0.5f;
                float force_y = (dst_vec.y - vec.y) / dist * 0.5f;
                float force_z = 0.15f;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                foreach (BaseSimObject actor in enemies)
                {
                    actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                    if (actor.objectType != MapObjectType.Actor) continue;
                    ((CW_Actor)actor).addForce(force_x, force_y, force_z);
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.LINE);

        EffectManager.instance.load_as_controller("water_polo_anim", "effects/water_polo/",
            controller_setting: anim_setting, base_scale: 0.08f);
        CW_SpellAsset spell = new()
        {
            id = "water_polo",
            rarity = 3,
            element = new CW_Element(new[]
            {
                100, 0, 0, 0, 0
            }),
            anim_id = "water_polo_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 10f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 火球
    private static void add_fire_polo_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            frame_interval = 0.05f,
            trace_grad = 20f,
            point_to_dst = true,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.src_object == null || !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = MapBox.instance.GetTile((int)dst_vec.x, (int)dst_vec.y);
                if (center == null) return;
                EffectManager.instance.spawn_anim("explosion_anim", center.posV, center.posV, null, null, 0.06f);
                List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 3);
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                float force = 3f;
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                foreach (BaseSimObject actor in enemies)
                {
                    actor.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                    if (actor.objectType != MapObjectType.Actor) continue;
                    ((CW_Actor)actor).addForce((actor.currentPosition.x - dst_vec.x) / force,
                        (actor.currentPosition.y - dst_vec.y) / force,
                        Toolbox.DistVec2Float(actor.currentPosition, dst_vec) / force);
                }
            }
        };
        anim_setting.set_trace(AnimationTraceType.LINE);

        EffectManager.instance.load_as_controller("fire_polo_anim", "effects/fire_polo/",
            controller_setting: anim_setting, base_scale: 0.08f);
        EffectManager.instance.load_as_controller("explosion_anim", "effects/explosion/",
            controller_setting: new AnimationSetting(),
            base_scale: 1f);
        CW_SpellAsset spell = new()
        {
            id = "fire_polo",
            rarity = 3,
            element = new CW_Element(new[]
            {
                0, 100, 0, 0, 0
            }),
            anim_id = "fire_polo_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost / 7f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 太阴五雷
    private static void add_negative_quintuple_lightning_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_nr_limit = 5,
            trace_type = AnimationTraceType.ATTACH,
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx != 5) return;
                float radius = 5;
                if (anim.src_object == null || !anim.src_object.isAlive() || anim.dst_object == null ||
                    !anim.dst_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = anim.dst_object.currentTile;
                if (center == null) return;
                List<WorldTile> tiles = GeneralHelper.get_tiles_in_circle(center, radius);
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                GeneralHelper.damage_to_tiles(tiles, spell_cost, anim.src_object, CW_AttackType.Spell);
                foreach (WorldTile tile in tiles)
                {
                    tile.setBurned();
                }
            },
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                // TODO: 补充额外效果
            }
        };
        EffectManager.instance.load_as_controller("negative_quintuple_lightning_anim", "effects/default_lightning/",
            controller_setting: anim_setting, base_scale: 0.125f);

        CW_SpellAsset spell = new()
        {
            id = "negative_quintuple_lightning",
            rarity = 30,
            element = new CW_Element(new[]
            {
                40, 40, 0, 20, 0
            }),
            anim_id = "negative_quintuple_lightning_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.simple_on_something,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 5f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 太阳五雷
    private static void add_positive_quintuple_lightning_spell()
    {
        AnimationSetting anim_setting = new()
        {
            loop_nr_limit = 5,
            trace_type = AnimationTraceType.ATTACH,
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx != 5) return;
                float radius = 5;
                if (anim.src_object == null || !anim.src_object.isAlive() || anim.dst_object == null ||
                    !anim.dst_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = anim.dst_object.currentTile;
                if (center == null) return;
                List<WorldTile> tiles = GeneralHelper.get_tiles_in_circle(center, radius);
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                GeneralHelper.damage_to_tiles(tiles, spell_cost, anim.src_object, CW_AttackType.Spell);
                foreach (WorldTile tile in tiles)
                {
                    tile.setBurned();
                }
            },
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) => { }
        };
        EffectManager.instance.load_as_controller("positive_quintuple_lightning_anim", "effects/default_lightning/",
            controller_setting: anim_setting, base_scale: 0.125f);

        CW_SpellAsset spell = new()
        {
            id = "positive_quintuple_lightning",
            rarity = 30,
            element = new CW_Element(new[]
            {
                40, 40, 0, 20, 0
            }),
            anim_id = "positive_quintuple_lightning_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.simple_on_something,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 5f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 雷法
    private static void add_default_lightning_spell()
    {
        AnimationSetting anim_setting = new()
        {
            frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (idx != 5) return;
                float radius = 5;
                if (anim.src_object == null || !anim.src_object.isAlive() || anim.dst_object == null ||
                    !anim.dst_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                WorldTile center = anim.dst_object.currentTile;
                if (center == null) return;
                List<WorldTile> tiles = GeneralHelper.get_tiles_in_circle(center, radius);
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                GeneralHelper.damage_to_tiles(tiles, spell_cost, anim.src_object, CW_AttackType.Spell);
                foreach (WorldTile tile in tiles)
                {
                    tile.setBurned();
                }
            }
        };
        EffectManager.instance.load_as_controller("default_lightning_anim", "effects/default_lightning/",
            controller_setting: anim_setting, base_scale: 0.125f);

        CW_SpellAsset spell = new()
        {
            id = "default_lightning",
            rarity = 4,
            element = new CW_Element(new[]
            {
                40, 40, 0, 20, 0
            }),
            anim_id = "default_lightning_anim",
            anim_type = SpellAnimType.ON_TARGET,
            anim_action = AnimActions.simple_on_something,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.TILE,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 2f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 虚无之火
    private static void add_void_fire_spell()
    {
        AnimationSetting anim_setting = new()
        {
            frame_interval = 0.05f,
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            point_to_dst = false,
            anim_froze_frame_idx = -1,
            trace_grad = 20,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.dst_object == null || !anim.dst_object.isAlive() || anim.src_object == null ||
                    !anim.src_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                Animation.SpriteAnimation anti_matter = EffectManager.instance.spawn_anim("anti_matter_anim",
                    anim.dst_object.currentPosition, anim.dst_object.currentPosition, anim.src_object, anim.dst_object);
                if (anti_matter == null) return;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                spell_cost = MiscUtils.WakanCostToDamage(spell_cost, anim.src_object);
                anti_matter.data.set(DataS.spell_cost, spell_cost);
            }
        };
        anim_setting.set_trace(AnimationTraceType.TRACK);

        EffectManager.instance.load_as_controller("void_fire_anim", "effects/void_fire/", 10,
            controller_setting: anim_setting, base_scale: 0.12f);

        anim_setting.loop_limit_type = AnimationLoopLimitType.TIME_LIMIT;
        anim_setting.loop_time_limit = 3f;
        anim_setting.frame_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
        {
            if (idx == 17) anim.cur_frame_idx = 6;
            WorldTile center = MapBox.instance.GetTile((int)vec.x, (int)vec.y);
            if (center == null || anim.src_object == null || !anim.src_object.isAlive())
            {
                anim.force_stop();
                return;
            }

            List<BaseSimObject> enemies = GeneralHelper.find_enemies_in_circle(center, anim.src_object.kingdom, 5);
            anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
            foreach (BaseSimObject enemy in enemies)
            {
                enemy.getHit(spell_cost, pType: (AttackType)CW_AttackType.Spell, pAttacker: anim.src_object);
                if (enemy.objectType != MapObjectType.Actor) continue;
                if (!enemy.isAlive())
                {
                    // TODO: 夺取灵气
                    // ((CW_Actor)anim.src_object).check_level_up(Content_Constants.immortal_id);
                }
                else
                {
                    // 拖拽
                    ((CW_Actor)enemy).addForce((dst_vec.x - enemy.currentPosition.x) * 0.1f,
                        (dst_vec.y - enemy.currentPosition.y) * 0.1f, 0.05f);
                }
            }
        };
        anim_setting.layer_name = "EffectsBack";
        anim_setting.end_action = null;
        anim_setting.set_trace(AnimationTraceType.NONE);
        EffectManager.instance.load_as_controller("anti_matter_anim", "effects/anti_matter/", 10,
            controller_setting: anim_setting, base_scale: 0.12f);

        CW_SpellAsset spell = new()
        {
            id = "void_fire",
            rarity = 95,
            element = new CW_Element(new[]
            {
                0, 100, 0, 0, 0
            }),
            anim_id = "void_fire_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 10f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 三昧真火
    private static void add_samadhi_fire_spell()
    {
        AnimationSetting anim_setting = new()
        {
            frame_interval = 0.05f,
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            point_to_dst = false,
            anim_froze_frame_idx = -1,
            trace_grad = 20,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.dst_object == null || !anim.dst_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                CW_StatusEffectData status_data = ((CW_Actor)anim.dst_object)
                    .AddStatus("status_samadhi_fire", anim.src_object);
                if (status_data == null) return;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                status_data.effect_val = spell_cost;
            }
        };
        anim_setting.set_trace(AnimationTraceType.TRACK);

        EffectManager.instance.load_as_controller("samadhi_fire_anim", "effects/samadhi_fire/", 10,
            controller_setting: anim_setting, base_scale: 0.12f);
        CW_SpellAsset spell = new()
        {
            id = "samadhi_fire",
            rarity = 95,
            element = new CW_Element(new[]
            {
                0, 100, 0, 0, 0
            }),
            anim_id = "samadhi_fire_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 10f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 红莲业火
    private static void add_loltus_fire_spell()
    {
        AnimationSetting anim_setting = new()
        {
            frame_interval = 0.05f,
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            point_to_dst = false,
            anim_froze_frame_idx = -1,
            trace_grad = 20,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.dst_object == null || !anim.dst_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                CW_Actor dst_obj = (CW_Actor)anim.dst_object;
                CW_StatusEffectData status_data = dst_obj
                    .AddStatus("status_loltus_fire", anim.src_object,
                        Library.Manager.statuses.get("status_loltus_fire").duration *
                        (0.1f + dst_obj.data.kills / 10f));
                if (status_data == null) return;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                status_data.effect_val = spell_cost;
            }
        };
        anim_setting.set_trace(AnimationTraceType.TRACK);

        EffectManager.instance.load_as_controller("loltus_fire_anim", "effects/loltus_fire/", 10,
            controller_setting: anim_setting, base_scale: 0.12f);
        CW_SpellAsset spell = new()
        {
            id = "loltus_fire",
            rarity = 95,
            element = new CW_Element(new[]
            {
                0, 100, 0, 0, 0
            }),
            anim_id = "loltus_fire_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 10f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }

    // 九幽冥焰
    private static void add_fen_fire_spell()
    {
        AnimationSetting anim_setting = new()
        {
            frame_interval = 0.05f,
            loop_limit_type = AnimationLoopLimitType.DST_LIMIT,
            loop_nr_limit = -1,
            point_to_dst = false,
            anim_froze_frame_idx = -1,
            trace_grad = 20,
            end_action = (int idx, ref Vector2 vec, ref Vector2 dst_vec, Animation.SpriteAnimation anim) =>
            {
                if (anim.dst_object == null || !anim.dst_object.isAlive())
                {
                    anim.force_stop();
                    return;
                }

                CW_StatusEffectData status_data = ((CW_Actor)anim.dst_object)
                    .AddStatus("status_fen_fire", anim.src_object);
                if (status_data == null) return;
                anim.data.get(DataS.spell_cost, out float spell_cost, 1f);
                status_data.effect_val = spell_cost;
            }
        };
        anim_setting.set_trace(AnimationTraceType.TRACK);

        EffectManager.instance.load_as_controller("fen_fire_anim", "effects/fen_fire/", 10,
            controller_setting: anim_setting, base_scale: 0.12f);
        CW_SpellAsset spell = new()
        {
            id = "fen_fire",
            rarity = 95,
            element = new CW_Element(new[]
            {
                0, 100, 0, 0, 0
            }),
            anim_id = "fen_fire_anim",
            anim_type = SpellAnimType.USER_TO_TARGET,
            anim_action = AnimActions.simple_user_to_target,
            target_camp = SpellTargetCamp.ENEMY,
            target_type = SpellTargetType.ACTOR,
            spell_cost_action = CostChecks.generate_spell_cost_action(new[]
            {
                new KeyValuePair<string, float>(DataS.wakan, Content_Constants.default_spell_cost * 10f)
            }),
            spell_learn_check = LearnChecks.default_learn_check
        };
        spell.add_trigger_tag(SpellTriggerTag.ATTACK);
        spell.add_cultisys_require(CultisysType.WAKAN);
        Library.Manager.spells.add(spell);
    }
}