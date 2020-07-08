namespace MVCGrid.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;
    using System.ComponentModel.DataAnnotations;

    public partial class DataContext : DbContext
    {
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
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class IndexViewModel
    {
        public IEnumerable<WorkersSmall> Workers { get; set; }
       
        public PageInfo PageInfo { get; set; }
    }
}


