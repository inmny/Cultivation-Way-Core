﻿using System.Collections.Generic;
using Cultivation_Way.Library;
using Cultivation_Way.Others;
using Cultivation_Way.Test;

namespace Cultivation_Way.Utils.General.AboutSpell;

public static class CostChecks
{
    /// <summary>
    ///     按cost_list生成消耗检查与应用的委托
    /// </summary>
    /// <param name="cost_list"></param>
    /// <returns></returns>
    public static SpellCheck generate_spell_cost_action(KeyValuePair<string, float>[] cost_list)
    {
        #region TODO:通过Expression Tree将cost_list展开

        /*
        var param_spell_asset = Expression.Parameter(typeof(CW_SpellAsset), "spell_asset");
        var param_user = Expression.Parameter(typeof(BaseSimObject), "user");

        var final_expression = Expression.Block();

        List<Expression> expressions = new();

        LabelTarget return_label = Expression.Label(typeof(float));

        foreach (KeyValuePair<string, float> key_cost_pair in cost_list)
        {
            switch (key_cost_pair.Key)
            {
                case "health":
                case "age":
                    expressions.Add(
                    Expression.IfThenElse(
                        Expression.GreaterThanOrEqual(Expression.Field(Expression.Field(param_user, "data"), key_cost_pair.Key), Expression.Constant(key_cost_pair.Value)),
                        Expression.SubtractAssign(Expression.Field(Expression.Field(param_user, "data"), key_cost_pair.Key), Expression.Constant(key_cost_pair.Value)),
                        Expression.Return(return_label, Expression.Constant(-1))
                        )
                    );
                    break;
                default:
                    expressions.Add(
                    Expression.
                    );
                    break;
            }
        }
        */

        #endregion

        SpellCheck delegate_ret = delegate(CW_SpellAsset spell_asset, BaseSimObject user, BaseSimObject target)
        {
            foreach (KeyValuePair<string, float> key_cost_pair in cost_list)
            {
                switch (key_cost_pair.Key)
                {
                    case "health":
                        if (user.base_data.health * key_cost_pair.Value < spell_asset.minimum_cost) return -1;
                        break;
                    default:
                        user.base_data.get(key_cost_pair.Key, out float _value, -1);
                        if (_value * key_cost_pair.Value < spell_asset.minimum_cost) return -1;
                        break;
                }
            }

            float cost = 0;
            foreach (KeyValuePair<string, float> key_cost_pair in cost_list)
            {
                float tmp;
                switch (key_cost_pair.Key)
                {
                    case "health":
                        tmp = user.base_data.health * key_cost_pair.Value;
                        user.base_data.health -= (int)tmp;
                        break;
                    default:
                        user.base_data.get(key_cost_pair.Key, out float _value, -1);
                        tmp = _value * key_cost_pair.Value;
                        user.base_data.set(key_cost_pair.Key, _value - tmp);
                        break;
                }

                cost += tmp;
            }

            return cost;
        };
        if (Config.isEditor)
        {
            SpellTest.SpellCostDict.Add(delegate_ret, cost_list);
        }

        return delegate_ret;
    }
}