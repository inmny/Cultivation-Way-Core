using System;
using System.Security.Cryptography;
using System.Text;
using Cultivation_Way.Core;
using Cultivation_Way.Extension;
using Cultivation_Way.Library;

namespace Cultivation_Way.Implementation;

internal static class Achievements
{
    public static Achievement lost;
    public static Achievement complete;

    public static void init()
    {
        AchievementGroupAsset cw_group = new()
        {
            id = "cw_achievement_group"
        };
        AssetManager.achievementGroups.add(cw_group);

        Achievement achievement;
        achievement = new Achievement
        {
            playStoreID = "CgkIia6M98wfEAIQAg",
            steamID = "",
            id = Constants.Core.mod_prefix + "achievementLost",
            hidden = true,
            icon = "iconAbout",
            group = "cw_achievement_group"
        };
        add(achievement);
        lost = achievement;
        achievement = new Achievement
        {
            playStoreID = "CgkIia6M98wfEAIQAg",
            steamID = "",
            id = Constants.Core.mod_prefix + "achievementComplete",
            hidden = true,
            icon = "iconAbout",
            group = "cw_achievement_group",
            action = (city, actor, power) =>
            {
                if (Config.selectedUnit == null) return false;

                CW_Actor cw_actor = (CW_Actor)Config.selectedUnit;

                CW_Element element = cw_actor.data.get_element();
                string str = "";
                for (int idx = 0; idx < element.base_elements.Length; idx++)
                {
                    if (element.base_elements[idx] > 0)
                    {
                        str += element.base_elements[idx] + ",";
                    }
                }

                str = str.Substring(0, str.Length - 1);

                MD5 md5_service = MD5.Create();
                byte[] hash = md5_service.ComputeHash(Encoding.UTF8.GetBytes(str));

                StringBuilder result_str = new();
                result_str.Append(BitConverter.ToString(hash).Replace("-", ""));

                Cultibook cultibook = cw_actor.data.get_cultibook();
                if (cultibook == null)
                {
                    str = "";
                }
                else
                {
                    str = cultibook.author_name + "_" + cultibook.editor_name + "_" + cultibook.name;
                }

                hash = md5_service.ComputeHash(Encoding.UTF8.GetBytes(str));
                result_str.Append(BitConverter.ToString(hash).Replace("-", ""));

                int[] cultisys_level = cw_actor.data.get_cultisys_level();
                str = "";
                for (int idx = 0; idx < 3; idx++)
                {
                    str += cultisys_level[idx] + ",";
                }

                str = str.Substring(0, str.Length - 1);
                hash = md5_service.ComputeHash(Encoding.UTF8.GetBytes(str));
                result_str.Append(BitConverter.ToString(hash).Replace("-", ""));

                str = cw_actor.getName();
                hash = md5_service.ComputeHash(Encoding.UTF8.GetBytes(str));
                result_str.Append(BitConverter.ToString(hash).Replace("-", ""));

                BloodNodeAsset main_blood = cw_actor.data.get_main_blood();
                if (main_blood == null)
                {
                    str = "";
                }
                else
                {
                    str = main_blood.ancestor_data.name;
                }

                hash = md5_service.ComputeHash(Encoding.UTF8.GetBytes(str));
                result_str.Append(BitConverter.ToString(hash).Replace("-", ""));
                // 期望结果, 总共160位, 只需要32位相同即可
                // 这里分割只是为了好看, 并不是有实际意义的分割, 如果你不信, 那就随你吧
                string expected_result = "8F3775FFDCB19E793D700BD73FB7290F15CA5895FF" +
                                         "36856E88CA2927BCF99F5983FE735" +
                                         "88B42954383031DEBA8698479FC450260A382F37681593FB56BD7647EA6A137" +
                                         "3C80813DC431D96743704E0A2B";

                int[,] equal_len = new int[expected_result.Length, expected_result.Length];
                for (int i = 0; i < expected_result.Length; ++i)
                {
                    for (int j = 0; j < expected_result.Length; ++j)
                    {
                        equal_len[i, j] = 0;
                    }
                }

                for (int i = 0; i < expected_result.Length; ++i)
                {
                    for (int j = 0; j < result_str.Length; ++j)
                    {
                        if (expected_result[i] == result_str[j])
                        {
                            if (i == 0 || j == 0)
                            {
                                equal_len[i, j] = 1;
                            }
                            else
                            {
                                equal_len[i, j] = equal_len[i - 1, j - 1] + 1;
                            }
                        }
                        else
                        {
                            equal_len[i, j] = 0;
                        }

                        if (equal_len[i, j] >= 32)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        };
        add(achievement);
        complete = achievement;
    }

    private static void add(Achievement a)
    {
        AssetManager.achievements.add(a);
        AssetManager.achievementGroups.get(a.group).achievementList.Add(a);
    }
}