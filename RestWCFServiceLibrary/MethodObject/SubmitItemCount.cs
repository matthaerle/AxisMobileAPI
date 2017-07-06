using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class SubmitItemCount
    {
        [DataMember]
        public string ProductUPC { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public int CountQty { get; set; }
        [DataMember]
        public int GroupID { get; set; }
    }
}
