# Tiledriver

Tiledriver is a toolkit for Unified Wolfenstein Mapping Format (UWMF) levels, supported by the [ECWolf](http://maniacsvault.net/ecwolf/) engine.

## Table of Contents

* [License](#licensing)
* [TODO](#todo)
* [Contributors](#contributors)

## Licensing

Tiledriver is licensed with the BSD 3-clause license.  See the file LICENSE for details.

## TODO

### Technical Debt

[ ] The GUI project is broken under .NET 5
[ ] The parsing system is old and ugly; Sector Director has a much nicer one
  [ ] Get rid of T4 templates
  [ ] UWMF
  [ ] XLAT
  [ ] MapInfo
[ ] There is way too much "temporary" code hanging out in `ECWolfLaucher`

### Level Generation

[ ] Move any and all cellular automata stuff out of the launcher project
[X] Create generator for Wang tiles
[ ] Create generator for lighting variants

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