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
        public DataContext()
            : base("name=Model1")
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
    public class WorkerSmall
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

        public static explicit operator WorkerSmall(WorkersSmall v)
        {
            if (v != null)
            {
                WorkerSmall a = new WorkerSmall();
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
            else
            { return null; }

        }
    }
    public class PageInfo
    {
        public int PageNumber { get; set; } // ����� ������� ��������
        public int PageSize { get; set; } // ���-�� �������� �� ��������
        public int TotalItems { get; set; } // ����� ��������
        public int TotalPages  // ����� �������
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

