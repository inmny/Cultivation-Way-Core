using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    public class CW_MapChunk_Data
    {
        public float wakan;
        public float wakan_level;
        public int[] element;
    }
    public class CW_MapChunk
    {
        public int x;
        public int y;
        public float wakan;
        public float wakan_level;
        public CW_Element element;
        internal float tmp_wakan;
        internal float total_wakan;
        internal CW_Element tmp_element;
        public CW_MapChunk(int x,int y)
        {
            this.x = x;
            this.y = y;
            this.wakan = Toolbox.randomFloat(0, 10f);
            this.wakan_level = Toolbox.randomFloat(1, 2);
        }
        public void re_gen()
        {
            this.wakan = Toolbox.randomFloat(0, 10f);
            this.wakan_level = Toolbox.randomFloat(1, 2);
            update(true);
        }
        internal void update(bool force = false)
        {
            if (force) tmp_wakan = wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, wakan_level - 1);
            total_wakan = tmp_wakan;
            wakan_level = Mathf.Max(Others.CW_Constants.chunk_wakan_compress_co * Mathf.Log(tmp_wakan, Others.CW_Constants.wakan_level_co), 1);
            wakan = tmp_wakan / Mathf.Pow(Others.CW_Constants.wakan_level_co, wakan_level - 1);
        }
    }
}
