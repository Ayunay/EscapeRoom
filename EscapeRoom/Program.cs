using System;

namespace EscapeRoom
{
    public class Program
    {
        static string[,] room;

        static string wall = "■", field = ".", player = "ß", finishDoor = "#", keyField = "?";

        static int playerPositionX, playerPositionY;    // Positionen des Players
        static int newPositionX, newPositionY;
        static string fieldCamefrom = ".";

        static bool movement = true;        // darf sich der Player bewegen? (Wand etc.)
        static bool key = false;            // hat der Player den Schlüssel?
        static bool finished = false;       // Konnte der Player den Raum verlassen?
        static bool closeGame = false;


        //Spielfeld Labyrinth = new Spielfeld();

        static void Main()
        {
            RoomCreator Room1 = new RoomCreator();
            Room1.SimpleOutput();

            Menu();

            if (!closeGame)
            {
                Console.Clear();
                RoomData();                 // Der Spieler gibt die Längen für den Raum ein

                Console.Clear();
                CreateChosenRoom();         // Room wird basierend auf den eingegebenen Daten erstellt
                //CreateRoom7();              // Room wird erstellt

                while (finished == false)   // Bis das Ziel erreicht wurde...
                {
                    Move();                 // ... bewegt sich der Player und tritt dabei evtl auf besondere Felder (Event())
                }

                Main();
                Console.Clear();
            }

        }

        // Der Spieler soll angeben, wie groß der Room sein soll
        static void RoomData()
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
        static void CreateChosenRoom()
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

            newPositionX = playerPositionX;
            newPositionY = playerPositionY;
        }


        // --- Bewegungsaktion des Players + Ausgabe des Rooms
        static void Move()
        {
            string fieldGoto;
            bool validMovement;
            movement = true;

            // Der Room wird ausgegeben
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)             // !! i und j < room.length() ??
                {
                    Console.Write(" " + room[i, j]);    // Room wird in der Konsole gezeichnet
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Wohin moechtest du dich bewegen? w = oben | a = links | s = unten | d = rechts \n");

            do  // Auswertung der Eingabe für die Bewegungsrichtung
            {
                char moveDirection = Console.ReadKey(true).KeyChar;     // Eingabe der Bewegunsrichtung vom Player

                switch (moveDirection)  // Eingabeauswertung der Bewegungsrichtung
                {
                    case 'w':
                        newPositionX = playerPositionX - 1;
                        validMovement = true;
                        break;

                    case 's':
                        newPositionX = playerPositionX + 1;
                        validMovement = true;
                        break;

                    case 'a':
                        newPositionY = playerPositionY - 1;
                        validMovement = true;
                        break;

                    case 'd':
                        newPositionY = playerPositionY + 1;
                        validMovement = true;
                        break;

                    default:
                        Console.WriteLine("Diese Bewegung ist nicht moeglich, bitte druecke w/a/s/d");
                        validMovement = false;
                        break;
                }
            }
            while (!validMovement); // Eingabe wird wiederholt, wenn die Eingabe ungueltig war

            Console.Clear();
            Event();

            if (movement) // wird nur ausgeführt, wenn sich der Player auf das angegebene Feld bewegen darf (keine Wand etc.)
            {
                // Aktualisierung der Felder, wenn der Player läuft
                fieldGoto = room[newPositionX, newPositionY];           // Feld wo ich hin will wird gespeichert

                room[newPositionX, newPositionY] = player;              // Player wird bewegt
                room[playerPositionX, playerPositionY] = fieldCamefrom; // * Feld von dem sich Player weg bewegt wird zurück gesetzt

                playerPositionX = newPositionX;                         // Positionen werden zum weiteren Gebrauch angepasst
                playerPositionY = newPositionY;                         // -> die "aktuellen" Positionen werden zu den "neuen"

                fieldCamefrom = fieldGoto;                              // Übergabe, damit * funktioniert
            }
        }


        // --- Events: wenn der Player auf/gegen ein spezielles Feld läuft
        static void Event()
        {
            if (room[newPositionX, newPositionY] == wall) // Wand
            {
                Console.WriteLine("Du läufst gegen eine Wand *aua*");
                movement = false;
            }
            if (room[newPositionX, newPositionY] == keyField)       // Schlüssel wurde gefunden
            {
                Console.WriteLine("Du hast einen Schlüssel gefunden. " +
                                  "Vielleicht findest du etwas, was du damit aufschliessen kannst...");
                room[newPositionX, newPositionY] = ".";
                key = true;
            }
            if (room[newPositionX, newPositionY] == finishDoor)     // finishDoor: Tür nach draussen
            {
                if (key)
                {
                    Console.WriteLine("Vor dir befindet sich eine Tür, die nach draußen zu führen scheint. \n" +
                                      "Sie ist verschlossen, doch der Schlüssel, den du gefunden hast, scheint zu passen. \n" +
                                      "So gelingt es dir, die Tür aufzuschliessen und zu entkommen! \n \n" +
                                      "Drücke eine beliebige Taste, um ins Menü zurück zu kehren. \n");
                    Console.ReadKey();
                    Console.Clear();

                    finished = true;
                }
                else
                {
                    Console.WriteLine("Vor dir befindet sich eine Tür, die nach draußen zu führen scheint. " +
                                      "Allerdings ist sie verschlossen.");
                }
            }

        }


        // --- Menüauswahl
        static void Menu()
        {
            bool validMenuselection = false;

            Console.WriteLine("Was möchtest du tun? \n" +
                                "1. Escape Room neu beginnen \n" +
                                "2. Spiel beenden \n");

            while (!validMenuselection)
            {
                char menuSelection = Console.ReadKey(true).KeyChar;   // Eingabe von Player + Auswertung unten

                switch (menuSelection)
                {
                    case '1':
                        validMenuselection = true;

                        // Alle relevanten Daten, die sich im Spielverlauf ändern, müssen zurückgesetzt werden
                        finished = false;
                        key = false;
                        fieldCamefrom = ".";
                        break;

                    case '2':
                        validMenuselection = true;
                        closeGame = true;
                        break;

                    default:
                        Console.WriteLine("Dies ist eine ungültige Angabe, bitte wähle zwischen 1 und 2");
                        break;
                }
            }
        }

    }
}