using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans
{
    public static class SimpleMinded
    {

        public static void NextMove(Player player)
        {
            int roll = Board.RollDice();
            //Si on peut faire avancer un pion sur les cases de fin
            if (player.Pawns.Any(p => p.Type == CaseType.EndGame && (roll - p.Position) == 1) &&
                !player.Pawns.Any(t => t.Type == CaseType.EndGame && t.Position == roll))
            {
                var pawn = player.Pawns.First(p => p.Type == CaseType.EndGame && (roll - p.Position) == 1);
                pawn.Position = roll;
            }
            //Entrée dans les cases de fin de jeu
            else if (player.Pawns.Any(p => p.Type == CaseType.Classic &&
                p.Position == Board.Normalize(player.StartCase - 1)) && roll == 1)
            {
                var pawn = player.Pawns.First(
                    p => p.Type == CaseType.Classic && p.Position == Board.Normalize(player.StartCase - 1));

                pawn.Position = 1;
                pawn.Type = CaseType.EndGame;
            }
            //Si le roll est 6 on sort un pion si aucun n'est sur la case de départ
            else if (roll == 6 &&
                !player.Pawns.Any(p => p.Type == CaseType.Classic && p.Position == player.StartCase) &&
                player.Pawns.Any(p => p.Type == CaseType.Square))
            {
                var pawn = player.Pawns.First(p => p.Type == CaseType.Square);
                pawn.Position = player.StartCase;
                pawn.Type = CaseType.Classic;
            }
            //Si pas 6 ou que la case de départ est occupée, on avance le premier pion disponible
            else if (!player.Pawns.All(p => p.Type == CaseType.Square || p.Type == CaseType.EndGame))
            {
                player.Pawns.First().Move(roll);
            }
            //Si aucun des chemins ci-dessus n'a pu être utilisé, on passe son tour
        }

    }
}