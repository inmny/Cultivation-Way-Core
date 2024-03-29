# CW_Actor	人物拓展

这是模组中最为核心的部分

继承自Actor，游戏所有Actor均已转换为CW_Actor，可强制转换

## 字段

| 字段名                   | 类型                                                   | 访问权限   | 实例成员  | 备注                                                         |
|-----------------------|------------------------------------------------------|--------|:-----:|------------------------------------------------------------|
| cw_stats              | [Library.CW_ActorStats](../Library/CW_ActorStats.md) | public | True  | 该生物对应的拓展后的ActorStats                                       |
| cw_cur_stats          | [CW_BaseStats](./CW_BaseStats.md)                    | public | True  | 该生物的属性，其中basestats字段为<br />该生物的curStats的引用                 |
| cw_data               | [CW_ActorData](./CW_ActorData.md)                    | public | True  | 该生物的数据                                                     |
| cw_status             | [CW_ActorStatus](./CW_ActorStatus.md)                | public | True  | 该生物的拓展状态（如灵气、护盾、<br />元神等）                                 |
| fast_data             | ActorStatus                                          | public | True  | 该生物的原版状态（如生命、性别等）<br />提供快速访问                              |
| cur_spells            | List\<string\>                                       | public | True  | 该生物当前可释放的法术的id                                             |
| can_act               | bool                                                 | public | True  | 该生物当前是否可以行动，一般由石化、<br />藤缚术等修改，在所有束缚状态效果<br />结束后会自动置False |
| get_attackedBy        | Func\<Actor, BaseSimObject\>                         | public | False | 获取Actor的attackedBy字段值                                      |
| get_attackTarget      | Func\<Actor, BaseSimObject\>                         | public | False | 获取Actor的attackTarget字段值                                    |
| get_attackTimer       | Func<Actor, float\>                                  | public | False | 获取Actor的attackTimer字段值                                     |
| get_activeStatus_dict | Func<Actor, Dictionary\<string, StatusEffectData\>\> | public | False | 获取Actor的activeStatus_dict字段值                               |
| get_is_moving         | Func<Actor, bool>                                    | public | False | 获取Actor的is_moving字段值                                       |
| set_attackTimer       | Action<Actor, float>                                 | public | False | 设置Actor的attackTimer字段值                                     |
| set_professionAsset   | Action<Actor, ProfessionAsset>                       | public | False | 设置Actor的professionAsset字段值                                 |
| set_attackedBy        | Action<Actor, BaseSimObject>                         | public | False | 设置Actor的attackedBy字段值                                      |
| func_setKingdom       | Action<Actor, Kingdom>                               | public | False | 等同于Actor.setKingdom                                        |
| func_setBodySprite    | Action<Actor, Sprite>                                | public | False | 等同于Actor.setBodySprite                                     |
| func_spawnOn          | Action<Actor, WorldTile, float>                      | public | False | 等同于Actor.spawnOn                                           |
| func_haveStatus       | Func<Actor, string, bool>                            | public | False | 等同于Actor.haveStatus                                        |
| func_tryToAttack      | Func<Actor, BaseSimObject, bool>                     | public | False | 等同于Actor.tryToAttack                                       |
| func_canAttackTarget  | Func<Actor, BaseSimObject, bool>                     | public | False | 等同于Actor.canAttackTarget                                   |

## 方法概览

| 方法名                                          |                         返回类型                         |                                                       参数类型                                                       |  访问权限  | 实例成员 | 功用                              | 注意点                                 |
|:---------------------------------------------|:----------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------:|:------:|:----:|---------------------------------|-------------------------------------|
| is_in_battle                                 |                         bool                         |                                                        -                                                         | public | True | 是否处在战斗状态                        | 战斗状态的判定为一倍速的三秒内是否有受到攻击或进行攻击         |
| is_in_default_attack_range                   |                         bool                         |                                                  BaseSimObject                                                   | public | True | 目标是否在原版攻击范围内                    |                                     |
| add_cultisys                                 |                          -                           |                                                      string                                                      | public | True | 按照id强制添加修炼体系                    |                                     |
| try_to_set_attack_target_by<br />attacked_by |                          -                           |                                                        -                                                         | public | True | 根据当前攻击目标的状态决定<br />并设置受击来源为攻击目标 |                                     |
| start_color_effect                           |                          -                           |                                                  string, float                                                   | public | True | 设置人物的颜色效果                       | 会被其他效果覆盖，且目前只支持grey                 |
| has_cultisys                                 |                         bool                         |                                                      string                                                      | public | True | 判断是否拥有某个修炼体系                    |                                     |
| remove_status_effect_forcely                 |                          -                           |                                                      string                                                      | public | True | 移除状态效果                          | 会触发状态效果的结束函数                        |
| add_status_effect                            |   [CW_StatusEffectData](./CW_StatusEffectData.md)    |                                          string, string, BaseSimObject                                           | public | True | 添加人物状态                          | 当状态的动画达到上限时会添加失败；<br />当状态重复时会添加失败； |
| clear_default_spell_timer                    |                          -                           |                                                        -                                                         | public | True | 清空默认法术冷却                        | 仅作用于默认法术                            |
| get_hit                                      |                          -                           | float, bool,<br />[Others.CW_Enums.CW_AttackType](../Others/CW_Enums/CW_AttackType.md),<br />BaseSimObject, bool | public | True | 受击                              |                                     |
| get_fixed_base_stats                         |          [CW_BaseStats](./CW_BaseStats.md)           |                                                        -                                                         | public | True | 获取固定的属性加成                       |                                     |
| check_level_up                               |                          -                           |                                                        -                                                         | public | True | 对各个体系尝试晋级                       |                                     |
| change_special_body                          |                          -                           |                        [Library.CW_Asset_SpecialBody](../Library/CW_Asset_SpecialBody.md)                        | public | True | 强制改变体质                          |                                     |
| change_special_body                          |                          -                           |                                                      string                                                      | public | True | 强制改变体质                          |                                     |
| learn_spells                                 |                          -                           |                          List\<[Library.CW_Asset_Spell](../Library/CW_Asset_Spell.md)>                           | public | True | 学习列表中的所有法术                      | 会筛除其中无法学习的部分                        |
| learn_spells                                 |                          -                           |                                                     string[]                                                     | public | True | 学习数组中所有的法术                      | 同上                                  |
| learn_cultibook                              |                          -                           |                                                      string                                                      | public | True | 学习功法                            | 不强制，当生物无法修炼时不可学习                    |
| regen_health                                 |                          -                           |                                                   float, float                                                   | public | True | 给定数值与等级，恢复气血                    |                                     |
| regen_wakan                                  |                          -                           |                                                   float, float                                                   | public | True | 给定数值与等级，恢复灵气                    |                                     |
| get_weapon_asset                             | [Library.CW_Asset_Item](../Library/CW_Asset_Item.md) |                                                        -                                                         | public | True | 获取武器Asset                       |                                     |
