using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tiempo.Models
{
    [Table("Tiempo")]
    public class Tiempo2
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Municipio { get; set; }
        public string GpxX { get; set; }
        public string GpxY { get; set; }
        public string Region { get; set; }
        public string ultimaHora { get; set; }
        public string Temperatura { get; set; }        
        public string DescripcionTiempo { get; set; }
        public string PathImg { get; set; }        
        public string VelocidadViento { get; set; }
        public string Precipitaciones { get; set; }
    }
}
