using System;
using System.Numerics;

namespace EscapeRoom
{
    public class Program
    {
        static void Main()
        {
            Menu Menu = new Menu();
            RoomCreator PlayField = new RoomCreator();
            PlayerActions Player = new PlayerActions();

            string[,] room;

            bool closeGame = Menu.closeGame;    // soll das Spiel geschlossen werden?

            PlayField.SetSymbols();             // Speichert die Symbole ... und setzt sie in PlayerActions ein
            Player.CopySymbols(PlayField.wall, PlayField.field, PlayField.player, PlayField.finishDoor, PlayField.keyField);
            Menu.CopySymbols(PlayField.wall, PlayField.field, PlayField.player, PlayField.finishDoor, PlayField.keyField);

            // Beginn des Programmes bzw. Start des Menüs

            Menu.MenuSelector();        // Führt das Menü aus
            closeGame = Menu.closeGame; // und prüft, ob das Spiel beendet werden soll

            if (!closeGame)
            {
                Console.Clear();
                Menu.GameExplanation();

                Console.Clear();
                PlayField.RoomData();           // Der Spieler gibt die Längen für den Raum ein

                Console.Clear();
                Menu.GameStart();

                Console.Clear();
                room = PlayField.CreateChosenRoom();    // Room wird basierend auf den eingegebenen Daten erstellt
                Player.SetPositions(PlayField.playerPositionX, PlayField.playerPositionY);  // die generierten Spielerpositionen
                                                                                            // werden zu PlayerActions übergeben

                Player.Move(room);      // Movement Aktion des Spielers 

                Console.Clear();
                Main();
            }
        }
    }
}