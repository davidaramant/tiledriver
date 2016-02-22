namespace Tiledriver.Uwmf
{
    public struct TileId
    {
        private readonly int _id;

        public TileId(int id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return _id.ToString();
        }

        #region Equality stuff

        public bool Equals(TileId other)
        {
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TileId && Equals((TileId)obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(TileId left, TileId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TileId left, TileId right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
