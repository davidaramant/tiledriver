# Tiledriver

[![.NET Windows](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-windows.yml/badge.svg)](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-windows.yml)
[![.NET macOS](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-macos.yml/badge.svg)](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-macos.yml)

Tiledriver is a .NET toolkit for [Unified Wolfenstein Mapping Format (UWMF)](https://maniacsvault.net/ecwolf/wiki/Universal_Wolfenstein_Map_Format) and [Unified Doom Mapping Format (UDMF)](https://doomwiki.org/wiki/UDMF) levels, supported by [ECWolf](http://maniacsvault.net/ecwolf/) and [ZDoom](https://zdoom.org).

"Toolkit" is deliberately vague. See the [capabilities](#capabilities) below for more detail about what Tiledriver can actually do.

Tiledriver supports Windows and macOS. There is nothing intentionally broken about Linux, but I think it needs more Skia packages or something. Support for that platform is not a priority for me so the issue is ignored for now.

## Table of Contents

* [License](#licensing)
* [Capabilities](#capabilities)
* [TODO](TODO.md)
* [Contributors](#contributors)

## Licensing

Tiledriver is licensed with the BSD 3-clause license.  See the file LICENSE for details.

This project ports some code from ECWolf in the map translation stuff, so I thought it was safest to just use its license.

## Capabilities

Tiledriver does a bunch of random stuff since the goal of the project has shifted over time. It started as an attempt to procedurally make Wolfenstein levels, shifted to a failed attempt at using machine learning to make levels, and is currently back to procedural level generation.

Once upon a time this project had a GUI application written in WPF for viewing UWMF levels. However, this got broken during the migration to .NET 5. A GUI may come back as a Maui app.

A recent (as of writing) change is a sort-of merge with the "sister" project [Sector Director](https://github.com/davidaramant/sector-director). Sector Director was similar but targeted Doom instead of Wolf 3D. Tiledriver will focus more on level generation, while Sector Director will be mostly about rendering (I think...)

### Format Models

|Format|Reading?|Writing?|
|---|:---:|:---:|
|[`UWMF`](https://maniacsvault.net/ecwolf/wiki/Universal_Wolfenstein_Map_Format)|Yes|Yes|
|[`UDMF`](https://doomwiki.org/wiki/UDMF)|Yes|Yes|
|[`XLAT`](https://maniacsvault.net/ecwolf/wiki/Map_translator)|Yes|No|
|[`MAPINFO`](https://maniacsvault.net/ecwolf/wiki/MAPINFO) (ECWolf flavor)|Partial|Yes|
|[`TEXTURES`](https://maniacsvault.net/ecwolf/wiki/TEXTURES)|No|Yes|
|WAD|Yes|Yes|
|PK3|Yes|Yes|

Format Notes:

* I cannot think of a reason to support writing `XLAT`
* Only the portions of `MAPINFO` that relate to converting binary maps are parsed.
* I cannot think of a reason to support reading `TEXTURES`

## Contributors

The following people have helped out over the years:

* David Aramant
* Kyle Pinches
* Jeff Loveless
* Ryan Clarke
* Jason Giles
* Aaron Alexander
* Leon Organ
* Luke Gilbert
* Jeremy Jarvis
* Grant Kimsey
