using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way
{
    public class CW_MapChunk_Manager
    {
        public int width;
        public int height;
        public float total_wakan;
        public CW_MapChunk[,] chunks;
        public bool zone_dirty = false;
        private Library.CW_Asset_WorldEvent wakan_tide;
        internal void init(int x, int y)
        {
            width = x; height = y;
            chunks = new CW_MapChunk[x, y];
            for(int i = 0; i < x; i++)
            {
                for(int j = 0; j < y; j++)
                {
                    chunks[i, j] = new CW_MapChunk(i,j);
                }
            }
            wakan_tide = Library.CW_Library_Manager.instance.events.get("wakan_tide");
            wakan_tide.action(wakan_tide);
        }
        internal void reset(int x, int y)
        {
            if (x != width || y != height)
            {
                width = x; height = y;
                chunks = new CW_MapChunk[x, y];
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        chunks[i, j] = new CW_MapChunk(i,j);
                    }
                }
            }
            else
            {
                for (int i = 0; i < x; i++)
                {
                    for (int j = 0; j < y; j++)
                    {
                        chunks[i, j].re_gen();
                    }
                }
            }
            //wakan_tide.action(wakan_tide);
        }
        private float __comp_delta_wakan(float wakan_1, float wakan_2)
        {
            if (Content.W_Content_WorldLaws.is_no_wakan_spread_working()) return 0;
            return Mathf.Sign(wakan_2-wakan_1) * Mathf.Sqrt(Mathf.Abs(wakan_2 - wakan_1)) * Others.CW_Constants.chunk_wakan_spread_grad;
        }
        internal void update()
        {
            if(this.width!=Config.ZONE_AMOUNT_X*Config.CITY_ZONE_SIZE || this.height != Config.ZONE_AMOUNT_Y * Config.CITY_ZONE_SIZE)
            {
                this.reset(Config.ZONE_AMOUNT_X * Config.CITY_ZONE_SIZE, Config.ZONE_AMOUNT_Y * Config.CITY_ZONE_SIZE);
            }
            int i; int j;
            // 首先更新各个区块的灵气裂解
            /**
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {   
                    chunks[i, j].lyse();
                }
            }
            */
            // 可采用原地算法，但在区块灵气更新作为单独线程时会造成麻烦，故而，申请额外固定空间
            zone_dirty = true;
            //if (Utils.CW_Utils_Others.is_map_mode_active("map_wakan_zones")) Content.W_Content_Helper.zone_calculator.CallMethod("setDrawnZonesDirty");
            total_wakan = 0;
            CW_MapChunk chunk_1 = null, chunk_2 = null;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    chunk_1 = chunks[i, j];
                    
                    chunk_1.total_wakan = chunk_1.wakan * Mathf.Pow(Others.CW_Constants.wakan_level_co, chunk_1.wakan_level - 1);
                    chunk_1.tmp_wakan = chunk_1.total_wakan;
                    total_wakan += chunk_1.total_wakan;
                }
            }
            float delta_wakan = 0;
            // 双方向更新，最后同步数据
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height - 1; j++)
                {
                    chunk_1 = chunks[i, j]; chunk_2 = chunks[i, j + 1];
                    delta_wakan = __comp_delta_wakan(chunk_1.total_wakan, chunk_2.total_wakan);
                    chunk_2.tmp_wakan -= delta_wakan;
                    chunk_1.tmp_wakan += delta_wakan;
                }
            }
            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width - 1; i++)
                {
                    chunk_1 = chunks[i, j]; chunk_2 = chunks[i + 1, j];
                    delta_wakan = __comp_delta_wakan(chunk_1.total_wakan, chunk_2.total_wakan);
                    chunk_2.tmp_wakan -= delta_wakan;
                    chunk_1.tmp_wakan += delta_wakan;
                }
            }
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    chunks[i, j].update(false);
                }
            }

            if (this.total_wakan / (this.width * this.height) < wakan_tide.trigger_val)
            {
                wakan_tide.action(wakan_tide);
            }
        }
        public List<CW_MapChunk_Data> save()
        {
            int i, j;
            List<CW_MapChunk_Data> ret = new List<CW_MapChunk_Data>();
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    ret.Add(new CW_MapChunk_Data
                    {
                        wakan = chunks[i, j].wakan,
                        wakan_level = chunks[i,j].wakan_level,
                        element = chunks[i,j].element == null ? new int[] { 20,20,20,20,20}: chunks[i, j].element.base_elements
                    });
                }
            }
            return ret;
        }
        public void load(List<CW_MapChunk_Data> datas)
        {
            if (datas.Count != this.width * this.height) throw new Exception();
            int i, j;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    chunks[i, j].wakan = datas[i * height + j].wakan;
                    chunks[i, j].wakan_level = datas[i * height + j].wakan_level;
                    chunks[i, j].element = new CW_Element(datas[i * height + j].element);
                }
            }
        }
        public CW_MapChunk get_chunk(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height) return null;
            return chunks[x, y];
        }
        private void print_rect(int x, int y, int width, int height)
        {
            MonoBehaviour.print(string.Format("***************({0},{1})*******************************({2},{3})****************", x, y, x + width, y + height));
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine();
            for(int i= y; i< y+height; i++)
            {
                for(int j= x; j< x+width; j++)
                {
                    stringBuilder.Append((int)chunks[i, j].wakan+"\t\t");
                }
                stringBuilder.AppendLine();
            }
            MonoBehaviour.print(stringBuilder.ToString());
        }
    }
}
