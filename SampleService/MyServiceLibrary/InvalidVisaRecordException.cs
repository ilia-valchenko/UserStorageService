using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyServiceLibrary
{
    /// <summary>
    /// This exception will be thrown if the client would attempt to create an incorrect visa record.
    /// </summary>
    public class InvalidVisaRecordException : Exception {}
}
