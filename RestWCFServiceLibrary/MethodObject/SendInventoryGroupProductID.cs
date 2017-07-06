using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class SendInventoryGroupProductID
    {
        [DataMember]
        public Int64 ProductID { get; set; }
        [DataMember]
        public Int64 InventoryGroupProductID { get; set; }
        [DataMember]
        public int GroupID { get; set; }
    }
}
