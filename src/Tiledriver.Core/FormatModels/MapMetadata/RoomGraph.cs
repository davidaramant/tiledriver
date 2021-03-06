﻿// Copyright (c) 2018, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tiledriver.Core.FormatModels.MapMetadata
{
    public sealed class RoomGraph : IEnumerable<Room>
    {
        private readonly List<Room> _rooms = new List<Room>();

        public RoomGraph(int width, int height):this(width,height,Enumerable.Empty<Room>()){}
        public RoomGraph(int width, int height,IEnumerable<Room> rooms)
        {
            Width = width;
            Height = height;
            _rooms.AddRange(rooms);  
        } 
        public int Width { get; }
        public int Height { get; }
        public int RoomCount => _rooms.Count;
        public void Add(Room room) => _rooms.Add(room);

        public IEnumerator<Room> GetEnumerator() => _rooms.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}