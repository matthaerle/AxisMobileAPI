using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class GetEmployees
    {
        [DataMember]
        public int EmployeeID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string LastName { get; set; } 
        [DataMember]
        public string EmployeeNumber { get; set; }
        [DataMember]
        public string PasswordEncrypted { get; set; }
        [DataMember]
        public string PasswordHashed { get; set; }
    }
}
