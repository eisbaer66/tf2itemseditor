namespace TF2Items.Core
{
    public class WeaponIdentifier
    {
        public int? Id { get; set; }

        private WeaponIdentifier()
        {

        }

        public override string ToString()
        {
            if (Id == null)
                return "*";

            return Id.ToString();
        }

        protected bool Equals(WeaponIdentifier other)
        {
            if (Id == null)
            {
                if (other.Id == null)
                    return true;
                return false;
            }
            if (other.Id == null)
                return false;

            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((WeaponIdentifier)obj);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static WeaponIdentifier Any()
        {
            return new WeaponIdentifier();
        }

        public static WeaponIdentifier FromId(int id)
        {
            return new WeaponIdentifier
                   {
                       Id = id,
                   };
        }
    }
}