using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace On_Call_Assistant.Models
{
    public class IsOnRotation
    {
        public int ID { get; set; }
        //[Key]
        [ForeignKey("employee")]
        public int employeeID { get; set; }
        //[Key]
        [ForeignKey("rotation")]
        public int rotationID { get; set; }

        public virtual Employee employee { get; set; }
        public virtual OnCallRotation rotation { get; set; }
    }
}