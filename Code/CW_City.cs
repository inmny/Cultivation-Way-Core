using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
namespace Cultivation_Way
{
    public class CW_City
    {
        #region Getter
        public static Func<City, CityData> get_data = CW_ReflectionHelper.create_getter<City, CityData>("data");
        public static Func<City, Kingdom> get_kingdom = CW_ReflectionHelper.create_getter<City, Kingdom>("kingdom");
        #endregion
        #region Setter
        internal static Action<City, bool> set_created = CW_ReflectionHelper.create_setter<City, bool>("created");
        public static Action<City, CityData> set_data = CW_ReflectionHelper.create_setter<City, CityData>("data");
        #endregion
        #region Func
        public static Action<City> func_createAI = (Action<City>)CW_ReflectionHelper.get_method<City>("createAI");
        public static Func<City, string, ResourceAsset> func_getFoodItem = (Func<City, string, ResourceAsset>)CW_ReflectionHelper.get_method<City>("getFoodItem");
        #endregion
    }
}
