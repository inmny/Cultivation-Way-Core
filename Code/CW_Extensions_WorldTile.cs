using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Extensions
{
    public static class CW_Extensions_WorldTile
    {
        public static CW_MapChunk get_cw_chunk(this WorldTile tile)
        {
            return World_Data.instance.map_chunk_manager.chunks[tile.chunk.x, tile.chunk.y];
        }
    }
}
