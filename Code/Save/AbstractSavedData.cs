using Cultivation_Way.Animation;
using Cultivation_Way.Library;

namespace Cultivation_Way.Save;

public abstract class AbstractSavedData
{
    public int cw_save_version = Constants.Core.save_version;
    public int origin_save_version;

    public virtual void load_to_world(SaveManager save_manager, SavedMap origin_data)
    {
    }

    public abstract void LoadWorld();

    public virtual void BeforeAll(SaveManager pSaveManager, SavedMap pVanillaData)
    {
    }

    public virtual void AfterAll(SaveManager pSaveManager)
    {
    }

    public void clear_cw_world()
    {
        Manager.cultibooks.reset();
        Manager.bloods.reset();
        Manager.cultisys.ClearForNewGame();
        EffectManager.instance.clear();
        CW_Core.mod_state.spell_manager.clear();
    }
}