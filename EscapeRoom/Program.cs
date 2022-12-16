using System;
using System.Numerics;

namespace EscapeRoom
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool gameFlow = true;
            while (gameFlow)
            {
                Menu Menu = new Menu();
                RoomCreator PlayField = new RoomCreator();
                PlayerActions Player = new PlayerActions();

                string[,] room;

                PlayField.SetSymbols();     // Speichert die Symbole ... und setzt sie in PlayerActions und Menu ein
                Player.CopySymbols(PlayField.wall, PlayField.field, PlayField.player, PlayField.finishDoor, PlayField.midDoor, PlayField.keyField);
                Menu.CopySymbols(PlayField.player, PlayField.finishDoor, PlayField.keyField);

                // Beginn des Programmes bzw. Start des Menüs

                gameFlow = Menu.MenuSelector();            // Führt das Menü aus
            
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
                
                Player.Move(room);              // Movement Aktion des Spielers 

                Console.Clear();
            }
        }
    }
}