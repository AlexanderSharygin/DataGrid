namespace MVCGrid
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Workers
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
}
