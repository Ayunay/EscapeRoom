using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeRoom
{
    public class Menu
    {
        public bool closeGame = false;

        // --- Menüauswahl
        public void MenuSelector()
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
                        /*finished = false;
                        key = false;
                        fieldCamefrom = ".";*/
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
