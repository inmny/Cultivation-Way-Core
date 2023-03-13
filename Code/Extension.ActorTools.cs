﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cultivation_Way.Utils;
namespace Cultivation_Way.Extension
{
    public static class ActorTools
    {
        private static Action<Actor> __newCreature = (Action<Actor>)ReflectionHelper.get_method<Actor>("newCreature");
        internal static void newCreature(this Actor actor)
        {
            __newCreature(actor);
        }
    }
}
