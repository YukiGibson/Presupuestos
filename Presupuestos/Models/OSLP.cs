namespace Presupuestos.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OSLP")]
    public partial class OSLP
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short SlpCode { get; set; }

        [Required]
        [StringLength(155)]
        public string SlpName { get; set; }

        [StringLength(50)]
        public string Memo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Commission { get; set; }

        public short? GroupCode { get; set; }

        [StringLength(1)]
        public string Locked { get; set; }

        [StringLength(1)]
        public string DataSource { get; set; }

        public short? UserSign { get; set; }

        public int? EmpID { get; set; }

        public string U_Supervisor { get; set; }

        public short U_Activo { get; set; }

        public string U_Email { get; set; }
    }
}
