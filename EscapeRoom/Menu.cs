﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    public class Menu
    {
        AsciiSigns ascii = new AsciiSigns();
        public bool closeGame = false;
        private string wall, field, player, finishDoor, keyField;

        public void CopySymbols(String _wall, String _field, String _player, String _finishDoor, String _keyField)
        {
            wall = _wall;
            field = _field;
            player = _player;
            finishDoor = _finishDoor;
            keyField = _keyField;
        }

        // --- Menüauswahl
        public void MenuSelector()
        {
            bool validMenuselection = false;

            while (!validMenuselection)
            {
                Console.WriteLine(ascii.menuSign);
                Console.WriteLine("\nWas möchtest du tun? \n" +
                                  "1. Escape Room neu beginnen \n" +
                                  "2. Spiel beenden \n");

                char menuSelection = Console.ReadKey(true).KeyChar;   // Eingabe von Player + Auswertung unten

                Console.Clear();

                switch (menuSelection)
                {
                    case '1':
                        validMenuselection = true;
                        break;

                    case '2':
                        validMenuselection = true;
                        closeGame = true;
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine(ascii.errorSign);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"Du hast > {menuSelection} < eingegeben. \n" +
                                           "Dies ist eine ungültige Angabe, bitte wähle zwischen den angegebenen Punkten \n");
                        Console.ResetColor();
                        Console.ReadKey();
                        Console.Clear();
                        break;
                }
            }
        }

        public void GameExplanation()
        {
            Console.WriteLine(ascii.gameExplanationSign);

            // Erklärung des Raumes an sich
            Console.WriteLine("Das Spielfeld besteht nur aus dem Raum, in dem du dich befindest.");

            // Spieler
            Console.Write(" ► Dein Charakter wird durch das ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(player);
            Console.ResetColor();
            Console.WriteLine(" dargestellt.");

            // Tür
            Console.Write(" ► Die Tür nach draußen wird mit ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(finishDoor);
            Console.ResetColor();
            Console.WriteLine(" symbolisiert.");

            // Schlüssel
            Console.Write(" ► Ein misteriöser Gegenstand, mit dem du vielleicht noch etwas anfangen kannst, " +
                          "versteckt sich hinter dem Zeichen ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write(keyField);
            Console.ResetColor();
            Console.WriteLine(".");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nBitte drücke eine beliebige Taste, um dein Raum zu erstellen");
            Console.ResetColor();

            Console.ReadKey(); 
        }

        public void GameStart()
        {
            Console.WriteLine(ascii.startSign);
            Console.ReadKey();
            Console.Clear();

            // Aktuelle Situation
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Du wachst in einem düsteren Raum auf. Alle Erinnerungen scheinen wie weggeflogen. \n" +
                              "Du beschliesst, an die frische Luft zu gehen, vielleicht fällt dir dann wieder ein," +
                              "was passiert ist. \n... Du machst dich also auf die Suche nach der Tür die nach draußen führt ... \n");

            // Start Game
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nDrücke eine Taste um mit der Suche zu beginnen.");
            Console.ResetColor();
            Console.ReadKey();
        }

    }
}
