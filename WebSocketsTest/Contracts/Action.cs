using PetitsChevaux.Game;

namespace PetitsChevaux.Contracts
{
    public class Action
    {
        public Pawn Subject;
        public int Position;
        public CaseType Type;


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="T:System.Object"/>.
        /// </summary>
        public Action(Pawn subject, int position, CaseType type)
        {
            Subject = subject;
            Position = position;
            Type = type;
        }

        #region Equality members

        private bool Equals(Action other)
        {
            return Equals(Subject.Id, other.Subject.Id) && Position == other.Position && Equals(Type, other.Type);
        }

        /// <summary>
        /// Détermine si l'objet <see cref="T:System.Object"/> spécifié est égal à l'objet <see cref="T:System.Object"/> actuel.
        /// </summary>
        /// <returns>
        /// true si l'objet spécifié est égal à l'objet actuel ; sinon, false.
        /// </returns>
        /// <param name="obj">Objet à comparer avec l'objet actif.</param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Action)obj);
        }

        /// <summary>
        /// Sert de fonction de hachage pour un type particulier.
        /// </summary>
        /// <returns>
        /// Code de hachage du <see cref="T:System.Object"/> actuel.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Subject != null ? Subject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position;
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                return hashCode;
            }
        }

        #endregion
    }
}