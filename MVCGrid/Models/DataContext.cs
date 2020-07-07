namespace MVCGrid.Models
{
    using System.Data.Entity;


    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<WorkersSmall> WorkersSmall { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

