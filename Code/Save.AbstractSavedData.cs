using Cultivation_Way.Animation;
using Cultivation_Way.Library;

namespace Cultivation_Way.Save;

internal class AbstractSavedData
{
    public int cw_save_version = Constants.Core.save_version;
    public int origin_save_version = 0;

    public virtual void load_to_world(SaveManager save_manager, SavedMap origin_data)
    {
    }

    public void clear_cw_world()
    {
        Manager.cultibooks.reset();
        Manager.bloods.reset();
        EffectManager.instance.clear();
        CW_Core.mod_state.spell_manager.clear();
    }
}