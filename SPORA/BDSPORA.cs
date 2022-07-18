using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace SPORA
{
    public partial class BDSPORA : DbContext
    {
        public BDSPORA()
            : base("name=BDSPORA")
        {
        }

        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
