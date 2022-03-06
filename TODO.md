# TODOs

## Technical Debt

- It would be nice to bring back the GUI project in some form.
  - Investigate if Maui or an alternate cross-platform GUI framework would work
- Clean out the remaining junk in `ECWolfLaucher`
- Importing binary Wolf 3D maps is broken. The translator was not entirely restored after the .NET 5 upgrade/refactor. Bring this back when I have a reason to need it.

## Long Term

- Doom caves using tile-based cellular automata
- Doom caves using hex-based cellular automata
- Doom geometry (not necessarily caves) using Voronoi diagrams and/or noise
- Wolf 3D related:
  - Support Spear of Destiny
  - Find GOG install of Wolf/Spear

## Short Term

### Wolf

- [x] The writer for `TEXTURES` spits out unnecessary empty paren blocks
- [x] The floor/ceiling lighting distinction is a hack (still a hack, but slightly less crude)
- [ ] Refactor light mapper to spit out one or two light maps
- [x] Should `LightMap` really be an array internally? It may be more convenient to use a set
- [x] Need sprites for glowing crystals
- [ ] Port over stalagmites/stalactites?
- [ ] Place treasure
- [ ] Enemies?
  - This would need weapons & ammo too
  - It would be a shame not to do patrolling enemies. Maybe use the interior distances?
- [ ] Connect disjoint caves as secrets?

### Doom

- [x] Infrastructure for generating a cave map
- [ ] Option to flip visualizations across Y-axis
- [x] Place 1-sided walls using marching squares
- [x] Don't generate so many redundant linedefs
- [x] Walk graph to set correct texture offsets
- [ ] Textures for use in Doom
- [x] Create detailed cave outline
- [x] Create interior of cave
- [ ] Cave lighting
- [ ] Alternate material
- [ ] Tweak how height level is used (sine wave?)
- [x] Place 2-sided walls for interior regions
- [ ] Utility method for placing Thing at location (check that it fits)
- [ ] Doom Things helper list (like the Wolf Actors)
  - [x] Actor list
  - [ ] Actor categories
  - [ ] Textures & Flats

### General

- [ ] Should CellBoard and ConnectedArea be combined????
- [x] Making the detailed cave is super slow; profile what the issue is
