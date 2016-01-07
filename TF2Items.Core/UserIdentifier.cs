namespace TF2Items.Core
{
    public class UserIdentifier
    {
        public string[] SteamIds { get; set; }

        private UserIdentifier()
        {
            
        }

        public override string ToString()
        {
            if (SteamIds == null)
                return "*";

            return string.Join(" ; ", SteamIds);
        }

        protected bool Equals(UserIdentifier other)
        {
            if (SteamIds == null)
            {
                if (other.SteamIds == null)
                    return true;
                return false;
            }
            if (other.SteamIds == null)
                return false;

            return SteamIds.EquivalentTo(other.SteamIds);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((UserIdentifier) obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static UserIdentifier Any()
        {
            return new UserIdentifier();
        }

        public static UserIdentifier FromStreamIds(params string[] streamIds)
        {
            bool listIsEmpty = streamIds.Length == 0;
            bool onlyContainsStar = streamIds.Length == 1 && (string.IsNullOrEmpty(streamIds[0]) || streamIds[0] == "*");
            if (listIsEmpty || onlyContainsStar)
                return Any();

            return new UserIdentifier
                   {
                       SteamIds = streamIds,
                   };
        }
    }
}