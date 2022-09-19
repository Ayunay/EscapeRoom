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
        private int playerPositionX, playerPositionY, newPositionX, newPositionY;

        private bool movement = true;        // darf sich der Player bewegen? (Wand etc.)
        private bool finished = false;
        private bool key = false;   // hat der Player den Schlüssel?

        private string wall, field, player, finishDoor, keyField;
        private string fieldCamefrom = ".";

        // Speichert die Symbole (werden vom RoomCreator erstellt)
        public void CopySymbols(String _wall, String _field, String _player, String _finishDoor, String _keyField)
        {
            wall = _wall;
            field = _field;
            player = _player;
            finishDoor = _finishDoor;
            keyField = _keyField;
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
                for (int j = 0; j < room.GetLength(1); j++)             // !! i und j < room.length() ??
                {
                    // für die verschiedenen Elemente (Schlüssel, Tür, ...) werden verschiedene Farben verwendet
                    if (room[i, j] == player) Console.ForegroundColor = ConsoleColor.Cyan;
                    else if (room[i, j] == keyField) Console.ForegroundColor = ConsoleColor.Yellow;
                    else if (room[i, j] == finishDoor && key) Console.ForegroundColor = ConsoleColor.Green;
                    else if (room[i, j] == finishDoor && !key) Console.ForegroundColor = ConsoleColor.Red;
                    else if (room[i, j] == wall) Console.ForegroundColor = ConsoleColor.White;
                    else Console.ForegroundColor = ConsoleColor.DarkGray;
                    
                    Console.Write(" " + room[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.ResetColor();

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
            if (room[newPositionX, newPositionY] == wall)           // Wand
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

    }
}
