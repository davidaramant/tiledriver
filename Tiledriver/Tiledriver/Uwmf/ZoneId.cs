namespace Tiledriver.Uwmf
{
    public struct ZoneId
    {
        private readonly int _id;

        public static readonly ZoneId NotSpecified = new ZoneId(-1);

        public ZoneId(int id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return _id.ToString();
        }

        #region Equality stuff

        public bool Equals(ZoneId other)
        {
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ZoneId && Equals((ZoneId)obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(ZoneId left, ZoneId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ZoneId left, ZoneId right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
