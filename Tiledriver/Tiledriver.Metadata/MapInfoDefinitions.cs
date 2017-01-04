// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class MapInfoDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            // TODO: cluster
            // TODO: episode
            // TODO: gameinfo
            // TODO: intermission

            new BlockData("map").
                HasRequiredString("bordertexture").
                HasRequiredInt("cluster").
                HasRequiredString("completionstring").
                HasRequiredBool("deathcam").
                HasRequiredString("defaultceiling").
                HasRequiredString("defaultfloor").
                HasRequiredStringList("ensureinventory").
                HasRequiredInt("exitfade").
                HasRequiredInt("floornumber").
                HasRequiredString("highscoresgraphic").
                HasRequiredInt("levelbonus").
                HasRequiredInt("levelnum").
                HasRequiredString("music").
                HasRequiredBool("spawnwithweaponraised").
                HasRequiredBool("secretdeathsounds").
                HasRequiredString("next").
                HasRequiredString("secretnext").
                HasRequiredString("victorynext").
                HasRequiredString("nextendsequence").
                HasRequiredString("secretnextendsequence").
                HasRequiredString("victorynextendsequence").
                HasSubBlockLists("specialaction").
                HasRequiredBool("nointermission").
                HasRequiredInt("par").
                HasRequiredString("translator"),

            new BlockData("specialaction").
                HasRequiredString("actorclass").
                HasRequiredString("special").
                HasRequiredString("arg0").
                HasRequiredString("arg1").
                HasRequiredString("arg2").
                HasRequiredString("arg3"),

        };
    }
}
