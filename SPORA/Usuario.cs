namespace SPORA
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(60)]
        public string Nombre { get; set; }

        [StringLength(60)]
        public string Apellido { get; set; }

        public int? Edad { get; set; }

        [StringLength(50)]
        public string Correo { get; set; }

        [StringLength(12)]
        public string Telefono { get; set; }

        [StringLength(100)]
        public string Imagen { get; set; }
    }
}
