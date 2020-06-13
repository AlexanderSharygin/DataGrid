namespace MVCGrid.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography.X509Certificates;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<Workers> Workers { get; set; }
        public virtual DbSet<WorkersSmall> WorkersSmall { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Workers>()
                .Property(e => e.Salary)
                .HasPrecision(19, 4);

            modelBuilder.Entity<WorkersSmall>()
              .Property(e => e.Salary)
              .HasPrecision(19, 4);
        }
    }


   public class Worker
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Prefix { get; set; }
        public string Position { get; set; }
        public DateTime BirthDate { get; set; }
        public string Notes { get; set; }
        public string Address { get; set; }
        public int StateID { get; set; }
        public decimal Salary { get; set; }
        public bool IsAlcoholic { get; set; }

        public static explicit operator Worker(Workers v)
        {
            Worker a = new Worker();
            a.Id = v.Id;
            a.FirstName = v.FirstName;
            a.LastName = v.LastName;
            a.Prefix = v.Prefix;
            a.Position = v.Position;
            a.BirthDate = v.BirthDate;
            a.Notes = v.Notes;
            a.Address = v.Address;
            a.StateID = v.StateID;
            a.Salary = v.Salary;
            a.IsAlcoholic = v.IsAlcoholic;
            return a;
            
        }
        public static explicit operator Worker(WorkersSmall v)
        {
            Worker a = new Worker();
            a.Id = v.Id;
            a.FirstName = v.FirstName;
            a.LastName = v.LastName;
            a.Prefix = v.Prefix;
            a.Position = v.Position;
            a.BirthDate = v.BirthDate;
            a.Notes = v.Notes;
            a.Address = v.Address;
            a.StateID = v.StateID;
            a.Salary = v.Salary;
            a.IsAlcoholic = v.IsAlcoholic;
            return a;

        }
        //   public int Salary { get; set; }
        //  public bool IsAlcoholic { get; set; }
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


