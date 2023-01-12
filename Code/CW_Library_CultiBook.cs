using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Asset_CultiBook : Asset
    {
        public bool is_fixed;
        /// <summary>
        /// 功法名
        /// </summary>
        public string name;
        /// <summary>
        /// 这个暂时用不到，以后可能会用到
        /// </summary>
        public string content;
        /// <summary>
        /// 作者名，作者id即功法的id
        /// </summary>
        public string author_name;
        /// <summary>
        /// 当前修炼人数，用于回收统计
        /// </summary>
        internal int cur_culti_nr;
        /// <summary>
        /// 历史修炼人数
        /// </summary>
        public int histroy_culti_nr;
        /// <summary>
        /// 等阶
        /// </summary>
        public int order;
        /// <summary>
        /// 品级
        /// </summary>
        public int level;
        /// <summary>
        /// 修炼速度加成
        /// </summary>
        public float culti_promt;
        /// <summary>
        /// 自带法术
        /// </summary>
        public string[] spells;
        /// <summary>
        /// 属性加成
        /// </summary>
        public CW_BaseStats bonus_stats;
        public CW_Asset_CultiBook deepcopy()
        {
            CW_Asset_CultiBook ret = new CW_Asset_CultiBook();
            ret.culti_promt = culti_promt;
            ret.cur_culti_nr = cur_culti_nr;
            ret.author_name = author_name;
            ret.bonus_stats = bonus_stats.deepcopy();
            ret.content = content;
            ret.histroy_culti_nr = histroy_culti_nr;
            ret.id = id;
            ret.is_fixed = is_fixed;
            ret.spells = new string[Others.CW_Constants.cultibook_spell_limit];
            for(int i = 0; i < Others.CW_Constants.cultibook_spell_limit; i++)
            {
                ret.spells[i] = spells[i];
            }
            ret.level = level;
            ret.name = name;
            ret.order = order;
            return ret;
        }
        public void store()
        {
            CW_Library_Manager.instance.cultibooks.add(this);
        }
    }
    public class CW_Library_CultiBook : AssetLibrary<CW_Asset_CultiBook>
    {
        public override void init()
        {
            base.init();

        }
        internal void register()
        {
            throw new NotImplementedException();
        }
        public void delete(string cultibook_id, bool force_delete = false)
        {
            CW_Asset_CultiBook book;
            if(this.dict.TryGetValue(cultibook_id, out book))
            {
                if (book.is_fixed && !force_delete) return;
                this.dict.Remove(cultibook_id);
                this.list.Remove(book);
            }
            return;
        }
        internal string select_better(string a, string b)
        {
            if (string.IsNullOrEmpty(a)) return b;
            if (string.IsNullOrEmpty(b)) return a;
            CW_Asset_CultiBook A = get(a);
            CW_Asset_CultiBook B = get(b);
            if (A == null) return B==null?null:b;
            if (B == null) return a;
            return A.level >= B.level ? a : b;
        }
    }
}
