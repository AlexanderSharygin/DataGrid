namespace MVCGrid.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<Workers> Workers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workers>()
                .Property(e => e.Salary)
                .HasPrecision(19, 4);
        }
        class Worker
        {
            public int ID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Prefix { get; set; }
            public string Position { get; set; }
            public DateTime BirthDate { get; set; }
            public string Notes { get; set; }
            public string Address { get; set; }
            public int StateID { get; set; }
            //   public int Salary { get; set; }
            //  public bool IsAlcoholic { get; set; }
        }
    }
}

