namespace MVCGrid.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=DBModel")
        {
        }

        public virtual DbSet<Workers> Workers { get; set; }

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
        }
    }
        public partial class Workers
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
            public decimal Salary { get; set; }

            [Display(Name = "���������")]
            public bool IsAlcoholic { get; set; }
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
        public IEnumerable<Workers> Workers { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
