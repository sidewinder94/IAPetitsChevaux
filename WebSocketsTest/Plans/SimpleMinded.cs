using System.Collections.Generic;
using System.Linq;
using PetitsChevaux.Contracts;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans
{
    public static class SimpleMinded
    {

        public static Contracts.Action NextMove(Player player, List<Player> board, int roll)
        {

            Contracts.Action result = null;

            //return roll;
            //Si on peut faire avancer un pion sur les cases de fin
            if (player.Pawns.Any(p => p.Type == CaseType.EndGame && (roll - p.Position) == 1) &&
                !player.Pawns.Any(t => t.Type == CaseType.EndGame && t.Position == roll))
            {
                var pawn = player.Pawns.First(p => p.Type == CaseType.EndGame && (roll - p.Position) == 1);
                result = new Action(pawn, roll, CaseType.EndGame);
                pawn.MoveTo(CaseType.EndGame, roll, board);
            }
            //Entrée dans les cases de fin de jeu
            else if (player.Pawns.Any(p => p.Type == CaseType.Classic &&
                p.Position == Board.Normalize(player.StartCase - 1)) && roll == 1)
            {
                var pawn = player.Pawns.First(
                    p => p.Type == CaseType.Classic && p.Position == Board.Normalize(player.StartCase - 1));

                result = new Action(pawn, 1, CaseType.EndGame);
                pawn.MoveTo(CaseType.EndGame, 1, board);
            }
            //Si le roll est 6 on sort un pion si aucun n'est sur la case de départ
            else if (roll == 6 &&
                !player.Pawns.Any(p => p.Type == CaseType.Classic && p.Position == player.StartCase) &&
                player.Pawns.Any(p => p.Type == CaseType.Square))
            {
                var pawn = player.Pawns.First(p => p.Type == CaseType.Square);
                result = new Action(pawn, player.StartCase, CaseType.Classic);
                pawn.MoveTo(CaseType.Classic, player.StartCase, board);

            }
            //Si pas 6 ou que la case de départ est occupée, on avance le premier pion disponible qui n'attends pas pour la fin de jeu
            else if (player.Pawns.Any(p => (p.Type == CaseType.Classic) && (p.Position != (Board.Normalize(player.StartCase - 1)))))
            {
                var pawn = player.Pawns.First(p => (p.Type == CaseType.Classic) && (p.Position != (Board.Normalize(player.StartCase - 1))));
                result = new Action(pawn, Board.Normalize(pawn.Position + roll), CaseType.Classic);
            }
            //Dans le cas ou on ne peut rien bouger d'autre que le pion attendant pour rentrer
            else if (!player.Pawns.All(p => p.Type == CaseType.Square || p.Type == CaseType.EndGame))
            {
                var pawn = player.Pawns.First(p => p.Type == CaseType.Classic);
                result = new Action(pawn, Board.Normalize(pawn.Position + roll), CaseType.Classic);
            }
            //Si aucun des chemins ci-dessus n'a pu être utilisé, on passe son tour
            return result;
        }

    }
}