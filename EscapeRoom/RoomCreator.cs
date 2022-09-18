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
        public string wall, field, player, finishDoor, keyField;

        public string[,] room;
        private int hight = 4;
        private int with = 8;

        public int playerPositionX, playerPositionY;

        public void SetSymbols()
        {
            wall = "■";
            field = ".";
            player = "ß";
            finishDoor = "#";
            keyField = "?";
        }

        // Der Spieler soll angeben, wie groß der Room sein soll
        public void RoomData()
        {
            int roomLengthX = 0, roomWithY = 0;
            bool validLengthX = false, validLengthY = false;

            Console.WriteLine("Wähle nun die Größe deines Spielfeldes. \n" +
                              "Welche Breite (nach rechts) soll dein Feld haben? Wähle zwischen 5 und 15");

            while (!validLengthY)   // zuerst soll der Y-Wert festgelegt werden
            {
                string roomWithYinput = Console.ReadLine();           // Eingabe des Y-Wertes

                if (!int.TryParse(roomWithYinput, out roomWithY))   // Prüft, ob die Eingabe gültig war / eine Zahl ist
                {
                    Console.WriteLine("Bitte gib eine Zahl ein");
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
                        Console.WriteLine("Dieser Wert liegt nicht in dem vorgegebenen Bereich.");
                    }
                }
            }

            Console.Clear();

            Console.WriteLine("Breite: " + roomWithY + "\n\n" +
                              "Welche Länge (nach unten) soll dein Feld haben? Wähle zwischen 5 und 20");

            while (!validLengthX)   // nun soll der X-Wert festgelegt werden, in der gleichen Weise wie beim Y-Wert oben
            {

                string roomLengthXinput = Console.ReadLine();           // Eingabe des X-Wertes

                if (!int.TryParse(roomLengthXinput, out roomLengthX))   // Prüft, ob die EIngabe gültig war / eine Zahl ist
                {
                    Console.WriteLine("Bitte gib eine Zahl ein");
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
                        Console.WriteLine("Dieser Wert liegt nicht in dem vorgegebenen Bereich.");
                    }
                }
            }

            room = new string[roomLengthX, roomWithY];

            Console.Clear();
            Console.WriteLine($"Breite: {roomWithY} \nLänge: {roomLengthX} \n" +
                              "Drücke nun eine beliebige Taste, um das Spiel zu beginnen");
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


            do      // die Schleife stellt sicher, dass der Schlüssel und der Spieler nicht auf dem selben Feld spawnen
            {
                keyX = random.Next(1, room.GetLength(0) - 2);       // erstellt Zufallszahlen für die Positionen des Schlüssels...
                keyY = random.Next(1, room.GetLength(1) - 2);

                playerPositionX = random.Next(1, room.GetLength(0) - 2);    // ... und für die Startposition des Spielers
                playerPositionY = random.Next(1, room.GetLength(1) - 2);
            }
            while (keyX == playerPositionX && keyY == playerPositionY);


            do      // Die Tür soll in einer der Wände und nicht in den 4 Eckpunkten platziert werden
            {
                doorX = random.Next(room.GetLength(0) - 1);
                doorY = random.Next(room.GetLength(1) - 1);
            }
            while (room[doorX, doorY] != wall || (doorX == 0 && doorY == 0) || (doorX == room.GetLength(0) - 1 && doorY == 0) ||
                  (doorX == 0 && doorY == room.GetLength(1) - 1) || (doorX == room.GetLength(0) - 1 && doorY == room.GetLength(1) - 1));


            // Platzierung der Tür, des Schlüssels und des Spielers im Room
            room[doorX, doorY] = finishDoor;
            room[keyX, keyY] = keyField;
            room[playerPositionX, playerPositionY] = player;

            return room;
        }
    }
}
