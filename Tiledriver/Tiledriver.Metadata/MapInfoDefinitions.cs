// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Metadata
{
    public static class MapInfoDefinitions
    {
        public static readonly IEnumerable<BlockData> Blocks = new[]
        {
            //new BlockData("cluster").
            //    HasRequiredInt("id").
            //    HasRequiredString("exittext").
            //    HasRequiredString("exittextlookup").
            //    HasOptionalBool("exittextislump",false).
            //    HasOptionalBool("exittextismessage",false), 

            //// clearepisodes
            //new BlockData("episode").
            //    HasRequiredString("map").
            //    HasRequiredChar("key").
            //    HasRequiredString("lookup").
            //    HasRequiredString("name").
            //    HasOptionalBool("noskillmenu",false).
            //    HasOptionalBool("optional",false).
            //    HasRequiredString("picname").
            //    HasOptionalBool("remove",false), 

            // TODO: gameinfo
            // TODO: intermission

            // TODO: defaultmap
            // TODO: adddefaultmap
            // TODO: map names
            //new BlockData("map").
            //    HasRequiredString("maplump").
            //    HasRequiredString("mapname").
            //    HasRequiredString("mapnamelookup").
            //    HasRequiredString("bordertexture").
            //    HasRequiredInt("cluster").
            //    HasRequiredString("completionstring").
            //    HasOptionalBool("deathcam",false).
            //    HasRequiredString("defaultceiling").
            //    HasRequiredString("defaultfloor").
            //    HasRequiredStringList("ensureinventory").
            //    HasRequiredInt("exitfade").
            //    HasRequiredInt("floornumber").
            //    HasRequiredString("highscoresgraphic").
            //    HasRequiredInt("levelbonus").
            //    HasRequiredInt("levelnum").
            //    HasRequiredString("music").
            //    HasOptionalBool("spawnwithweaponraised",false).
            //    HasOptionalBool("secretdeathsounds",false).
            //    HasRequiredString("next").
            //    HasRequiredString("secretnext").
            //    HasRequiredString("victorynext").
            //    HasRequiredString("nextendsequence").
            //    HasRequiredString("secretnextendsequence").
            //    HasRequiredString("victorynextendsequence").
            //    HasSubBlockLists("specialaction").
            //    HasOptionalBool("nointermission",false).
            //    HasRequiredInt("par").
            //    HasRequiredString("translator"),

            new BlockData("specialaction", 
                properties:new []
                {
                    new PropertyData("actorclass",PropertyType.String),
                    new PropertyData("special",PropertyType.String),
                    new PropertyData("arg0",PropertyType.String),
                    new PropertyData("arg1",PropertyType.String),
                    new PropertyData("arg2",PropertyType.String),
                    new PropertyData("arg3",PropertyType.String),
                }),
        };
    }
}
