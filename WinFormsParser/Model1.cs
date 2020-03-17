namespace Parser
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=ParserDB")
        {
        }

        public virtual DbSet<Workers> Workers { get; set; }
        public virtual DbSet<WorkersSmall> WorkersSmall { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workers>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<Workers>()
                .Property(e => e.Prefix)
                .IsFixedLength();

            modelBuilder.Entity<Workers>()
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
