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