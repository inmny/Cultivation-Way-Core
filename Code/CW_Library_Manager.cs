using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Library
{
    public class CW_Library_Manager
    {
        public static CW_Library_Manager instance;
        public List<BaseAssetLibrary> extended_libraries;
        public Dictionary<string, BaseAssetLibrary> extended_libraries_dict;
        
        public CW_Library_Element elements;

        internal static CW_Library_Manager create()
        {
            instance = new CW_Library_Manager();
            return instance;
        }
        internal void init()
        {
            extended_libraries = new List<BaseAssetLibrary>();
            extended_libraries_dict = new Dictionary<string, BaseAssetLibrary>();
            elements = new CW_Library_Element();
            elements.init();
        }
        /// <summary>
        /// 添加AssetLibrary并进行初始化
        /// </summary>
        /// <param name="id"></param>
        /// <param name="library"></param>
        /// <exception cref="Exception"></exception>
        public void add_library(string id, BaseAssetLibrary library)
        {
            if (extended_libraries_dict.ContainsKey(id)) throw new Exception(String.Format("Repeated Library, id:{0}", id));
            extended_libraries.Add(library);
            extended_libraries_dict.Add(id, library);
            library.init();
        }
    }
}
