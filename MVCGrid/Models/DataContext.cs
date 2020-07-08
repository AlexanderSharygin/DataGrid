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
    [Table("WorkersSmall")]
    public partial class WorkersSmall
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required]
        [Display(Name = "���")]
        [MaxLength(20, ErrorMessage = "��������� ���������� ����� ������.")]
        [Column(TypeName = "nvarchar")]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "�������")]
        [MaxLength(50, ErrorMessage = "��������� ���������� ����� ������.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(5)]
        [MaxLength(5, ErrorMessage = "��������� ���������� ����� ������.")]
        [Display(Name = "�������")]
        [Column(TypeName = "nvarchar")]
        public string Prefix { get; set; }

        [Required]
        [Display(Name = "���������")]
        [MaxLength(50, ErrorMessage = "��������� ���������� ����� ������.")]
        [StringLength(50)]
        public string Position { get; set; }

        [Required]
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "���� ��������")]
        public DateTime BirthDate { get; set; }

        [StringLength(500)]
        [Display(Name = "�������")]
        [MaxLength(500, ErrorMessage = "��������� ���������� ����� ������.")]
        public string Notes { get; set; }

        [Required]
        [Display(Name = "�����")]
        [StringLength(200)]
        [MaxLength(200, ErrorMessage = "��������� ���������� ����� ������.")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "�����")]
        public int StateID { get; set; }

        [Required]
        [Column(TypeName = "money")]
        [Display(Name = "���. �����")]

        [Range(1, 2000, ErrorMessage = "�������� ������ ���� � �������� 1-2000")]
        public decimal Salary { get; set; }

        [Display(Name = "���������")]
        public bool IsAlcoholic { get; set; }
    }
    public class IndexViewModel
    {
        public IEnumerable<WorkersSmall> Workers { get; set; }
       
        public PageInfo PageInfo { get; set; }
    }
}


