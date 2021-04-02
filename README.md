# Tiledriver

[![.NET](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet.yml/badge.svg)](https://github.com/davidaramant/tiledriver/actions/workflows/dotnet.yml)

Tiledriver is a .NET toolkit for Unified Wolfenstein Mapping Format (UWMF) levels, supported by the [ECWolf](http://maniacsvault.net/ecwolf/) engine.

Features:

* UWMF viewer written in WPF (currently broken after updating to .NET 5 😒)
* Full support for reading and writing UWMF
* Parse and apply XLAT transforms to turn binary Wolf 3D maps into UWMF
* Parsing support for MapInfo format
* Support for various ECWolf formats like WAD, PK3, etc.

## Table of Contents

* [License](#licensing)
* [TODO](#todo)
* [Contributors](#contributors)

## Licensing

Tiledriver is licensed with the BSD 3-clause license.  See the file LICENSE for details.

## TODO

### Technical Debt

- [ ] The GUI project is broken under .NET 5
- [ ] The parsing system is old and ugly; Sector Director has a much nicer one
  - [ ] Get rid of T4 templates
  - [ ] UWMF
  - [ ] XLAT
  - [ ] MapInfo
- [ ] There is way too much "temporary" code hanging out in `ECWolfLaucher`
- [ ] There is a `SteamGameSearcher` class that deals with the Windows Registry. Where is this supposed to live? Can you make a truly cross-platform console app in .NET 5?

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
