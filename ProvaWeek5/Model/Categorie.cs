using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProvaWeek5.Model
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Categoria { get; set; }

        public IList<Spesa> Spese { get; set; }

    }
}
