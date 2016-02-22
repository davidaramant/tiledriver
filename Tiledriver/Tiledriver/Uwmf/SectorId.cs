namespace Tiledriver.Uwmf
{
    public struct SectorId
    {
        private readonly int _id;

        public static readonly SectorId NotSpecified = new SectorId(-1);

        public SectorId(int id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return _id.ToString();
        }

        #region Equality stuff

        public bool Equals(SectorId other)
        {
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SectorId && Equals((SectorId)obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(SectorId left, SectorId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SectorId left, SectorId right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
