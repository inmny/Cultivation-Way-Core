﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace Cultivation_Way.Utils;

internal static class ReflectionHelper
{
    internal static Delegate get_method<T>(string method_name, bool is_static = false)
    {
        return createMethodDelegate(is_static
            ? typeof(T).GetMethod(method_name, BindingFlags.Static | BindingFlags.NonPublic)
            : AccessTools.Method(typeof(T), method_name));
    }

    internal static Func<InstanceType, OutType> create_getter<InstanceType, OutType>(string field_name)
    {
        FieldInfo field =
            typeof(InstanceType).GetField(field_name,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) ??
            AccessTools.Field(typeof(InstanceType), field_name);
        if (field == null)
            MonoBehaviour.print("Cannot find '" + field_name + "' in type " + typeof(InstanceType).FullName);
        try
        {
            ParameterExpression instance = Expression.Parameter(typeof(InstanceType), "instance");
            UnaryExpression instanceCast =
                !field.DeclaringType.IsValueType
                    ? Expression.TypeAs(instance, field.DeclaringType)
                    : Expression.Convert(instance, field.DeclaringType);
            Func<InstanceType, OutType> GetDelegate;
            if (typeof(OutType).IsPrimitive)
            {
                GetDelegate =
                    Expression.Lambda<Func<InstanceType, OutType>>(
                            Expression.Field(instanceCast, field),
                            instance)
                        .Compile();
            }
            else
            {
                GetDelegate =
                    Expression.Lambda<Func<InstanceType, OutType>>(
                            Expression.TypeAs(
                                Expression.Field(instanceCast, field),
                                typeof(OutType)),
                            instance)
                        .Compile();
            }

            return GetDelegate;
        }
        catch (Exception)
        {
            Debug.LogError("Expression Tree-Getter:" + field.DeclaringType + "::" + field_name);
            return null;
        }
    }

    internal static Action<TI, TF> create_setter<TI, TF>(string field_name)
    {
        FieldInfo field = typeof(TI).GetField(field_name,
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        ParameterExpression instance = Expression.Parameter(typeof(TI), "instance");
        ParameterExpression parameter = Expression.Parameter(typeof(TF), field_name);
        return Expression.Lambda<Action<TI, TF>>(Expression.Assign(Expression.Field(instance, field), parameter),
            instance, parameter).Compile();
    }

    private static Delegate createMethodDelegate(MethodInfo method_info)
    {
        List<ParameterExpression> paramExpressions = method_info.GetParameters()
            .Select((p, i) => Expression.Parameter(p.ParameterType, p.Name)).ToList();

        MethodCallExpression callExpression;
        if (method_info.IsStatic)
        {
            callExpression = Expression.Call(method_info, paramExpressions);
        }
        else
        {
            ParameterExpression instanceExpression = Expression.Parameter(method_info.ReflectedType, "instance");
            callExpression = Expression.Call(instanceExpression, method_info, paramExpressions);
            paramExpressions.Insert(0, instanceExpression);
        }

        LambdaExpression lambdaExpression = Expression.Lambda(callExpression, paramExpressions);
        return lambdaExpression.Compile();
    }
}