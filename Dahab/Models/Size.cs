//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dahab.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Size
    {
        public int ID { get; set; }
        public int Size1 { get; set; }
        public int Cat_ID { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
