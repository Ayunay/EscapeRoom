using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    public class RoomCreator
    {
        AsciiSigns ascii = new AsciiSigns();

        public string wall, field, player, finishDoor, keyField;

        public string[,] room;
        private int hight = 4;
        private int with = 8;

        public int playerPositionX, playerPositionY;

        public void SetSymbols()
        {
            wall = "■";
            field = "·";
            player = "☺";
            finishDoor = "#";
            keyField = "F";
        }

        // Der Spieler soll angeben, wie groß der Room sein soll
        public void RoomData()
        {
            int roomLengthX = 0, roomWithY = 0;
            bool validLengthX = false, validLengthY = false;

            Console.WriteLine(ascii.roomCreatorSign);
            Console.WriteLine("Wähle nun die Größe deines Spielfeldes. \n" +
                              "Welche Breite (nach rechts) soll dein Feld haben? Wähle zwischen 5 und 15");

            while (!validLengthY)   // zuerst soll der Y-Wert festgelegt werden
            {
                string roomWithYinput = Console.ReadLine();           // Eingabe des Y-Wertes

                if (!int.TryParse(roomWithYinput, out roomWithY))   // Prüft, ob die Eingabe gültig war / eine Zahl ist
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Bitte gib eine Zahl ein");
                    Console.ResetColor();
                }
                else
                {
                    roomWithY = int.Parse(roomWithYinput);          // wandelt die eingegebene Zahl als string in int um

                    if (roomWithY >= 5 && roomWithY <= 15)          // Prüft, ob die eingegebene Zahl im Bereich liegt
                    {
                        validLengthY = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Dieser Wert liegt nicht in dem vorgegebenen Bereich.");
                        Console.ResetColor();
                    }
                }
            }

            Console.Clear();

            Console.WriteLine(ascii.roomCreatorSign);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Breite: " + roomWithY + "\n");
            Console.ResetColor();
            Console.WriteLine("Welche Länge (nach unten) soll dein Feld haben? Wähle zwischen 5 und 20");

            while (!validLengthX)   // nun soll der X-Wert festgelegt werden, in der gleichen Weise wie beim Y-Wert oben
            {

                string roomLengthXinput = Console.ReadLine();           // Eingabe des X-Wertes

                if (!int.TryParse(roomLengthXinput, out roomLengthX))   // Prüft, ob die EIngabe gültig war / eine Zahl ist
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Bitte gib eine Zahl ein");
                    Console.ResetColor();
                }
                else
                {
                    roomLengthX = int.Parse(roomLengthXinput);          // wandelt die eingegebene Zahl in int um

                    if (roomLengthX >= 5 && roomLengthX <= 20)          // Prüft, ob die eingegebene Zahl im Bereich liegt    
                    {
                        validLengthX = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Dieser Wert liegt nicht in dem vorgegebenen Bereich.");
                        Console.ResetColor();
                    }
                }
            }

            room = new string[roomLengthX, roomWithY];

            Console.Clear();
            Console.WriteLine(ascii.roomCreatorSign);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Breite: {roomWithY} \nLänge: {roomLengthX} \n");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Drücke nun eine beliebige Taste, um das Spiel zu beginnen");
            Console.ResetColor();
            Console.ReadKey();
        }


        // Der Raum wird nach dem Vorgaben des Spielers erstellt
        public string[,] CreateChosenRoom()
        {
            Random random = new Random();
            int keyX, keyY, doorX, doorY;

            // Initialisierung des Rooms 
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    room[i, j] = field;                 // erstmal werden alle Felder initialisiert

                    if (i == 0 || i == room.GetLength(0) - 1 || j == 0 || j == room.GetLength(1) - 1)
                    {
                        room[i, j] = wall;              // dann sollen alle Randfelder Wände sein
                    }
                }
            }


            do      // der Schlüssel soll auf einem freien Feld im Raum platziert werden
            {
                keyX = random.Next(1, room.GetLength(0) - 2);       // erstellt Zufallszahlen für die Positionen des Schlüssels
                keyY = random.Next(1, room.GetLength(1) - 2);       // innerhalb der äußeren Wände
            }
            while (room[keyX, keyY] == wall);                       // und nicht auf einer Wand (innere Wände)

            room[keyX, keyY] = keyField;                            // endgültige Platzierung im Array


            do      // die Schleife stellt sicher, dass der Spieler nicht auf dem Feld des Schlüssels oder in einer Wand spawnt 
            {
                playerPositionX = random.Next(1, room.GetLength(0) - 2);
                playerPositionY = random.Next(1, room.GetLength(1) - 2);
            }
            while (room[playerPositionX, playerPositionY] == keyField || room[playerPositionX, playerPositionY] == wall);

            room[playerPositionX, playerPositionY] = player;


            do      // Die Tür soll in einer der Wände und nicht in den 4 Eckpunkten platziert werden
            {
                doorX = random.Next(room.GetLength(0) - 1);
                doorY = random.Next(room.GetLength(1) - 1);
            }
            while (room[doorX, doorY] != wall || (doorX == 0 && doorY == 0) || (doorX == room.GetLength(0) - 1 && doorY == 0) ||
                  (doorX == 0 && doorY == room.GetLength(1) - 1) || (doorX == room.GetLength(0) - 1 && doorY == room.GetLength(1) - 1));

            room[doorX, doorY] = finishDoor;


            return room;
        }
    }
}
