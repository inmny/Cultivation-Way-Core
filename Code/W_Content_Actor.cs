using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Library;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way.Content
{
    internal static class W_Content_Actor
    {
        internal static void add_actors()
        {
            add_eastern_humans();
            add_yaos();
        }

        private static void add_yaos()
        {
            CW_ActorStats _yao = new CW_ActorStats();
            
            throw new NotImplementedException();
        }

        private static void add_eastern_humans()
        {
            CW_ActorStats stats = new CW_ActorStats();
            stats.anti_time_stop = false;                       // 不抗时停
            stats.born_spells = new List<string>();             // 出生自带法术
            stats.culti_velo = 1.0f;                            // 修炼速度系数
            stats.fixed_name = null;                            // 固定名
            stats.id = "unit_eastern_human";
            stats.prefer_element = new int[] { 20, 20, 20, 20, 20 };// 元素偏好
            stats.prefer_element_scale = 0f;                    // 元素偏好程度
            stats.cw_base_stats = new CW_BaseStats();
            stats.origin_stats = new ActorStats();
            ActorStats origin = stats.origin_stats;
            origin.id = stats.id;
            origin.action_death = null;                         // TODO: 转换为神明
            origin.actorSize = ActorSize.S13_Human;             // 人物大小，用于判定底下光圈
            origin.aggression = 0;                              // TODO: 0.14.3游戏未使用
            origin.animal = false;                              // 不为动物
            origin.animal_baby_making_around_limit = 2;         // 由于animal=false无意义
            origin.animation_idle = "walk_0";                   // 采用默认值
            origin.animation_idle_speed = 0.1f;                 // 采用默认值
            origin.animation_swim = "swim_0,swim_1,swim_2,swim_3";               // 采用默认值
            origin.animation_swim_speed = 0.1f;                 // 采用默认值
            origin.animation_walk = "walk_0,walk_1,walk_2,walk_3";               // 采用默认值
            origin.animation_walk_speed = 0.1f;                 // 采用默认值
            origin.attack_spells = new List<string>();          // 原版法术，舍弃
            origin.baby = false;                                // 不为小孩
            stats.cw_base_stats.base_stats = origin.baseStats;       // 人物基本属性，在后面进行设置
            origin.body_separate_part_hands = false;            // TODO: 0.14.3游戏未使用
            origin.body_separate_part_head = false;             // 头身一体
            origin.canAttackBrains = false;                     // 不允许攻击金脑
            origin.canAttackBuildings = true;                   // 允许攻击建筑
            origin.canBeHurtByPowers = true;                    // 允许权力造成伤害
            origin.canBeInspected = true;                       // 允许检查属性
            origin.canBeKilledByDivineLight = false;            // 不允许被圣光击杀
            origin.canBeKilledByLifeEraser = true;              // 允许被生命橡皮擦击杀
            origin.canBeKilledByStuff = true;                   // 允许被人物击杀，一个canAttackTarget条件
            origin.canBeMovedByPowers = true;                   // 允许被吸铁石等移动
            origin.canHaveStatusEffect = true;                  // 允许拥有状态
            origin.canLevelUp = true;                           // 允许升级（原版
            origin.canReceiveTraits = true;                     // 允许获取特质
            origin.canTurnIntoMush = true;                      // 允许变成菌类生物
            origin.canTurnIntoTumorMonster = true;              // 允许变成肿瘤怪物
            origin.canTurnIntoZombie = true;                    // 允许变成僵尸
            origin.can_edit_traits = true;                      // 允许编辑特质
            origin.color = new Color32(90, 164, 174, 255);      // 略缩地图上无文化颜色0x5aa4ae
            origin.color_sets = null;                           // 肤色集合，并未提供多肤色
            addColorSet(origin, "default", "#FFC984", "#543E2C");
            origin.cost = null;                                 // 从建筑产生需要的资源，无
            origin.countAsUnit = false;                         // 不记为野兽
            origin.currentAmount = 0;                           // 当前数量
            origin.damagedByOcean = false;                      // 在海洋里不受伤
            origin.damagedByRain = false;                       // 在雨里不受伤
            origin.deathAnimationAngle = true;                  // 死亡后倾倒
            origin.defaultAttack = "base";                      // 默认采用拳头攻击
            origin.defaultWeapons = null;                       // 无初始武器
            origin.defaultWeaponsMaterial = null;               // 无初始武器材料
            origin.defaultZ = 0;                                // 默认受击中心
            origin.dieByLightning = true;                       // TODO: 0.14.3游戏未使用
            origin.dieInLava = true;                            // 能够被岩浆伤害
            origin.dieOnBlocks = true;                          // 能够被山地伤害
            origin.dieOnGround = false;                         // 不能够被平地伤害
            origin.diet_berries = false;                        // 不个体获食
            origin.diet_crops = false;                          // 不个体获食
            origin.diet_flowers = false;                        // 不个体获食
            origin.diet_grass = false;                          // 不个体获食
            origin.diet_meat = false;                           // 不个体获食
            origin.diet_meat_insect = false;                    // 不个体获食
            origin.diet_meat_same_race = false;                 // 不个体获食
            origin.diet_vegetation = false;                     // 不个体获食
            origin.disableJumpAnimation = true;                 // 移动时不跳跃
            origin.disablePunchAttackAnimation = false;         // 保留攻击动画
            origin.drawBoatMark = false;                        // 不标记为船
            origin.drawBoatMark_big = false;                    // 不标记为大船，上一项为false时，此项作废
            origin.effectDamage = true;                         // 受击特效，如变红，或接收Drop变白
            origin.effect_cast_ground = "";                     // 使用法术时自身的额外的作用于地面的特效
            origin.effect_cast_top = "";                        // 使用法术时自身的额外的作用于头顶的特效
            origin.effect_teleport = "";                        // 单独的传送特效，TODO: 暂无用
            origin.egg = false;                                 // 不是蛋
            origin.eggStatsID = "";                             // 所生的蛋的id
            //origin.flags                                      // TODO: 无需初始化
            origin.flag_dragon = false;                         // 被击杀后不提供猎龙者
            origin.flag_finger = false;                         // 不逃离神之指伤害
            origin.flag_mage = false;                           // 被击杀后不提供猎巫者
            origin.flag_tornado = false;                        // 不存在Tornado组件
            origin.flag_turtle = false;                         // 原版满级时不提供相关成就
            origin.flag_ufo = false;                            // 原版雷击下不会受到ufo专属的10000原始伤害并赐死
            origin.flipAnimation = true;                        // 移动时转向
            origin.flying = false;                              // 人物初始状态不为飞行，并在如果会受到海洋伤害的情况下无法穿越海洋，即便人物处在飞行状态
            origin.growIntoID = null;                           // 长大后的id，baby为false，此项无效
            origin.haveSpriteRenderer = true;                   // 采用默认的贴图渲染方式
            origin.have_skin = true;                            // 能够被灼伤、中毒
            origin.have_soul = true;                            // 能够产生幽灵
            origin.heads = 1;                                   // 头的种类，body_separate_part_head为false，此项无效，为1目的是令[0,heads)整数随机数能够正常生成
            origin.hideFavoriteIcon = false;                    // 显示最爱图标
            origin.hideOnMinimap = true;                        // 不在略缩地图上完整显示
            origin.hit_fx_alternative_offset = true;            // TODO: 0.14.3游戏中未使用
            origin.hovering = false;                            // 不悬停于空中
            origin.hovering_max = 1.2f;                         // 悬停最大高度，采用默认值
            origin.hovering_min = 0.5f;                         // 悬停最低高度，采用默认值
            origin.icon = "iconEastern_Humans";                 // 在生物信息窗口顶部的人物图标
            origin.ignoredByInfinityCoin = false;               // 不被无限硬币忽略
            origin.ignoreJobs = false;                          // TODO: 0.14.3游戏中未使用
            origin.ignoreTileSpeedMod = false;                  // 不忽略地块的速度影响
            origin.immune_to_injuries = false;                  // 不免疫残疾（眼罩 等特质
            origin.immune_to_slowness = false;                  // 不免疫地块给予的缓慢
            origin.immune_to_tumor = false;                     // 不免疫肿瘤感染、疯狂
            origin.inspectAvatarScale = 2.5f;                   // 查看人物时，人物图像的大小，采用默认值
            origin.inspect_children = true;                     // 人物信息窗口显示孩子数量
            origin.inspect_experience = true;                   // 人物信息窗口显示经验和等级
            origin.inspect_home = true;                         // 人物信息窗口显示所在城市
            origin.inspect_kills = true;                        // 人物信息窗口显示击杀数
            origin.inspect_stats = true;                        // 人物信息窗口显示如（攻击、防御、移速、外交、攻速、暴击）属性
            origin.isBoat = false;                              // 不是船，
            origin.job = "unit";                                // 默认职业为空，该项相关逻辑顺序为：（智慧生物（小孩、城市、该项）、该项），疯狂，灼烧，空
            origin.kingdom = "";                                // 默认国家，该项在unit = true时基本无用，当不设置默认国家时，会存在无国家生物（原版已经存在），放置的智慧生物会自动设置为nomads_{race}国家
            origin.landCreature = true;                         // 陆地生物，影响寻路与目标攻击
            origin.layEggs = false;                             // 不生蛋
            origin.maxAge = 90;                                 // 初始最大寿命
            origin.maxHunger = 100;                             // 最大饱食度
            origin.maxRandomAmount = 3;                         // 触发世界随机生成该生物的最大数量+1
            origin.moveFromBlock = true;                        // 自动从山地走开
            origin.mushID = "mush_unit";                        // 菌类感染诞生的生物id
            origin.nameLocale = "Eastern Humans";               // 统计数据中的id
            origin.nameTemplate = "eastern_human_name";         // 命名模板id
            origin.needFood = true;                             // 需要食物
            origin.newBeh = false;                              // TODO: 0.14.3游戏未使用
            origin.oceanCreature = false;                       // 海洋生物，影响寻路与目标攻击
            origin.path_movement_timeout = 2f;                  // 移动最大时间消耗
            origin.playRandomSound = false;                     // 随机播放声音
            origin.playRandomSound_id = "-";                    // 随机播放的声音id，当playRandomSound=false时无效
            origin.prefab = "p_unit";                           // 人物单位的预制体
            origin.procreate = true;                            // 可生育，TODO: 应用于智慧生物
            origin.procreate_age = 18;                          // 生育年龄，TODO: 统一至此
            origin.race = "eastern_human";                      // 种族
            origin.rotatingAnimation = false;                   // 行走时身体不发生转动
            origin.shadow = true;                               // 人物阴影
            origin.shadowTexture = "unitShadow_5";              // 阴影材质，采用默认值
            origin.showIconInspectWindow = false;               // TODO: 0.14.3游戏未使用
            origin.showIconInspectWindow_id = "";               // TODO: 0.14.3游戏未使用
            origin.skeletonID = "skeleton_cursed";              // 转换为骷髅后的骷髅的id
            origin.skipFightLogic = false;                      // 采用默认的战斗逻辑
            origin.skipSave = false;                            // 不跳过保存
            origin.source_meat = false;                         // 不是肉类来源
            origin.source_meat_insect = false;                  // 不是昆虫肉类来源
            origin.specialAnimation = false;                    // 不采用特殊移动动画
            origin.specialDeadAnimation = false;                // 不采用特殊死亡动画
            origin.speedModLiquid = 1f;                         // 在液体中的移动速度
            origin.sprite_group_renderer = true;                // TODO: 待研究，决定了checkSpriteConstructor是否生效
            origin.swampCreature = false;                       // 不是沼泽生物
            origin.swimToIsland = true;                         // 在不是海洋生物的情况下，如果在水中停止了移动则会自动游泳到最近的岛上
            origin.take_items = true;                           // 当使用物品且击杀目标时会尝试夺取对方物品
            origin.take_items_ignore_range_weapons = false;     // 夺取物品时不忽略远程武器
            origin.tech = String.Empty;                         // 生产该单位不需要科技，仅用于船
            origin.texture_atlas = UnitTextureAtlasID.UnitsSmall;// 默认的单位贴图构建
            origin.texture_heads = String.Empty;                // 对于智慧生物无用
            origin.texture_path = String.Empty;                 // 对于智慧生物无用
            origin.timeToGrow = 0;                              // 小孩长大需要时间（秒），baby=false时无用
            origin.traits = new List<string>();                 // 默认特质
            origin.tumorMonsterID = "tumor_monster_unit";       // 转换为肿瘤怪物后的生物id
            origin.unit = true;                                 // 是智慧生物
            origin.updateZ = true;                              // z轴坐标会更新
            origin.useSkinColors = true;                        // 不使用不同肤色
            origin.use_items = true;                            // 能够从除诞生以外其他途径获取物品
            origin.very_high_flyer = false;                     // 不能避免高温、部分爆炸、部分force的作用
            origin.zombieID = "zombie";                         // TODO: 转换为僵尸后的生物id
            origin.baseStats.attackSpeed = 60;
            origin.setBaseStats(120, 15, 40, 0, 10, 95, 10);
            CW_Library_Manager.instance.units.add(stats);

            CW_ActorStats baby_stats = CW_Library_Manager.instance.units.clone("baby_eastern_human", "unit_eastern_human");
            ActorStats baby_origin = AssetManager.unitStats.clone("baby_eastern_human", "unit_eastern_human");
            baby_stats.origin_stats = baby_origin;
            baby_origin.take_items = false;
            baby_origin.baseStats.speed = 10f;
            baby_origin.timeToGrow = 200;
            baby_origin.baby = true;
            baby_origin.growIntoID = "unit_eastern_human";
            baby_origin.animation_idle = "walk_3";
            baby_origin.traits.Add("peaceful");
            addColorSet(baby_origin, "default", "#FFC984", "#543E2C");

            AssetManager.unitStats.CallMethod("loadShadow", origin);
            AssetManager.unitStats.CallMethod("loadShadow", baby_origin);
        }
        private static void addColorSet(ActorStats stats, string pID, string pColorFrom, string pColorTo)
        {
            stats.useSkinColors = true;
            if (stats.color_sets == null)
            {
                stats.color_sets = new List<ColorSet>();
            }
            ColorSet colorSet = new ColorSet();
            colorSet.id = pID;
            stats.color_sets.Add(colorSet);
            Color color = Toolbox.makeColor(pColorFrom, -1f);
            Color color2 = Toolbox.makeColor(pColorTo, -1f);
            int num = 5;
            float num2 = 1f / (float)(num - 1);
            for (int i = 0; i < num; i++)
            {
                float num3 = 1f - (float)i * num2;
                if (num3 > 1f)
                {
                    num3 = 1f;
                }
                Color color3 = Toolbox.blendColor(color, color2, num3);
                colorSet.colors.Add(color3);
            }
        }
    }
}
