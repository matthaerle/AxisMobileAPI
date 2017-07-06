using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.ObjectViews
{
    [DataContract]
    public class ProductView
    {
        [DataMember]
        public Int64 ProductID { get; set; }
        [DataMember]
        public string ProductUPC { get; set; }
        [DataMember]
        public int MinLevel { get; set; }
        [DataMember]
        public int MaxLevel { get; set; }
        [DataMember]
        public string ItemNmbr { get; set; }
        [DataMember]
        public decimal Price { get; set; }
        [DataMember]
        public int PhysicalQoH { get; set; }
        [DataMember]
        public int QtyOnOrder { get; set; }
        [DataMember]
        public int QtyCommitted { get; set; }
        [DataMember]
        public string Manufacturer { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string ShortDescription { get; set; }
        [DataMember]
        public bool IsSerialNonFirearm { get; set; }
        [DataMember]
        public bool IsStock { get; set; }
        [DataMember]
        public bool IsFirearm { get; set; }
        [DataMember]
        public bool Active { get; set; }
    }
}
