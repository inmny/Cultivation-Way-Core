﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public class CW_Spell
    {
        public Library.CW_Asset_Spell asset;
        public float might;
        public float cost;
        public void cast(BaseSimObject pUser, BaseSimObject pTarget, WorldTile pTargetTile)
        {
            // TODO: 细化
            asset.spell_action(this, pUser, pTarget, pTargetTile);
            if(asset.anim_type!=Library.CW_Spell_Animation_Type.CUSTOM) asset.anim_action(this, pUser, pTarget, pTargetTile);
        }
    }
}
