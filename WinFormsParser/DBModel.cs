namespace Parser
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBModel : DbContext
    {
        public DBModel()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<Worker> Workers { get; set; }
        public virtual DbSet<WorkersSmall> WorkersSmalls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worker>().HasKey(s=>s.Id)
                .Property(e => e.Salary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<WorkersSmall>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<WorkersSmall>()
                .Property(e => e.Prefix)
                .IsFixedLength();

            modelBuilder.Entity<WorkersSmall>()
                .Property(e => e.Salary)
                .HasPrecision(19, 4);
          
        }
    }
}
