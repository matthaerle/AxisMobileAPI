using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class FirearmInfo
    {
        [DataMember]
        public String SerialNumber { get; set; }
        [DataMember]
        public String UPC { get; set; }
        [DataMember]
        public String Manufacturer { get; set; }
        [DataMember]
        public String Model { get; set; }
        [DataMember]
        public String GaugeCaliber { get; set; }
        [DataMember]
        public String TypeOfAction { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public String NewUsed { get; set; }
        [DataMember]
        public Int64? InvNbr { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public int InventoryNumber { get; set; }
        [DataMember]
        public String Importer { get; set; }
    }
}
