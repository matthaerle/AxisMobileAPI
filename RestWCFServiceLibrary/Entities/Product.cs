//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RestWCFServiceLibrary.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.InventoryGroupProducts = new HashSet<InventoryGroupProduct>();
            this.ProductStockAdjustments = new HashSet<ProductStockAdjustment>();
            this.FirearmInventories = new HashSet<FirearmInventory>();
            this.SerialUPCs = new HashSet<SerialUPC>();
        }
    
        public long ProductID { get; set; }
        public int CompanyID { get; set; }
        public int StoreID { get; set; }
        public string ProductUPC { get; set; }
        public string ManufacturerReference { get; set; }
        public string ProductDescription { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public int PhysicalQoH { get; set; }
        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public string AutoOrder { get; set; }
        public Nullable<System.Guid> ProductImageId { get; set; }
        public Nullable<int> AccountingCategoryID { get; set; }
        public bool IsFirearm { get; set; }
        public bool IsProductKit { get; set; }
        public int ManufacturerID { get; set; }
        public int DepartmentID { get; set; }
        public int ItemID { get; set; }
        public int UpdateUserID { get; set; }
        public System.DateTime UpdateDateTime { get; set; }
        public string UpdateMachineName { get; set; }
        public bool Active { get; set; }
        public string ExtendedDescription { get; set; }
        public bool IsStockItem { get; set; }
        public bool IsCase { get; set; }
        public decimal LastCost { get; set; }
        public decimal AverageCost { get; set; }
        public int QtyOnOrder { get; set; }
        public int QtyOnOrderExcluded { get; set; }
        public int QtyCommitted { get; set; }
        public bool IsLicense { get; set; }
        public Nullable<int> StateVendorID { get; set; }
        public int UnvoucheredQty { get; set; }
        public Nullable<int> OrderType { get; set; }
        public Nullable<bool> AutoClassification { get; set; }
        public Nullable<int> SupplyType { get; set; }
        public string ItemNmbr { get; set; }
        public Nullable<bool> IsUserDefinedUPC { get; set; }
        public Nullable<bool> IsUserDefinedUPCActive { get; set; }
        public Nullable<int> SuggestedMin { get; set; }
        public Nullable<int> SuggestedMax { get; set; }
        public int AdjustedMinMaxBy { get; set; }
        public Nullable<System.DateTime> LastMinMaxUpdateDateTime { get; set; }
        public Nullable<System.DateTime> LastSoldDateTime { get; set; }
        public Nullable<decimal> EstimatedWeeklySalesUnits { get; set; }
        public bool HideProductDetailsDialog { get; set; }
        public Nullable<int> ReservedProductID { get; set; }
        public Nullable<short> AgeVerification { get; set; }
        public bool IsWeb { get; set; }
        public decimal WebPrice { get; set; }
        public bool HasSerialTrack { get; set; }
        public bool HasFirearmInventory { get; set; }
    
        public virtual Department Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventoryGroupProduct> InventoryGroupProducts { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductStockAdjustment> ProductStockAdjustments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FirearmInventory> FirearmInventories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SerialUPC> SerialUPCs { get; set; }
    }
}
