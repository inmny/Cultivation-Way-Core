using System;
using System.Reflection;
using ModDeclaration;
using UnityEngine;

namespace Cultivation_Way.Addon;

public abstract class CW_Addon : MonoBehaviour
{
    private string _adapted_core_version = "";
    private bool _disabled;
    private bool _loaded;

    internal bool initialized;

    /// <summary>
    ///     此Mod, NCMS提供的全局Mod在编辑器中报错, 膈应.
    /// </summary>
    public _Mod this_mod { get; } = new();

    /// <summary>
    ///     此附属名称, 取自mod.json中名称一项
    /// </summary>
    public string addon_name { get; private set; }

    private void Awake()
    {
        if (_loaded) return;
        _loaded = true;
        awake();
        load_addon_info();
    }

    private void Update()
    {
        if (_disabled) return;
        if (initialized)
        {
            update(Time.fixedDeltaTime);
            return;
        }

        if (!CW_Core.mod_state.core_initialized) return;

        initialized = true;
        addon_name = this_mod.info.Name.Replace($"{CW_Core.mod_state.mod_info.Name}.postload", "");
        try
        {
            initialize();
        }
        catch (Exception e)
        {
            _disabled = true;
            if (_adapted_core_version != CW_Core.mod_state.mod_info.Version)
            {
                if (!string.IsNullOrEmpty(_adapted_core_version))
                    warn($"适配于{_adapted_core_version}版本核心, 当前核心版本{CW_Core.mod_state.mod_info.Version}");
                else
                {
                    warn($"适配核心版本未知, 当前核心版本{CW_Core.mod_state.mod_info.Version}");
                }
            }

            error(e.Message);
            error(e.StackTrace);
        }
    }

    /// <summary>
    ///     加载附属信息, 并加入addons
    /// </summary>
    private void load_addon_info()
    {
        Assembly assembly = GetType().Assembly;
        Type mod_type = assembly.GetType("Mod");
        this_mod.game_object = gameObject;
        this_mod.info = mod_type.GetField("Info").GetValue(null) as Info;
        if (this_mod.info == null)
        {
            _disabled = true;
            return;
        }

        CW_Core.mod_state.addons.Add(this);
    }

    /// <summary>
    ///     设置适配的核心版本
    /// </summary>
    /// <param name="version">版本</param>
    public void set_adapted_core_version(string version)
    {
        _adapted_core_version = version;
    }

    public void log(string msg)
    {
        Logger.Log($"[CW.{addon_name}]: {msg}");
    }

    public void warn(string msg)
    {
        Logger.Warn($"[CW.{addon_name}]: {msg}");
    }

    public void error(string msg)
    {
        Logger.Error($"[CW.{addon_name}]: {msg}");
    }

    public virtual void awake()
    {
    }

    /// <summary>
    ///     初始化
    /// </summary>
    public abstract void initialize();

    /// <summary>
    ///     按elapsed间隔调用的更新函数
    /// </summary>
    /// <param name="elapsed">(1/60s, \infty)</param>
    public virtual void update(float elapsed)
    {
    }

    // ReSharper disable once InconsistentNaming
    public class _Mod
    {
        public GameObject game_object;
        public Info info;
    }
}