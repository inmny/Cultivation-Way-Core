using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Utils
{
    internal static class CW_ReflectionHelper
    {
        public static Delegate GetFastMethod(Type instanceType, string methodName, bool isStatic = false)
        {
            if (instanceType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic) == null) WorldBoxConsole.Console.print(methodName);
            if (isStatic)
            {
                return createMethodDelegate(instanceType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic));
            }
            return createMethodDelegate(HarmonyLib.AccessTools.Method(instanceType, methodName));
            if (isStatic)
            {
                return createMethodDelegate(instanceType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic));
            }
            return createMethodDelegate(instanceType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic));
        }
        internal static Func<InstanceType, OutType> create_getter<InstanceType, OutType>(string fieldName)
        {
            FieldInfo field = typeof(InstanceType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field == null) field = HarmonyLib.AccessTools.Field(typeof(InstanceType), fieldName);
            if (field == null) WorldBoxConsole.Console.print("Cannot find '" + fieldName + "' in type " + typeof(InstanceType).FullName);
            try
            {
                ParameterExpression instance = Expression.Parameter(typeof(InstanceType), "instance");
                UnaryExpression instanceCast =
                    !field.DeclaringType.IsValueType ?
                        Expression.TypeAs(instance, field.DeclaringType) :
                        Expression.Convert(instance, field.DeclaringType);

                Func<InstanceType, OutType> GetDelegate =
                    Expression.Lambda<Func<InstanceType, OutType>>(
                        Expression.TypeAs(
                            Expression.Field(instanceCast, field),
                            typeof(OutType)),
                        instance)
                    .Compile();

                return GetDelegate;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("Expression Tree-Getter:"+field.DeclaringType+"::"+fieldName);
                return null;
            }
            //}
        }
        internal static Func<InstanceType, int> create_getter_int<InstanceType>(string fieldName)
        {
            FieldInfo field = typeof(InstanceType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            try
            {
                ParameterExpression instance = Expression.Parameter(typeof(InstanceType), "instance");
                UnaryExpression instanceCast =
                    !field.DeclaringType.IsValueType ?
                        Expression.TypeAs(instance, field.DeclaringType) :
                        Expression.Convert(instance, field.DeclaringType);
                Func<InstanceType, int> GetDelegate =
                    Expression.Lambda<Func<InstanceType, int>>(
                            Expression.Field(instanceCast, field),
                        instance)
                    .Compile();
                return GetDelegate;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("Expression Tree-Getter int");
                return null;
            }
            //}
        }
        internal static Func<InstanceType, float> create_getter_float<InstanceType>(string fieldName)
        {
            FieldInfo field = typeof(InstanceType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            try
            {
                ParameterExpression instance = Expression.Parameter(typeof(InstanceType), "instance");
                UnaryExpression instanceCast =
                    !field.DeclaringType.IsValueType ?
                        Expression.TypeAs(instance, field.DeclaringType) :
                        Expression.Convert(instance, field.DeclaringType);
                Func<InstanceType, float> GetDelegate =
                    Expression.Lambda<Func<InstanceType, float>>(
                            Expression.Field(instanceCast, field),
                        instance)
                    .Compile();
                return GetDelegate;
            }
            catch (Exception)
            {
                UnityEngine.Debug.LogError("Expression Tree-Getter float");
                return null;
            }
            //}
        }
        internal static Func<InstanceType, bool> create_getter_bool<InstanceType>(string fieldName)
        {
            FieldInfo field = typeof(InstanceType).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            try
            {
                ParameterExpression instance = Expression.Parameter(typeof(InstanceType), "instance");
                UnaryExpression instanceCast =
                    !field.DeclaringType.IsValueType ?
                        Expression.TypeAs(instance, field.DeclaringType) :
                        Expression.Convert(instance, field.DeclaringType);
                Func<InstanceType, Boolean> GetDelegate =
                    Expression.Lambda<Func<InstanceType, Boolean>>(Expression.Field(instanceCast, field), instance)
                    .Compile();
                return GetDelegate;
            }
            catch (ArgumentNullException)
            {
                UnityEngine.Debug.LogError("Expression Tree-Getter bool argument null exception");
                return null;
            }
            catch (ArgumentException e)
            {
                UnityEngine.Debug.LogError("Expression Tree-Getter bool argument invalid exception for argument: " + e.ParamName + " in function: " + e.TargetSite);
                WorldBoxConsole.Console.print(e.StackTrace);
                return null;
            }
            //}
        }
        internal static Delegate createNewSetter<TI, TF>(Type instanceType, string fieldName)
        {
            FieldInfo field = instanceType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            ParameterExpression instance = Expression.Parameter(typeof(TI), "instance");
            ParameterExpression parameter = Expression.Parameter(typeof(TF), fieldName);
            return Expression.Lambda<Action<TI, TF>>(Expression.Assign(Expression.Field(instance, field), parameter), instance, parameter).Compile();
        }
        internal static Delegate createMethodDelegate(MethodInfo methodInfo)
        {
            List<ParameterExpression> paramExpressions = methodInfo.GetParameters().Select((p, i) =>
            {
                return Expression.Parameter(p.ParameterType, p.Name);
            }).ToList();

            MethodCallExpression callExpression;
            if (methodInfo.IsStatic)
            {
                callExpression = Expression.Call(methodInfo, paramExpressions);
            }
            else
            {
                ParameterExpression instanceExpression = Expression.Parameter(methodInfo.ReflectedType, "instance");
                callExpression = Expression.Call(instanceExpression, methodInfo, paramExpressions);
                paramExpressions.Insert(0, instanceExpression);
            }
            LambdaExpression lambdaExpression = Expression.Lambda(callExpression, paramExpressions);
            return lambdaExpression.Compile();
        }

    }
}
