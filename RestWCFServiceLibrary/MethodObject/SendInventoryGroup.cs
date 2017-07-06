using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class SendInventoryGroup
    {
        [DataMember]
        public int InventoryGroupID { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public int ProductCount { get; set; }
    }
}
