"""Provides methods for loading/saving metamaps"""
import struct
from enum import IntEnum
import numpy as np

METAMAP_FILE_VERSION = 0x100

class TileType(IntEnum):
    """Tile types in a metamap"""
    UNREACHABLE = 0
    EMPTY = 1
    WALL = 2
    PUSHWALL = 3
    DOOR = 4

class EncodingDim(IntEnum):
    """Dimensions for the one-hot encoding of a metamap"""
    PLAYABLE = 0
    SOLID = 1
    PASSAGE = 2

def load_metamap(filename):
    """Loads a metamap from a file into a numpy array of shape (width, height, 3)"""
    with open(filename, "rb") as fin:
        version = struct.unpack('Q', fin.read(8))[0]

        if version != METAMAP_FILE_VERSION:
            raise ValueError("Unsupported metamap version")

        width = struct.unpack('i', fin.read(4))[0]
        height = struct.unpack('i', fin.read(4))[0]
        size = width * height

        raw_map = np.fromfile(fin, dtype=np.uint8)
        one_hot = np.zeros([size, 3])

        for i in range(size):
            tile_type = TileType(raw_map[i])
            if tile_type == TileType.EMPTY:
                one_hot[i, EncodingDim.PLAYABLE] = 1
            elif tile_type == TileType.UNREACHABLE or tile_type == TileType.WALL:
                one_hot[i, EncodingDim.SOLID] = 1
            elif tile_type == TileType.PUSHWALL or tile_type == TileType.DOOR:
                one_hot[i, EncodingDim.PASSAGE] = 1

        one_hot.shape = (width, height, len(EncodingDim))

        return one_hot

def save_metamap(metamap, filename):
    """Saves a metamap to a file"""
    with open(filename, "wb") as fout:
        fout.write(struct.pack('Q', METAMAP_FILE_VERSION))

        width = metamap.shape[0]
        height = metamap.shape[1]

        fout.write(struct.pack('i', width))
        fout.write(struct.pack('i', height))
        for y in range(height):
            for x in range(width):
                tile_type = TileType.WALL
                if metamap[x, y, EncodingDim.PLAYABLE] == 1:
                    tile_type = TileType.EMPTY
                elif metamap[x, y, EncodingDim.SOLID] == 1:
                    tile_type = TileType.WALL
                elif metamap[x, y, EncodingDim.PASSAGE] == 1:
                    tile_type = TileType.DOOR

                fout.write(struct.pack('b', tile_type))
    return

if __name__ == '__main__':
    # TODO: The map is being transposed
    metamap = load_metamap("test.metamap")
    print("metamap shape: " + str(metamap.shape))
    save_metamap(metamap, "roundtripped.metamap")
