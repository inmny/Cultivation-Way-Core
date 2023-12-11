using System.Linq;
using Cultivation_Way.Library;

namespace Cultivation_Way.Core;

public partial class CW_Actor
{
    private void __check_level_up__(string cultisys_id)
    {
        if (string.IsNullOrEmpty(cultisys_id))
            CW_Core.LogInfo("CW_Actor.__check_level_up__: cultisys_id is null or empty");
        data.get(cultisys_id, out int level, -1);
        if (level == -1) CW_Core.LogInfo($"CW_Actor.__check_level_up__: has not {cultisys_id}");
        CW_Core.LogInfo($"CW_Actor.__check_level_up__: {cultisys_id} level is {level}");
        var cultisys = Manager.cultisys.get(cultisys_id);
        if (cultisys == null) CW_Core.LogInfo("CW_Actor.__check_level_up__: cultisys is null");
        if (cultisys.can_levelup(this, cultisys)) CW_Core.LogInfo("CW_Actor.__check_level_up__: can level up");
        else
        {
            CW_Core.LogInfo(
                $"CW_Actor.__check_level_up__: can not level up: {cultisys.curr_progress(this, cultisys, level)}/{cultisys.max_progress(this, cultisys, level)}");
        }
    }

    private void __set_base_cw_item__(string item_id, EquipmentType slot)
    {
        if (equipment == null)
        {
            CW_Core.LogInfo("CW_Actor.__set_base_cw_item__: equipment is null");
            return;
        }

        CW_ItemAsset item_asset = Manager.items.get(item_id);
        if (item_asset == null)
        {
            CW_Core.LogInfo($"CW_Actor.__set_base_cw_item__: item_asset is null: {item_id}");
            return;
        }

        string main_material = item_asset.MainMaterials.Keys.ToList().GetRandom();
        CW_Core.LogInfo($"CW_Actor.__set_base_cw_item__: main_material: '{main_material}'");
        CW_ItemData item_data = new(item_asset, this, main_material);
        equipment.getSlot(slot).setItem(item_data);
    }
}