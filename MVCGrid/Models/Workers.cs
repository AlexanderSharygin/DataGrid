namespace MVCGrid
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Workers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(5)]
        public string Prefix { get; set; }

        [Required]
        [StringLength(50)]
        public string Position { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime BirthDate { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        public int StateID { get; set; }

        [Column(TypeName = "money")]
        public decimal Salary { get; set; }

        public bool IsAlcoholic { get; set; }
    }
    [Table("WorkersSmall")]
    public partial class WorkersSmall
    {
       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]  
        public int Id { get; set; }

        [Required]
        [Display(Name = "���")]
        [MaxLength(50, ErrorMessage = "��������� ���������� ����� ������.")]
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
}
