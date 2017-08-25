namespace MOE.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ControllerType
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]       
        public int ControllerTypeID { get; set; }

       
        [StringLength(50)]
        public string Description { get; set; }

       
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public long SNMPPort { get; set; }

       
        public string FTPDirectory { get; set; }

   
        public bool ActiveFTP { get; set; }

     
        [StringLength(50)]
        public string UserName { get; set; }

     
        [StringLength(50)]
        public string Password { get; set; }

   

    }
}
