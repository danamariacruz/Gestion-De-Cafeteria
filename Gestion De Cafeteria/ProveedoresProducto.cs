//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestion_De_Cafeteria
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProveedoresProducto
    {
        public int Id { get; set; }
        public Nullable<int> ProveedorId { get; set; }
        public Nullable<int> ProductoId { get; set; }
    
        public virtual Producto Producto { get; set; }
        public virtual Proveedore Proveedore { get; set; }
    }
}
