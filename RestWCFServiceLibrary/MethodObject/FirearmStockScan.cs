using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class FirearmStockScan
    {
        [DataMember]
        public Int64 Log { get; set; }
        [DataMember]
        public string SerialNumber { get; set; }
        [DataMember]
        public bool SerialScanned { get; set; }
        [DataMember]
        public bool LogScanned { get; set; }
        [DataMember]
        public string BoundBookType { get; set; }
    }
}
