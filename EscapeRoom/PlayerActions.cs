using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    public class PlayerActions
    {
        AsciiSigns ascii = new AsciiSigns();

        private int playerPositionX, playerPositionY, newPositionX, newPositionY;

        private bool movement = true;        // darf sich der Player bewegen? (Wand etc.)
        private bool finished = false;
        private bool key = false;   // hat der Player den Schlüssel?

        private string wall, field, player, finishDoor, keyField;
        private string fieldCamefrom;

        // Speichert die Symbole (werden vom RoomCreator erstellt)
        public void CopySymbols(String _wall, String _field, String _player, String _finishDoor, String _keyField)
        {
            wall = _wall;
            field = _field;
            player = _player;
            finishDoor = _finishDoor;
            keyField = _keyField;

            fieldCamefrom = field;
        }

        public void SetPositions(int positionX, int positionY)
        {
            newPositionX = positionX;
            newPositionY = positionY;
            playerPositionX = positionX;
            playerPositionY = positionY;
        }

        // --- Bewegungsaktion des Players + Ausgabe des Rooms
        public void Move(String[,] room)
        {
            string fieldGoto;
            bool validMovement;
            movement = true;


            // Der Room wird ausgegeben
            for (int i = 0; i < room.GetLength(0); i++)
            {
                for (int j = 0; j < room.GetLength(1); j++)
                {
                    string activeField = room[i, j];

                    // für die verschiedenen Elemente (Schlüssel, Tür, ...) werden verschiedene Farben verwendet
                    if      (activeField == player)              Console.ForegroundColor = ConsoleColor.Cyan;
                    else if (activeField == keyField)            Console.ForegroundColor = ConsoleColor.DarkYellow;
                    else if (activeField == finishDoor && key)   Console.ForegroundColor = ConsoleColor.Green;
                    else if (activeField == finishDoor && !key)  Console.ForegroundColor = ConsoleColor.Red;
                    else if (activeField == wall)                Console.ForegroundColor = ConsoleColor.White;
                    else                                         Console.ForegroundColor = ConsoleColor.DarkGray;
                    
                    Console.Write(" " + activeField);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ResetColor();

            Console.WriteLine("Wohin moechtest du dich bewegen? \n" +
                              " ► Benutze für die Bewegung w/a/s/d oder die Pfeiltasten. \n");
            Console.CursorVisible = false;

            do  // Auswertung der Eingabe für die Bewegungsrichtung
            {
                ConsoleKeyInfo button = Console.ReadKey();      // Eingabe der Bewegunsrichtung vom Player

                switch (button.Key)  // Eingabeauswertung der Bewegungsrichtung
                {
                    case ConsoleKey.UpArrow or ConsoleKey.W:    // wenn Pfeiltaste nach oben oder W gedrückt wird
                        newPositionX = playerPositionX - 1;     // wird gespeichert, dass der Spieler nach oben laufen soll
                        validMovement = true;                   // und es war eine gültige Bewegungseingabe
                        break;

                    case ConsoleKey.DownArrow or ConsoleKey.S:
                        newPositionX = playerPositionX + 1;
                        validMovement = true;
                        break;

                    case ConsoleKey.LeftArrow or ConsoleKey.A:
                        newPositionY = playerPositionY - 1;
                        validMovement = true;
                        break;

                    case ConsoleKey.RightArrow or ConsoleKey.D:
                        newPositionY = playerPositionY + 1;
                        validMovement = true;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" ist nicht möglich zum bewegen, bitte befolge die obenstehenden Anweisungen.");
                        Console.ResetColor();
                        validMovement = false;
                        break;
                }
            }
            while (!validMovement); // Eingabe wird wiederholt, wenn die Eingabe ungültig war
            Console.CursorVisible = true;

            Console.Clear();
            Event(room);

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
            else
            {
                newPositionX = playerPositionX;     // die neuen Positionen werden zurückgesetzt auf die aktuellen
                newPositionY = playerPositionY;     // ... damit danach ordentlich damit weiter gearbeitet werden kann
            }

            while (finished == false)
            {
                Move(room);     // rekursiv: ruft sich selbst auf, bis das Spiel abgeschlossen wurde (Tür erreicht)
            }
        }


        // --- Events: wenn der Player auf/gegen ein spezielles Feld läuft
        public void Event(String[,] room)
        {
            Console.CursorVisible = false;
            if (room[newPositionX, newPositionY] == wall)           // Wand
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Du läufst gegen eine Wand *aua*");
                Console.ResetColor();
                movement = false;
            }
            if (room[newPositionX, newPositionY] == keyField)       // Schlüssel wurde gefunden
            {
                Console.Beep();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Du hast einen Schlüssel gefunden. " +
                                  "Vielleicht findest du etwas, was du damit aufschliessen kannst...");
                Console.ResetColor();
                room[newPositionX, newPositionY] = ".";
                key = true;
            }
            if (room[newPositionX, newPositionY] == finishDoor)     // finishDoor: Tür nach draussen
            {
                if (key)
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Vor dir befindet sich eine Tür, die nach draußen zu führen scheint. \n" +
                                      "Sie ist verschlossen, doch der Schlüssel, den du gefunden hast, scheint zu passen. \n" +
                                      "So gelingt es dir, die Tür aufzuschliessen und zu entkommen! \n");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();

                    Console.WriteLine(ascii.endSign);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Drücke eine beliebige Taste, um ins Menü zurück zu kehren. \n");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();

                    finished = true;
                }
                else
                {
                    Console.Beep();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Vor dir befindet sich eine Tür, die nach draußen zu führen scheint. " +
                                      "Allerdings ist sie verschlossen.");
                    Console.ResetColor();
                    movement = false;
                }
            }
            Console.CursorVisible = true;
        }

    }
}
