namespace Tiledriver.Uwmf
{
    public struct Tag
    {
        private readonly int _id;

        public Tag(int id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return _id.ToString();
        }

        #region Equality stuff

        public bool Equals(Tag other)
        {
            return _id == other._id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Tag && Equals((Tag)obj);
        }

        public override int GetHashCode()
        {
            return _id;
        }

        public static bool operator ==(Tag left, Tag right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Tag left, Tag right)
        {
            return !left.Equals(right);
        }

        #endregion
    }
}
