using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way.Content
{
    internal static class W_Content_SpectateUnit
    {
        private static StatBar wakan_bar;
        private static StatBar shield_bar;
        internal static void init()
        {
            GameObject spectate_unit_object = NCMS.Utils.GameObjects.FindEvenInactive("SpectateUnit");
            spectate_unit_object.SetActive(false);

            RectTransform rect_transform = spectate_unit_object.transform.GetChild(0).GetComponent<RectTransform>();
            rect_transform.offsetMin = new Vector2(rect_transform.offsetMin.x, -25.5f);
            rect_transform.offsetMax = new Vector2(rect_transform.offsetMax.x, 25.5f);

            rect_transform = spectate_unit_object.transform.GetChild(1).GetComponent<RectTransform>();
            rect_transform.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/cw_window/sim_info_bg");
            rect_transform.transform.localPosition = new Vector3(-6.80f, 15.38f);
            rect_transform.transform.localScale = new Vector3(1.10f, 1.60f);

            for(int i = 2; i < 7; i++)
            {
                Transform __transform_to_reset_position = spectate_unit_object.transform.GetChild(i);
                __transform_to_reset_position.localPosition = new Vector3(__transform_to_reset_position.localPosition.x, __transform_to_reset_position.localPosition.y + 7);
            }

            Transform icons = spectate_unit_object.transform.Find("Icons");

            GameObject WakanBar = GameObject.Instantiate(icons.Find("HealthBar").gameObject, icons);
            WakanBar.name = "WakanBar";
            WakanBar.transform.Find("Mask/Bar").GetComponent<Image>().color = new Color(0.38f, 0.71f, 1, 0.75f);
            rect_transform = WakanBar.transform.Find("Mask/Bar").GetComponent<RectTransform>();
            //rect_transform.offsetMin = new Vector2(0.1f, rect_transform.offsetMin.y);
            WakanBar.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconWakan");
            WakanBar.transform.Find("Icon").GetComponent<Image>().transform.localScale = new Vector3(0.35f, 0.35f);

            WakanBar.transform.localPosition = new Vector3(-47, -14);
            //rect_transform = WakanBar.transform.GetComponent<RectTransform>();
            //rect_transform.offsetMax = new Vector2(-5, rect_transform.offsetMax.y);
            wakan_bar = WakanBar.GetComponent<StatBar>();

            GameObject ShieldBar = GameObject.Instantiate(icons.Find("HealthBar").gameObject, icons);
            ShieldBar.name = "ShieldBar";
            ShieldBar.transform.Find("Mask/Bar").GetComponent<Image>().color = new Color(1, 1, 1, 0.79f);
            ShieldBar.transform.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/Icons/iconShield");
            //rect_transform = ShieldBar.transform.GetComponent<RectTransform>();
            //rect_transform.sizeDelta = new Vector2(85.5f, rect_transform.sizeDelta.y);
            ShieldBar.transform.localPosition = new Vector3(48, -14);
            shield_bar = ShieldBar.GetComponent<StatBar>();

            spectate_unit_object.transform.localPosition = new Vector3(0, 30);
            spectate_unit_object.SetActive(true);
        }
        internal static void updateStats(SpectateUnit spectate_unit)
        {
            CW_Actor cw_actor = (CW_Actor)MoveCamera.focusUnit;
            wakan_bar.setBar((int)cw_actor.cw_status.wakan, cw_actor.cw_cur_stats.wakan, "/" + cw_actor.cw_cur_stats.wakan);
            wakan_bar.transform.Find("Mask/Bar").GetComponent<Image>().color = Utils.CW_Utils_Others.get_wakan_color(cw_actor.cw_status.wakan_level, cw_actor.cw_cur_stats.wakan);
            shield_bar.setBar((int)cw_actor.cw_status.shield, cw_actor.cw_cur_stats.shield, "/" + cw_actor.cw_cur_stats.shield);
        }
    }
}
