using ProvaWeek5.Helper;
using System;
using System.Collections.Generic;

namespace ProvaWeek5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Gestione spese ===");


            bool quit = false;
            do
            {
                string command = ConsoleHelpers.BuildMenu("Main Menu",
                    new List<string> {
                    "[ 1 ] - Elenco Spese",
                    "[ 2 ] - Aggiungi Spesa",
                    "[ 3 ] - Cancella Spesa",
                    "[ 4 ] - Approva una spesa",
                    "[ 5 ] - Elenco Ticket per Utente",
                    "[ 6 ] - Numero Spese per Categoria",
                    "[ 7 ] - Disconnected MODE Menù",
                    "[ q ] - ESCI"
                    });

                switch (command)
                {
                    case "1":
                        
                        EFClient.ListaSpese();
                        break;
                    case "2":
                        
                        EFClient.CreaSpesa();
                        break;
                    case "3":
                        
                        EFClient.EliminaSpesa();
                        break;
                    case "4":
                        
                        EFClient.ApprovaSpesa();
                        break;
                    case "5":
                        string userName = ConsoleHelpers.GetData("Nome Utente");
                        EFClient.ListaSpese(t => t.Utente == userName);
                        break;
                    case "6":
                        EFClient.ListaSpesePerCategoria();
                        break;
                    case "7":
                        DisconnectedMODE.Start();
                        break;

                    case "q":
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Comando sconosciuto.");
                        break;
                }

            } while (!quit);
        }
    }
}
