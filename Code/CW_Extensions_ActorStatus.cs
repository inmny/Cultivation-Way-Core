using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Extensions
{
    public static class CW_Extensions_ActorStatus
    {
        public static ActorStatus deepcopy(this ActorStatus status)
        {
            ActorStatus copy = new ActorStatus();
            copy.actorID = status.actorID;
            copy.age = status.age;
            copy.alive = status.alive;
            copy.bornTime = status.bornTime;
            copy.children = status.children;
            copy.culture = status.culture;
            copy.diplomacy = status.diplomacy;
            copy.experience = status.experience;
            copy.favorite = status.favorite;
            copy.favoriteFood = status.favoriteFood;
            copy.firstName = status.firstName;
            copy.gender = status.gender;
            copy.health = status.health;
            copy.homeBuildingID = status.homeBuildingID;
            copy.hunger = status.hunger;
            copy.intelligence = status.intelligence;
            copy.kills = status.kills;
            copy.level = status.level;
            copy.mood = status.mood;
            copy.profession = status.profession;
            copy.skin = status.skin;
            copy.skin_set = status.skin_set;
            copy.special_graphics = status.special_graphics;
            copy.statsID = status.statsID;
            copy.stewardship = status.stewardship;
            copy.traits = new List<string>(); 
            copy.traits.AddRange(status.traits);
            copy.transportID = status.transportID;
            copy.warfare = status.warfare;
            return copy;
        }
    }
}
