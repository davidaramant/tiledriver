using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tiledriver.Generator.SimpleGeometry
{
    public sealed class GeometryStack
    {
        public static GeometryStack Empty = new GeometryStack();

        private readonly GeometrySlice _slice;
        private readonly GeometryStack _tail;

        public Rectangle LastRoom => _slice.Room;

        private GeometryStack()
        {          
        }

        private GeometryStack(GeometrySlice slice, GeometryStack tail)
        {
            _slice = slice;
            _tail = tail;
        }

        public GeometryStack Push(GeometrySlice slice)
        {
            return new GeometryStack(slice,_tail);
        }

        public GeometryStack Pop()
        {
            return _tail;
        }

        public IEnumerable<Rectangle> GetAllRooms()
        {
            var stackCell = this;
            while (stackCell != Empty)
            {
                yield return stackCell._slice.Room;
                stackCell = stackCell._tail;
            }
        }

        public IEnumerable<Rectangle> GetAllHallways()
        {
            var stackCell = this;
            while (stackCell != Empty)
            {
                var hallway = stackCell._slice.Hallway;
                if (hallway.HasValue)
                {
                    yield return hallway.Value;
                }
                stackCell = stackCell._tail;
            }
        }

        public bool DoesIntersect(Rectangle rectangle)
        {
            return
                GetAllRooms().
                Concat(GetAllHallways()).
                Any(room => room.IntersectsWith(rectangle));
        }
    }
}
