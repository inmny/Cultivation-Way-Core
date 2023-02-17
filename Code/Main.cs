using System;
using NCMS;
using UnityEngine;
using Cultivation_Way.Content;
using Cultivation_Way.Library;
using System.Collections.Generic;
using System.Reflection;
using Cultivation_Way.Animation;

using System.Runtime.InteropServices;
namespace Cultivation_Way{
    public enum Load_Object_Reason
    {
        SPAWN,
        LOAD_SAVES
    }
    public enum Loading_Save_Type
    {
        ORIGIN,
        CW
    }
    public enum Destroy_Unit_Reason
    {
        KILL,
        CLEAR
    }

    public class ModState
    {
        public static ModState instance;
        public List<CW_Addon> addons;
        public bool registered = false;
        public string cur_language = "cz";
        public string map_mode = "";
        public NCMod mod_info;
        internal CW_Spell_Manager spell_manager;
        public CW_EffectManager effect_manager;
        public CW_Library_Manager library_manager;
        public Destroy_Unit_Reason destroy_unit_reason = Destroy_Unit_Reason.KILL;
        public Loading_Save_Type loading_save_type = Loading_Save_Type.CW;
        public Load_Object_Reason load_object_reason = Load_Object_Reason.LOAD_SAVES;
    }
    public class World_Data
    {
        public static World_Data instance;
        public CW_MapChunk_Manager map_chunk_manager;
    }
    [ModEntry]
    public class Main : MonoBehaviour{
        public static Main instance { get; private set; }
        public ModState mod_state;
        public World_Data world_data;
        public CW_WorldConfig world_config;
        internal bool initialized = false;
        private bool mod_state_prepared = false;
        private int last_month;
        void Awake(){
            instance = this;
            if (!mod_state_prepared)
            {
                mod_state_prepared = true;
                mod_state = new ModState();
                ModState.instance = mod_state;
                mod_state.addons = new List<CW_Addon>();
            }
            print("[CW Core]: Awake");

        }
        void Update()
        {
            if (!initialized)
            {
                initialized = true;
                CW_Library_Manager.create();
                world_data = new World_Data();
                world_config = new CW_WorldConfig();

                foreach (NCMod ncmod in NCMS.ModLoader.Mods)
                {
                    if (ncmod.name == Others.CW_Constants.mod_name) { mod_state.mod_info = ncmod; break; }
                }

                World_Data.instance = world_data;
                mod_state.spell_manager = new CW_Spell_Manager();
                CW_Spell_Manager.instance = mod_state.spell_manager;
                mod_state.effect_manager = this.gameObject.AddComponent<CW_EffectManager>();
                mod_state.library_manager = CW_Library_Manager.instance;
                mod_state.library_manager.init();

                world_data.map_chunk_manager = new CW_MapChunk_Manager();
                
                Utils.CW_ItemTools.init();
                W_Content_Manager.add_content();

                world_data.map_chunk_manager.init(32, 32);
                //mod_state.library_manager.register();
                print("[CW Core]: Finish Initialization");
            }
            if (!mod_state.registered)
            {
                bool all_addons_loaded = true;
                foreach(CW_Addon addon in mod_state.addons)
                {
                    if (!addon.initialized) all_addons_loaded = false;
                }
                if (all_addons_loaded)
                {
                    mod_state.registered = true;
                    mod_state.library_manager.register();
                    W_Content_Tab.apply_buttons();
                }
            }
            if (last_month!=MapBox.instance.mapStats.month)
            {
                world_data.map_chunk_manager.update();
                last_month = MapBox.instance.mapStats.month;
            }
            
        }
        void OnDisable()
        {
            Window_Cultisys_Name_Setting.save_to_file();
            Window_Cultisys_Stats_Setting.save_to_file();
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string winName);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlage, uint timeout, IntPtr result);

        //���Ҵ��ڵ�ί�� �����߼�
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc proc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string winName);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hwnd, IntPtr parentHwnd);
        /// <summary>
        /// �����й�ָ�����ڵ���Ϣ���ú���������ָ����ƫ������ֵ����������Ĵ����ڴ��С�
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return GetWindowLongPtr32(hWnd, nIndex);
        }
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern IntPtr GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        /// <summary>
        /// ���־�̬�����Ǳ���ģ���Ϊ���ڲ���ϵͳ��֧��
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="nIndex"></param>
        /// <param name="dwNewLong"></param>
        /// <returns></returns>
        public static IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(HandleRef hWnd, int nIndex, IntPtr dwNewLong);
        /// <summary>
        /// ����һ���µĴ�����ʽ
        /// </summary>
        public const int GWL_STYLE = -16;

        /// <summary>
        /// ����
        /// </summary>
        public const uint WS_CAPTION = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */

        /// <summary>
        /// ϵͳ�˵�
        /// </summary>
        public const uint WS_SYSMENU = 0x00080000;


        /// <summary>
        /// ���õ������ڴ�С�ĺ�Ľṹ�Ĵ��ڡ�
        /// </summary>
        public const uint WS_THICKFRAME = 0x00040000;

        /// <summary>
        /// ����һ���ɵ��߿�Ĵ��ڣ��� WS_THICKFRAME �����ͬ
        /// </summary>
        public const uint WS_SIZEBOX = WS_THICKFRAME;
        internal void hide_window()
        {
            // ���洰�ھ�������ⲿ���壬���ں��潫�����Լ��Ĵ�����Ϊ�Ӵ��ڷ���
            IntPtr programHandle = FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;
            // �� Program Manager ���ڷ�����Ϣ 0x52c ��һ����Ϣ����ʱ����Ϊ2��
            SendMessageTimeout(programHandle, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 2, result);

            // ������������
            EnumWindows((hwnd, lParam) =>
            {
                // �ҵ���һ�� WorkerW ���ڣ��˴��������Ӵ��� SHELLDLL_DefView�����������Ӵ���
                if (FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    // �ҵ���ǰ��һ�� WorkerW ���ڵģ���һ�����ڣ����ڶ��� WorkerW ���ڡ�
                    IntPtr tempHwnd = FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

                    // ���صڶ��� WorkerW ����
                    ShowWindow(tempHwnd, 0);
                }
                return true;
            }, IntPtr.Zero);

            IntPtr cur_win_handle = GetForegroundWindow();

            HandleRef ParentHandle = new HandleRef(this, cur_win_handle);

            // ��ʼ�����ڷ��
            IntPtr Style = GetWindowLongPtr(cur_win_handle, GWL_STYLE);
            uint tempStyle = (uint)Style.ToInt32() & ~WS_CAPTION & ~WS_SYSMENU & ~WS_SIZEBOX;
            SetWindowLongPtr(ParentHandle, GWL_STYLE, new IntPtr(tempStyle));

            SetParent(cur_win_handle, FindWindow("Progman", "Program Manager"));
            // ���ؿ�����
            MapBox.instance.selectedButtons.unselectAll();
            MapBox.instance.selectedButtons.unselectTabs();
        }
    }
}
