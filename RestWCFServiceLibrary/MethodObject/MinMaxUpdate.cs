using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class MinMaxUpdate
    {
        [DataMember]
        public Int64 ProductID { get; set; }
        [DataMember]
        public int MinLevel { get; set; }
        [DataMember]
        public int MaxLevel { get; set; }
        [DataMember]
        public int EmployeeID { get; set; }
    }
}
