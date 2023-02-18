/** 高开销的灵气更新
                    chunk_1 = chunks[i, j]; chunk_2 = chunks[i, j + 1];
                    delta_wakan = chunk_2.wakan - chunk_1.wakan;
                    delta_wakan_level = chunk_2.wakan_level - chunk_1.wakan_level;

                    delta_wakan_1 = delta_wakan * delta_wakan_level * Others.CW_Constants.chunk_wakan_spread_grad * Mathf.Pow(Others.CW_Constants.wakan_level_co, delta_wakan_level * Others.CW_Constants.chunk_wakan_level_spread_grad - chunk_1.wakan_level);

                    delta_wakan_2 = delta_wakan * delta_wakan_level * Others.CW_Constants.chunk_wakan_spread_grad;

                    delta_wakan_level_2 = delta_wakan_level * Others.CW_Constants.chunk_wakan_level_spread_grad;

                    delta_wakan_level_1 = Mathf.Log(
                        (chunk_1.wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, chunk_1.wakan_level)+chunk_2.wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, chunk_2.wakan_level)-(chunk_2.wakan+delta_wakan_2)*Mathf.Pow(Others.CW_Constants.wakan_level_co, chunk_2.wakan_level + delta_wakan_level_2))/(chunk_1.wakan + delta_wakan_1) , 
                        Others.CW_Constants.wakan_level_co);

                    chunk_1.tmp_wakan += delta_wakan_1;
                    chunk_2.tmp_wakan += delta_wakan_2;
                    chunk_1.total_wakan += delta_wakan_level_1;
                    chunk_2.total_wakan += delta_wakan_level_2;
*/
/** 建筑周围地块检查
internal bool checkTilesForUpgrade(WorldTile pTile, BuildingAsset pTemplate)
{
	int num = pTile.pos.x - pTemplate.fundament.left;
	int num2 = pTile.pos.y - pTemplate.fundament.bottom;
	int num3 = pTemplate.fundament.right + pTemplate.fundament.left + 1;
	int num4 = pTemplate.fundament.top + pTemplate.fundament.bottom + 1;
	for (int i = 0; i < num3; i++)
	{
		for (int j = 0; j < num4; j++)
		{
			WorldTile tile = this.world.GetTile(num + i, num2 + j);
			if (tile == null)
			{
				MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of tile==null", this.stats.id, pTile.x, pTile.y));
				return false;
			}
			if (!tile.Type.canBuildOn)
			{
				MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of !tile.Type.canBuildOn", this.stats.id, pTile.x, pTile.y));
				return false;
			}
			if (tile.zone.city != this.city)
			{
				MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of tile.zone.city != this.city", this.stats.id, pTile.x, pTile.y));
				return false;
			}
			Building building = tile.building;
			if (building != null && building != this)
			{
				if (building.stats.priority >= this.stats.priority)
				{
					MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of lower priority than '{3}', {4}<={5}", new object[]
					{
						this.stats.id,
						pTile.x,
						pTile.y,
						building.stats.id,
						this.stats.priority,
						building.stats.priority
					}));
					return false;
				}
				if (building.stats.upgradeLevel >= this.stats.upgradeLevel)
				{
					MonoBehaviour.print(string.Format("'{0}' at ({1},{2}) cannot upgrade because of lower upgradeLevel than '{3}', {4}<={5}", new object[]
					{
						this.stats.id,
						pTile.x,
						pTile.y,
						building.stats.id,
						this.stats.upgradeLevel,
						building.stats.upgradeLevel
					}));
					return false;
				}
			}
		}
	}
	return true;
}
*/
/** 嵌入桌面
[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string className, string winName);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlage, uint timeout, IntPtr result);

        //查找窗口的委托 查找逻辑
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
        /// 检索有关指定窗口的信息。该函数还将以指定的偏移量将值检索到额外的窗口内存中。
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
        /// 这种静态方法是必需的，因为早期操作系统不支持
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
        [DllImport("Shell32.dll")]
        static extern int SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            public char szDisplayName;
            public char szTypeName;
        }

        /// <summary>
        /// 从文件扩展名得到文件关联图标
        /// </summary>
        /// <param name="fileName">文件名或文件扩展名</param>
        /// <param name="smallIcon">是否是获取小图标，否则是大图标</param>
        /// <returns>图标</returns>
        static public Icon GetFileIcon(string fileName, bool smallIcon)
        {
            //也可用自带的
            //Icon icon = Icon.ExtractAssociatedIcon(fileName);
            SHFILEINFO fi = new SHFILEINFO();
            Icon ic = null;
            //SHGFI_ICON + SHGFI_USEFILEATTRIBUTES + SmallIcon   
            int iTotal = (int)SHGetFileInfo(fileName, 100, ref fi, 0, (uint)(smallIcon ? 273 : 272));
            if (iTotal > 0)
            {
                ic = Icon.FromHandle(fi.hIcon);
            }
            return ic;
        }
        /// <summary>
        /// 设置一个新的窗口样式
        /// </summary>
        public const int GWL_STYLE = -16;

        /// <summary>
        /// 标题
        /// </summary>
        public const uint WS_CAPTION = 0x00C00000;     // WS_BORDER | WS_DLGFRAME

/// <summary>
/// 系统菜单
/// </summary>
public const uint WS_SYSMENU = 0x00080000;


/// <summary>
/// 设置调整窗口大小的厚的结构的窗口。
/// </summary>
public const uint WS_THICKFRAME = 0x00040000;

/// <summary>
/// 创建一个可调边框的窗口，与 WS_THICKFRAME 风格相同
/// </summary>
public const uint WS_SIZEBOX = WS_THICKFRAME;
internal void hide_window()
{
    // 桌面窗口句柄，在外部定义，用于后面将我们自己的窗口作为子窗口放入
    IntPtr programHandle = FindWindow("Progman", null);

    IntPtr result = IntPtr.Zero;
    // 向 Program Manager 窗口发送消息 0x52c 的一个消息，超时设置为2秒
    SendMessageTimeout(programHandle, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 2, result);

    // 遍历顶级窗口
    EnumWindows((hwnd, lParam) =>
    {
        // 找到第一个 WorkerW 窗口，此窗口中有子窗口 SHELLDLL_DefView，所以先找子窗口
        if (FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
        {
            // 找到当前第一个 WorkerW 窗口的，后一个窗口，及第二个 WorkerW 窗口。
            IntPtr tempHwnd = FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

            // 隐藏第二个 WorkerW 窗口
            ShowWindow(tempHwnd, 0);
        }
        return true;
    }, IntPtr.Zero);

    IntPtr cur_win_handle = GetForegroundWindow();

    HandleRef ParentHandle = new HandleRef(this, cur_win_handle);

    // 初始化窗口风格
    IntPtr Style = GetWindowLongPtr(cur_win_handle, GWL_STYLE);
    uint tempStyle = (uint)Style.ToInt32() & ~WS_CAPTION & ~WS_SYSMENU & ~WS_SIZEBOX;
    SetWindowLongPtr(ParentHandle, GWL_STYLE, new IntPtr(tempStyle));

    SetParent(cur_win_handle, FindWindow("Progman", "Program Manager"));
    // 隐藏控制栏
    MapBox.instance.selectedButtons.unselectAll();
    MapBox.instance.selectedButtons.unselectTabs();
}
**/

