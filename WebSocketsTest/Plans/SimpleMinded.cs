using System.Linq;
using PetitsChevaux.Game;

namespace PetitsChevaux.Plans
{
    public static class SimpleMinded
    {

        public static int NextMove(Player player)
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
                var ennemies = Board.Players.Where(e => e != player);

                foreach (var p in
                    from e in ennemies
                    where e.Pawns.Any(pa => pa.Position == player.StartCase && pa.Type == CaseType.Classic)
                    select e.Pawns.First(pa => pa.Position == player.StartCase && pa.Type == CaseType.Classic))
                {
                    p.Type = CaseType.Square;
                    p.Position = 0;
                }
            }
            //Si pas 6 ou que la case de départ est occupée, on avance le premier pion disponible qui n'attends pas pour la fin de jeu
            else if (!player.Pawns.All(p => p.Type == CaseType.Square || p.Type == CaseType.EndGame))
            {
                player.Pawns.First(p => p.Type == CaseType.Classic && p.Position != Board.Normalize(player.StartCase - 1)).Move(roll);
            }
            //Si aucun des chemins ci-dessus n'a pu être utilisé, on passe son tour
            return roll;
        }

    }
}