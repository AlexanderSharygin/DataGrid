namespace MVCGrid.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table ("WorkersSmall")]
    public partial class WorkersSmall
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
       
        [Range(1, 2000, ErrorMessage = "Значение должно быть в пределах 1-2000")]
        public decimal Salary { get; set; }

        [Display(Name = "Алкоголик")]
        public bool IsAlcoholic { get; set; }
    }
}
