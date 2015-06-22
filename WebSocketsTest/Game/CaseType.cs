using System;

namespace PetitsChevaux.Game
{
    public class CaseType
    {
        public static readonly CaseType Classic = new CaseType("c-{0}");
        public static readonly CaseType EndGame = new CaseType("e-{0}-{1}");
        public static readonly CaseType Square = new CaseType("sq-{0}");

        private readonly String _value;

        private CaseType(String value)
        {
            _value = value;
        }





        #region Equality members

        private bool Equals(CaseType other)
        {
            return string.Equals(_value, other._value);
        }

        public static bool operator ==(CaseType c1, CaseType c2)
        {
            if (ReferenceEquals(c1, null)) return ReferenceEquals(c2, null);
            return !ReferenceEquals(c2, null) && c1.Equals(c2);
        }

        public static bool operator !=(CaseType c1, CaseType c2)
        {
            return !(c1 == c2);
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
            return Equals((CaseType)obj);
        }


        /// <summary>
        /// Sert de fonction de hachage pour un type particulier.
        /// </summary>
        /// <returns>
        /// Code de hachage du <see cref="T:System.Object"/> actuel.
        /// </returns>
        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }

        #endregion

        #region Overrides of Object

        /// <summary>
        /// Retourne une chaîne qui représente l'objet actuel.
        /// </summary>
        /// <returns>
        /// Chaîne qui représente l'objet en cours.
        /// </returns>
        public override string ToString()
        {
            return _value;
        }

        #endregion
    }
}