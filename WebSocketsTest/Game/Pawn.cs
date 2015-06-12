using System;
using System.Collections.Generic;
using System.Linq;

namespace PetitsChevaux.Game
{
    public class Pawn : ICloneable
    {

        public readonly int Id;

        public CaseType Type { get; private set; }
        private int _position;


        private Stack<Tuple<int, CaseType>> _old = new Stack<Tuple<int, CaseType>>();

        public CaseType OldType
        {
            get { return _old.Peek().Item2; }
        }

        public int OldPosition
        {
            get { return _old.Peek().Item1; }
        }


        public int Position
        {
            get { return _position; }
            private set { _position = Board.Normalize(value); }
        }

        public static void Move(Pawn pawn, int roll, List<Player> board = null)
        {
            pawn.Move(roll, board);
        }


        public Pawn RollBack(int moves = 1)
        {
            while (moves > 0)
            {
                if (_old.Count > 1)
                {
                    var old = _old.Pop();
                    Position = old.Item1;
                    Type = old.Item2;
                }
                moves--;
            }
            return this;
        }

        public Pawn NoMove()
        {
            _old.Push(new Tuple<int, CaseType>(Position, Type));
            return this;
        }


        public Pawn MoveTo(CaseType type, int position, List<Player> board)
        {
            if (board == null) throw new ArgumentException("Board should not be null", "board");

            if (type == CaseType.Square)
            {
                _old.Push(new Tuple<int, CaseType>(Position, Type));
                Type = type;
                Position = position;
                return this;
            }

            Player owner = board.Find(p => p.Pawns.Contains(this));
            var newPosition = Board.Normalize(position);
            var newType = type;



            if (type == CaseType.Classic)
            {
                board.ForEach(player =>
                {
                    var count = player.Pawns.Count(p => p.Position == newPosition &&
                                                        p.Type == type);
                    //Si un pion d'un autre joueur est sur la case de destination, il est renvoyé au "Box"
                    if (count == 1 && player != owner)
                    {
                        var removed = player.Pawns.First(p => p.Position == newPosition &&
                                                              p.Type == type);
                        removed.Eaten();
                    }

                    //Si 2 pions du même joueur sur la même case, on ne peut atterrir dessus... on annule donc le déplacement
                    if (count == 2)
                    {
                        newType = this.Type;
                        newPosition = this.Position;
                    }

                });
            }


            _old.Push(new Tuple<int, CaseType>(Position, Type));

            this.Position = newPosition;
            this.Type = newType;
            return this;
        }


        public Pawn Move(int roll, List<Player> board)
        {
            if (board == null) throw new ArgumentException("Null Board", "board");

            MoveTo(CaseType.Classic, Board.Normalize(this.Position + roll), board);

            return this;
        }

        private void Eaten()
        {
            Position = 0;
            Type = CaseType.Square;
        }

        public Pawn(int id)
        {
            Id = id;

            _old.Push(new Tuple<int, CaseType>(0, CaseType.Square));
        }

        public Pawn(CaseType type, int position, int id)
            : this(id)
        {
            Type = type;
            Position = position;
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
            if (Type == CaseType.Classic)
            {
                return String.Format(CaseType.Classic.ToString(), Position);
            }
            else if (Type == CaseType.EndGame)
            {

                return String.Format(CaseType.EndGame.ToString(), "{0}", Position);

            }
            else
            {
                return String.Format(CaseType.Square.ToString(), "{0}");
            }
        }

        #endregion

        #region Implementation of ICloneable

        /// <summary>
        /// Crée un objet qui est une copie de l'instance actuelle.
        /// </summary>
        /// <returns>
        /// Nouvel objet qui est une copie de cette instance.
        /// </returns>
        public object Clone()
        {
            var result = new Pawn(this.Id)
            {
                Type = this.Type,
                Position = this.Position,
            };

            result._old.Push(new Tuple<int, CaseType>(this.OldPosition, this.OldType));

            return result;
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