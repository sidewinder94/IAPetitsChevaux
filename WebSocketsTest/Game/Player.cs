using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;

namespace PetitsChevaux.Game
{
    public class Player : ICloneable
    {
        private static int _nextId = 0;

        public readonly int Id;

        public static void Reset()
        {
            _nextId = 0;
        }

        public Boolean Won
        {
            get
            {
                var temp = Pawns.Any(p => p.Type == CaseType.EndGame && p.Position == 6);
                return temp;
            }
        }

        public readonly List<Pawn> Pawns = new List<Pawn>();

        public Func<Player, List<Player>, int> NextMove { private get; set; }

        public int StartCase
        {
            get { return Id * 14; }
        }

        private Player(int id)
        {
            this.Id = id;
        }

        public Player()
        {
            this.Id = _nextId++;
            if (Id > 3) throw new InvalidOperationException("There can only be 4 players at most in a game");
            for (var i = 0; i < Board.PawnsPerPlayer; i++)
            {
                Pawns.Add(new Pawn(CaseType.Square, 0));
            }
        }

        public int Play(List<Player> board)
        {
            if (NextMove == null) throw new ArgumentException("No Method to determine " + this + " next move");
            return NextMove(this, board);
        }

        public int Evaluate
        {
            get
            {
                int result = 0;

                Pawns.ForEach(p =>
                {
                    if (p.OldType == CaseType.Square && p.Type == CaseType.Classic && p.Position == 0) result +=
                     (int)Math.Pow(56, 3);

                    //Si sur les cases de fin ajouter 56 à la position, puisqu'on reprends de 1 et qu'on considère ce mouvement comme plus important
                    if (p.Type == CaseType.EndGame) result += (int)Math.Pow(p.Position * 10 + 56, 2);
                    if (p.Type == CaseType.Classic) result += (int)Math.Pow(Board.Normalize(p.Position - StartCase), 2);
                });

                if (Won) result = int.MaxValue;

                return result;
            }
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
            return String.Format("Player {0}", Id + 1);
        }

        /// <summary>
        /// Crée un objet qui est une copie de l'instance actuelle.
        /// </summary>
        /// <returns>
        /// Nouvel objet qui est une copie de cette instance.
        /// </returns>
        public object Clone()
        {
            var result = new Player(this.Id);
            this.Pawns.ForEach(p => result.Pawns.Add((Pawn)p.Clone()));
            return result;

        }

        #endregion

    }
}