using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using UserLibrary;

namespace WcfService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string TestConnection();

        [OperationContract]
        void Add(User user);

        [OperationContract]
        void Delete(int userId);

        [OperationContract]
        User GetUserByPredicate(Func<User, bool> criteria);

        [OperationContract]
        IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}
}
