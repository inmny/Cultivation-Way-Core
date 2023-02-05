# CW_Actor	人物拓展

这是模组中最为核心的部分

继承自Actor，游戏所有Actor均已转换为CW_Actor，可强制转换

## 字段

| 字段名                | 类型                                                 | 访问权限 | 实例成员 | 备注                                                                                                |
| --------------------- | ---------------------------------------------------- | -------- | :------: | --------------------------------------------------------------------------------------------------- |
| cw_stats              | [Library.CW_ActorStats](../Library/CW_ActorStats.md)    | public   |   True   | 该生物对应的拓展后的ActorStats                                                                      |
| cw_cur_stats          | [CW_BaseStats](./CW_BaseStats.md)                       | public   |   True   | 该生物的属性，其中basestats字段为<br />该生物的curStats的引用                                       |
| cw_data               | [CW_ActorData](./CW_ActorData.md)                       | public   |   True   | 该生物的数据                                                                                        |
| cw_status             | [CW_ActorStatus](./CW_ActorStatus.md)                   | public   |   True   | 该生物的拓展状态（如灵气、护盾、<br />元神等）                                                      |
| fast_data             | ActorStatus                                          | public   |   True   | 该生物的原版状态（如生命、性别等）<br />提供快速访问                                                |
| cur_spells            | List\<string\>                                       | public   |   True   | 该生物当前可释放的法术的id                                                                          |
| can_act               | bool                                                 | public   |   True   | 该生物当前是否可以行动，一般由石化、<br />藤缚术等修改，在所有束缚状态效果<br />结束后会自动置False |
| get_attackedBy        | Func\<Actor, BaseSimObject\>                         | public   |  False  | 获取Actor的attackedBy字段值                                                                         |
| get_attackTarget      | Func\<Actor, BaseSimObject\>                         | public   |  False  | 获取Actor的attackTarget字段值                                                                       |
| get_attackTimer       | Func<Actor, float\>                                  | public   |  False  | 获取Actor的attackTimer字段值                                                                        |
| get_activeStatus_dict | Func<Actor, Dictionary\<string, StatusEffectData\>\> | public   |  False  | 获取Actor的activeStatus_dict字段值                                                                  |
| get_is_moving         | Func<Actor, bool>                                    | public   |  False  | 获取Actor的is_moving字段值                                                                          |
| set_attackTimer       | Action<Actor, float>                                 | public   |  False  | 设置Actor的attackTimer字段值                                                                        |
| set_professionAsset   | Action<Actor, ProfessionAsset>                       | public   |  False  | 设置Actor的professionAsset字段值                                                                    |
| set_attackedBy        | Action<Actor, BaseSimObject>                         | public   |  False  | 设置Actor的attackedBy字段值                                                                         |
| func_setKingdom       | Action<Actor, Kingdom>                               | public   |  False  | 等同于Actor.setKingdom                                                                              |
| func_setBodySprite    | Action<Actor, Sprite>                                | public   |  False  | 等同于Actor.setBodySprite                                                                           |
| func_spawnOn          | Action<Actor, WorldTile, float>                      | public   |  False  | 等同于Actor.spawnOn                                                                                 |
| func_haveStatus       | Func<Actor, string, bool>                            | public   |  False  | 等同于Actor.haveStatus                                                                              |
| func_tryToAttack      | Func<Actor, BaseSimObject, bool>                     | public   |  False  | 等同于Actor.tryToAttack                                                                             |
| func_canAttackTarget  | Func<Actor, BaseSimObject, bool>                     | public   |  False  | 等同于Actor.canAttackTarget                                                                         |
