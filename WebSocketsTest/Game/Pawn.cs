using System;
using System.Collections.Generic;
using System.Linq;

namespace PetitsChevaux.Game
{
    public class Pawn : ICloneable
    {
        public CaseType Type;
        public int Position;



        public static void Move(Pawn pawn, int roll, List<Player> board = null)
        {
            pawn.Move(roll, board);
        }



        public void Move(int roll, List<Player> board = null)
        {
            board = board ?? Board.Players;

            var newPosition = Board.Normalize(this.Position + roll);

            board.ForEach(player =>
            {

                var count = player.Pawns.Count(p => p.Position == this.Position &&
                    p.Type == this.Type);
                //Si un pion d'un autre joueur est sur la case de destination, il est renvoyé au "Box"
                if (count == 1)
                {
                    var removed = player.Pawns.First(p => p.Position == this.Position &&
                        p.Type == this.Type);
                    removed.Position = 0;
                    removed.Type = CaseType.Square;
                }

                //Si 2 pions du même joueur sur la même case, on ne peut atterrir dessus... on annule donc le déplacement
                if (count == 2)
                {
                    newPosition = this.Position;
                }

            });

            this.Position = newPosition;
        }

        #region Implementation of ICloneable

        /// <summary>
        /// Crée un objet qui est une copie de l'instance actuelle.
        /// </summary>
        /// <returns>
        /// Nouvel objet qui est une copie de cette instance.
        /// </returns>
        public object Clone()
        {
            return new Pawn()
            {
                Type = this.Type,
                Position = this.Position
            };
        }
        #endregion

        #region Equality members

        private bool Equals(Pawn other)
        {
            return Position == other.Position && Equals(Type, other.Type);
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
            return Equals((Pawn)obj);
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
                return (Position * 397) ^ (Type != null ? Type.GetHashCode() : 0);
            }
        }

        #endregion
    }
}