using System;
using System.IO;
using System.Linq;
using PetitsChevaux.Game;

namespace PetitsChevaux
{
    class Program
    {


        static void Main(string[] args)
        {
            int[] wins = { 0, 0 };

            Board.PlayerNumber = 2;
            Board.PawnsPerPlayer = 4;

            Console.WriteLine("[Player 1 : {0}, Player 2 : {1}] = {2}", wins[0], wins[1], wins.Sum());
            String results = null;
            Enumerable.Range(0, 1000).AsParallel().ForAll(a =>
            {
                Boolean run = true;
                Board board = new Board();

                Board.PlayerNumber = 2;
                Board.PawnsPerPlayer = 4;

                while (run)
                {
                    board.NextTurn();
                    if (board.Players.Any(p => p.Won))
                    {
                        run = false;
                        var winner = board.Players.First(p => p.Won);
                        wins[winner.Id]++;
                        results = String.Format("[Player 1 : {0}, Player 2 : {1}] = {2}", wins[0], wins[1], wins.Sum());
                        Console.WriteLine(results);
                    }
                }

            });


            using (var fs = File.AppendText("results.txt"))
            {
                fs.Write(results + fs.NewLine);
                fs.Flush();
            }

            Console.WriteLine("Finished");
            Console.ReadLine();

        }
    }
}
