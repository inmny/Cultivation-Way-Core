namespace Cultivation_Way.Save;

internal static class CW_SaveManager
{
    private static readonly int cur_cw_save_version = Constants.Core.save_version;
    internal static AbstractSavedData _data;

    public static void init()
    {
    }

    public static void PatchLoadMethods(SaveManager pSaveManager, SavedMap pVanillaData, AbstractSavedData pSavedData)
    {
        for (var i = SmoothLoader._index; i < SmoothLoader._actions.Count; i++)
        {
            var action_container = SmoothLoader._actions[i];
            switch (action_container.id)
            {
                case "Clearing World":
                    InsertAction(() => { pSavedData.BeforeAll(pSaveManager, pVanillaData); }, "Preload CW World", i++);
                    break;
                case "Setting Map Size":
                    InsertAction(pSavedData.clear_cw_world, "Clear CW World", i++);
                    break;
                case "Loading Cultures":
                    InsertAction(pSavedData.LoadWorld, "Load CW World", i++);
                    break;
                case "Finishing up...":
                    InsertAction(() =>
                    {
                        pSavedData.AfterAll(pSaveManager);
                        _data = null;
                    }, "Post finish CW World", i++);
                    break;
            }
        }
    }

    private static void InsertAction(MapLoaderAction pAction, string pId, int pIndex)
    {
        SmoothLoader._actions.Insert(pIndex, new MapLoaderContainer(pAction, pId, true, 0.0001f));
        SmoothLoader._added++;
    }
}