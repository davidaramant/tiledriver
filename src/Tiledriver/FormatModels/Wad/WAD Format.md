# WAD File Format

## Header
A WAD file always starts with a 12-byte header. It contains three values:

### Header Contents
| Position | Length | Name             | Description                                                    |
| -------- | :----: | ---------------- | -------------------------------------------------------------- |
| 0x00     | 4      | `identification` | The ASCII characters "IWAD" or "PWAD".                         |
| 0x04     | 4      | `numlumps`       | An integer specifying the number of lumps in the WAD.          |
| 0x08     | 4      | `infotableofs`   | An integer holding the absolute byte offset of the directory from the start of the file. |

### Notes
All integers are 4 bytes long in x86-style little-endian order. Their values can never exceed 2^31 - 1 (2,147,483,647), since Doom reads them as signed ints.

## Directory
The directory associates names of lumps with the data that belong to them. It consists of a number of entries, each with a length of 16 bytes. The length of the directory is determined by the number given in the WAD header. The structure of each entry is as follows:

### Directory Entry
| Offset | Length | Name      | Content                                                                         |
| ------ | :----: | --------- | ------------------------------------------------------------------------------- |
| 0x00   | 4      | `filepos` | An integer holding the absolute byte offset of the start of the lump's data from the start of the file. |
| 0x04   | 4      | `size`    | An integer representing the size of the lump in bytes.                          |
| 0x08   | 8      | `name`    | An ASCII string defining the lump's name. The name is at most 8 characters long. |

### Notes
Tools should not assume the lump-order in the WAD to be sorted by their byte offset into the WAD.

All lumps with a size of 0 are "virtual" marker lumps that only exist in the directory. Their offset value is irrelevant and should be ignored when reading.

It is possible for more than one lump to have the same offset value, as well as having offsets that overlap other lump data. This is valid aliasing as long as each lump's declared byte range can be read completely within the file.

Any 7-bit ASCII character is allowed in the name. The name may contain up to 8 non-nul characters. If a nul byte appears, the name ends there and any remaining bytes in the field are ignored. Names are case-sensitive. For maximum tool compatibility, names shorter than 8 characters should be terminated with a nul byte and any remaining bytes should also be padded with nul bytes.

Some WAD files pad each lump such that the following lump is aligned on a four-byte boundary. The padding, when present, is always the first byte of the preceding lump repeated either one, two, or three times. Readers should ignore this padding and use the directory offsets and sizes as the source of truth.

## Typical WAD file

The directory is not required to be the last structure in the file. Lump data may appear before or after the directory, and trailing bytes after the directory are allowed.

| Order of contents |
| ----------------- |
| Header            |
| Lump Data         |
| Directory         |
