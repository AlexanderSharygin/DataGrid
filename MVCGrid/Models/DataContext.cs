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
            [Display(Name = "Имя")]
            [MaxLength(20, ErrorMessage = "Превышена допустимая длина строки.")]
            [Column(TypeName = "nvarchar")]
            [StringLength(20)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Фамилия")]
            [MaxLength(50, ErrorMessage = "Превышена допустимая длина строки.")]
            [StringLength(50)]
            public string LastName { get; set; }

            [Required]
            [StringLength(5)]
            [MaxLength(5, ErrorMessage = "Превышена допустимая длина строки.")]
            [Display(Name = "Префикс")]
            [Column(TypeName = "nvarchar")]
            public string Prefix { get; set; }

            [Required]
            [Display(Name = "Должность")]
            [MaxLength(50, ErrorMessage = "Превышена допустимая длина строки.")]
            [StringLength(50)]
            public string Position { get; set; }

            [Required]
            [Column(TypeName = "smalldatetime")]
            [Display(Name = "Дата Рождения")]
            public DateTime BirthDate { get; set; }

            [StringLength(500)]
            [Display(Name = "Заметки")]
            [MaxLength(500, ErrorMessage = "Превышена допустимая длина строки.")]
            public string Notes { get; set; }

            [Required]
            [Display(Name = "Адрес")]
            [StringLength(200)]
            [MaxLength(200, ErrorMessage = "Превышена допустимая длина строки.")]
            public string Address { get; set; }

            [Required]
            [Display(Name = "Номер")]
            public int StateID { get; set; }

            [Required]
            [Column(TypeName = "money")]
            [Display(Name = "Зар. плата")]
            public decimal Salary { get; set; }

            [Display(Name = "Алкоголик")]
            public bool IsAlcoholic { get; set; }
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
        public IEnumerable<Workers> Workers { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}
