# Tiledriver

[![.NET Crossplatform](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-crossplatform.yml/badge.svg)](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-crossplatform.yml)
[![.NET Windows](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-windows.yml/badge.svg)](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet-windows.yml)

Tiledriver is a .NET toolkit for Unified Wolfenstein Mapping Format (UWMF) levels, supported by the [ECWolf](http://maniacsvault.net/ecwolf/) engine.

"Toolkit" is deliberatly vague. See the [capabilities](#capabilities) below for more detail about what Tiledriver can actually do.

## Table of Contents

* [License](#licensing)
* [Capcilities](#capabilities)
* [TODO](#todo)
* [Contributors](#contributors)

## Licensing

Tiledriver is licensed with the BSD 3-clause license.  See the file LICENSE for details.

This project basically ports some code from ECWolf in the map translation stuff, so I thought it was safest to just use its license.

## Capabilities

Tiledriver does a bunch of random stuff since the goal of the project has shifted over time. It started as an attempt to procedurally make levels, shifted to a failed attempt at using machine learning to make levels, and is currently back to procedural level generation.

Once upon a time this project had a GUI application written in WPF for viewing UWMF levels. However, this got broken during the migration to .NET 5.

### Format Models

|Format|Reading?|Writing?|
|---|:---:|:---:|
|[UWMF](https://maniacsvault.net/ecwolf/wiki/Universal_Wolfenstein_Map_Format)|Yes|Yes|
|[XLAT](https://maniacsvault.net/ecwolf/wiki/Map_translator)|Yes||
|[MapInfo](https://maniacsvault.net/ecwolf/wiki/MAPINFO)|WIP||
|[Textures](https://maniacsvault.net/ecwolf/wiki/TEXTURES) (TODO)||TODO|
|WAD|Yes|Yes|
|PK3|Yes|Yes|

## TODO

### Technical Debt

- [ ] The GUI project is broken under .NET 5
- [ ] The parsing system is old and ugly; Sector Director has a much nicer one
  - [ ] Get rid of T4 templates
  - [X] UWMF
  - [X] XLAT
  - [ ] MapInfo
- [ ] There is way too much "temporary" code hanging out in `ECWolfLaucher`
- [X] There is a `SteamGameSearcher` class that deals with the Windows Registry. Where is this supposed to live? Can you make a truly cross-platform console app in .NET 5?
- [X] Transition to XUnit/FluentAssertions. Maybe NUnit is causing that weird test failure on Ubuntu

### Level Generation

- [ ] Move any and all cellular automata stuff out of the launcher project
- [X] Create generator for Wang tiles
- [ ] Create generator for lighting variants

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
