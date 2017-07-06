using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RestWCFServiceLibrary.MethodObject
{
    [DataContract]
    public class UpdateStatus
    {
        [DataMember]
        public Boolean IsSuccesfull { get; set; }
    }
}
