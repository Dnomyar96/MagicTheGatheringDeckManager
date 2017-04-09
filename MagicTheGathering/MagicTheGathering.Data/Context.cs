using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagicTheGathering.Models;

namespace MagicTheGathering.Data
{
    class Context : DbContext
    {
        public Context() : base()
        {

        }

        public DbSet<Card> Cards { get; set; }
    }
}
