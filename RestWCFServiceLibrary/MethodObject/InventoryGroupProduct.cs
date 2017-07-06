using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class InventoryGroupProduct
    {
        [DataMember]
        public Int64 InventoryGroupProductID { get; set; }
        [DataMember]
        public int InventoryGroupID { get; set; }
        [DataMember]
        public Int64 ProductID { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public int QtyScanned { get; set; }
    }
}
