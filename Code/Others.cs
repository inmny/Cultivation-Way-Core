using Cultivation_Way.Animation;
namespace Cultivation_Way.Others
{
    public class CW_Constants
    {
        public const int save_version = 1;
        public const int base_element_types = 5;
        
    }
    public class CW_Delegates
    {
        public delegate void CW_Animation_Trace_Update(float src_x, float src_y, float dst_x, float dst_y, float play_time, ref float anim_x, ref float anim_y);
    }
}