namespace EscapeRoom
{
    internal class Program
    {
        static string[,] room;

        static string walli = "|", wallj = "_", field = ".", player = "ß", finishDoor = "#", keyField = "?";

        static int playerPositionX = 3, playerPositionY = 5;    // Positionen des Players
        static int newPositionX = playerPositionX, newPositionY = playerPositionY;
        static string fieldCamefrom = ".";
        static string fieldGoto;

        static int roomLengthX, roomLengthY;
        static int doorPositionX, doorPositionY;
        static int keyPositionX, keyPositionY;

        static bool movement = true;        // darf sich der Player bewegen? (Wand etc.)
        static bool key = false;            // hat der Player den Schlüssel?
        static bool finished = false;       // Konnte der Player den Raum verlassen?
        static bool closeGame = false;


        //Spielfeld Labyrinth = new Spielfeld();

        static void Main()
        {
            Menu();

            if (!closeGame)
            {
                Console.Clear();
                RoomData();

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

        // --- Initialisierung des Rooms als 7x8 Feld
        static void CreateRoom7()
        {
            room = new string[,] { { " ", "_", "_", "_", "_", "_", "_", " " },  // Erstellung des Rooms
                                   { "|", ".", "?", ".", ".", ".", ".", "|" },
                                   { "|", ".", ".", ".", ".", ".", ".", "|" },
                                   { "|", ".", ".", ".", ".", ".", ".", "#" },
                                   { "|", ".", ".", ".", ".", ".", ".", "|" },
                                   { "|", ".", ".", ".", ".", ".", ".", "|" },
                                   { "|", "_", "_", "_", "_", "_", "_", "|" } };

            room[playerPositionX, playerPositionY] = player;    // Startposition des Players
        }

        // Der Spieler kann angeben, wie groß der Room sein soll
        static void RoomData()
        {
            bool validLengthX = false, validLengthY = false;

            Console.WriteLine("Wähle nun die Größe deines Spielfeldes. \n" +
                              "Welche Breite (nach rechts) soll dein Feld haben? Wähle zwischen 4 und 9");

            while (!validLengthY)
            {
                Char roomLengthYinput = Console.ReadKey(true).KeyChar;      // Eingabe des Y-Wertes
                roomLengthY = int.Parse(new string(roomLengthYinput, 1));   // und deren Umwandlung in ein int

                if (roomLengthY >= 4 && roomLengthY <= 10)                  // Überprüfung, ob die eingegebene Zahl gültig ist
                {
                    validLengthY = true;

                    Console.WriteLine("Breite: " + roomLengthY + "\n\n" +
                                      "Welche Länge (nach unten) soll dein Feld haben? Wähle zwischen 4 und 9");

                    while (!validLengthX)
                    {
                        Char roomLengthXinput = Console.ReadKey(true).KeyChar;      // Eingabe des X-Wertes
                        roomLengthX = int.Parse(new string(roomLengthXinput, 1));   // und deren Umwandlung in ein int

                        if (roomLengthX >= 4 && roomLengthX <= 10)           // Überprüfung, ob die eingegebene Zahl gültig ist    
                        {
                            validLengthX = true;

                            Console.WriteLine("Länge: " + roomLengthX + "\n\n" +
                                              "Drücke nun eine beliebige Taste, um das Spiel zu beginnen.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Diese Eingabe ist ungültig.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Diese Eingabe ist ungültig.");
                }
            }     
        }
 
        static void CreateChosenRoom()
        {
            room = new string[roomLengthX, roomLengthY];

            for (int i = 0; i < roomLengthX; i++)
            {
                for (int j = 0; j < roomLengthY; j++)
                {
                    room[i, j] = ".";
                    
                    if (i == 0 || i == roomLengthX - 1)
                    {
                        room[i, j] = "_";
                    }
                    if(j == 0 || j == roomLengthY - 1)
                    {
                        room[i, j] = "|";
                    }
                }
            }
        }


        // --- Bewegungsaktion des Players + Ausgabe des Rooms
        static void Move()
        {
            bool validMovement;
            movement = true;

            // Der Room wird ausgegeben
            for (int i = 0; i < roomLengthY; i++)
            {
                for (int j = 0; j < roomLengthX; j++)             // !! i und j < room.length() ??
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

            if(movement) // wird nur ausgeführt, wenn sich der Player auf das angegebene Feld bewegen darf (keine Wand etc.)
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
            if (room[newPositionX, newPositionY] == walli || room[newPositionX, newPositionY] == wallj) // Wand
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
                if(key)
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
                        playerPositionX = 3;
                        playerPositionY = 5;
                        newPositionX = playerPositionX;
                        newPositionY = playerPositionY;
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