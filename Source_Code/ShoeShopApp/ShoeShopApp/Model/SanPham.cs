//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShoeShopApp.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.ChiaTietHoaDons = new HashSet<ChiaTietHoaDon>();
        }
    
        public int MaSP { get; set; }
        public string TenSP { get; set; }
        public int MaLoaiSP { get; set; }
        public decimal Gia { get; set; }
        public string Mau { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string Anh { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiaTietHoaDon> ChiaTietHoaDons { get; set; }
        public virtual LoaiSP LoaiSP { get; set; }
    }
}
