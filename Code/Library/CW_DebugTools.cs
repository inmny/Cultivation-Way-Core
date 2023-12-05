namespace Cultivation_Way.Library;

internal static class CW_DebugTools
{
    public static void init()
    {
        DebugToolAsset asset;

        asset = AssetManager.debug_tool_library.add(new DebugToolAsset
        {
            id = "Benchmark CW SpriteAnimation",
            show_benchmark_buttons = true,
            benchmark_group_id = "EffectManager.Update",
            benchmark_total = "EffectManager.Update",
            benchmark_total_group = "main",
            split_benchmark = true,
            action_start = AssetManager.debug_tool_library.setBenchmarksDefaultValue,
            action_1 = AssetManager.debug_tool_library.showGroupBenchmarkTop,
            action_2 = AssetManager.debug_tool_library.showGroupBenchmarkBottom
        });
    }
}