"""Provides methods for loading/saving metamaps"""
import struct
from enum import IntEnum
import numpy as np

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
    DOOR = 2

def load_metamap(filename):
    """Loads a metamap from a file into a numpy array of shape (width, height, 3)"""
    with open(filename, "rb") as fin:
        version = struct.unpack('Q', fin.read(8))[0]

        if version != 0x100:
            raise ValueError("Unsupported metamap version")

        width = struct.unpack("i", fin.read(4))[0]
        height = struct.unpack("i", fin.read(4))[0]
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
                one_hot[i, EncodingDim.DOOR] = 1

        one_hot.shape = (width, height, len(EncodingDim))

        return one_hot

def save_metamap(metamap, filename):
    """Saves a metamap to a file"""
    return

if __name__ == '__main__':
    metamap = load_metamap("test.metamap")
    print("metamap shape: " + str(metamap.shape))