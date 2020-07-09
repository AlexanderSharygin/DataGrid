namespace Parser
{
    using System.Data.Entity;

    public partial class DBModel : DbContext
    {
        public DBModel()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<Worker> Workers { get; set; }
      

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worker>().HasKey(s=>s.Id)
                .Property(e => e.Salary)
                .HasPrecision(19, 4);

          
          
        }
    }
}
