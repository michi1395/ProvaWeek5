using Microsoft.EntityFrameworkCore;
using ProvaWeek5.EF;
using ProvaWeek5.Helper;
using ProvaWeek5.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaWeek5
{
    public class EFClient
    {
        public static void ListaSpese(Func<Spesa, bool> filter = null, bool showPrompt = true)
        {
            using SpesaContext ctx = new();

            IEnumerable<Spesa> speseToList;
            if (filter != null)
                speseToList = ctx.Spese
                    .Include(t => t.Categoria)
                    .Where(filter);
            else
                speseToList = ctx.Spese
                    .Include(t => t.Categoria);

            Console.Clear();
            Console.WriteLine("---- Lista spese ----");
            Console.WriteLine();
            Console.WriteLine("{0,-5}{1,-40}{2,10}{3,20}{4,20}{5,15}{6,20}", "ID", "Data", "Descrizione", "Categoria", "Utente", "Importo", "Approvato");
            Console.WriteLine(new String('-', 110));

            foreach (Spesa s in speseToList)
            {
                Console.WriteLine("{0,-5}{1,-40}{2,10}{3,20}{4,20}{5,15}{6,20}",
                    s.Id, s.Data.ToString("dd-MMM-yyyy"), s.Descrizione, s.Categoria?.Categoria, s.Utente, s.Importo, s.Approvato);
            }
            Console.WriteLine(new String('-', 110));

            if (!showPrompt)
                return;

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        public static void CreaSpesa()
        {
            using SpesaContext ctx = new();

            Console.Clear();
            Console.WriteLine("---- Inserire una nuova Spesa ----");

            string descrizione = ConsoleHelpers.GetData("Descrizione");
            string utente = ConsoleHelpers.GetData("Utente");
            decimal importo = decimal.Parse(ConsoleHelpers.GetData("Importo"));

            string categoriaName = ConsoleHelpers.GetData("Categoria");

            var selectedCategory = ctx.Categorie.FirstOrDefault(
                c => c.Categoria.ToUpper() == categoriaName.ToUpper()
            );

            Spesa newSpesa = new()
            {
                Data = DateTime.Now,
                Categoria=selectedCategory,
                Descrizione = descrizione,
                Utente = utente,
                Importo=importo,
                Approvato=false,
                
            };

            ctx.Spese.Add(newSpesa);
          

            ctx.SaveChanges();

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

       public static void EliminaSpesa()
        {
            using SpesaContext ctx = new();

            Console.Clear();
            Console.WriteLine("---- Cancellare una Spesa ----");

            ListaSpese(showPrompt: false);

            string idValue = ConsoleHelpers.GetData("ID della spesa da cancellare");
            int.TryParse(idValue, out int id);

            var spesaToDelete = ctx.Spese.Find(id);

            if (spesaToDelete != null)
                ctx.Spese.Remove(spesaToDelete);
           

            ctx.SaveChanges();

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        public static void ApprovaSpesa()
        {
            using SpesaContext ctx = new();

            Console.Clear();
            Console.WriteLine("---- Approva una Spesa ----");

            ListaSpese(showPrompt: false);

            string idValue = ConsoleHelpers.GetData("ID della spesa da approvare");
            int.TryParse(idValue, out int id);

            var spesaToChange = ctx.Spese.Find(id);

            if (spesaToChange != null)
            {
                spesaToChange.Approvato = true;
                ctx.Spese.Update(spesaToChange);
            }

            ctx.SaveChanges();

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }
        public static void ListaSpesePerCategoria()
        {
            using SpesaContext ctx = new();

            Console.Clear();
            Console.WriteLine("---- Spese per Categoria ----");
            Console.WriteLine("---------------------------------");
            Console.WriteLine("{0,-15} {1, 20}", "Categorie", "# Spese");
            Console.WriteLine("---------------------------------");

            foreach (var item in ctx.Categorie.Include(s => s.Spese))
                Console.WriteLine("{0,-15} {1, 20}",
                    item.Categoria, item.Spese.Sum(t => t.Importo));
            Console.WriteLine("---------------------------------");
            Console.WriteLine();

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }
    }
}
