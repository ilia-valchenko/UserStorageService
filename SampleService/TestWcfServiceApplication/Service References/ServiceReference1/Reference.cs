﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestWcfServiceApplication.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/TestConnection", ReplyAction="http://tempuri.org/IService/TestConnectionResponse")]
        string TestConnection();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/TestConnection", ReplyAction="http://tempuri.org/IService/TestConnectionResponse")]
        System.Threading.Tasks.Task<string> TestConnectionAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/Add", ReplyAction="http://tempuri.org/IService/AddResponse")]
        void Add(UserLibrary.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/Add", ReplyAction="http://tempuri.org/IService/AddResponse")]
        System.Threading.Tasks.Task AddAsync(UserLibrary.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/Delete", ReplyAction="http://tempuri.org/IService/DeleteResponse")]
        void Delete(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/Delete", ReplyAction="http://tempuri.org/IService/DeleteResponse")]
        System.Threading.Tasks.Task DeleteAsync(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetUserByPredicate", ReplyAction="http://tempuri.org/IService/GetUserByPredicateResponse")]
        UserLibrary.User GetUserByPredicate(System.Func<UserLibrary.User, bool> criteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetUserByPredicate", ReplyAction="http://tempuri.org/IService/GetUserByPredicateResponse")]
        System.Threading.Tasks.Task<UserLibrary.User> GetUserByPredicateAsync(System.Func<UserLibrary.User, bool> criteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetUsersByPredicate", ReplyAction="http://tempuri.org/IService/GetUsersByPredicateResponse")]
        UserLibrary.User[] GetUsersByPredicate(System.Func<UserLibrary.User, bool> criteria);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetUsersByPredicate", ReplyAction="http://tempuri.org/IService/GetUsersByPredicateResponse")]
        System.Threading.Tasks.Task<UserLibrary.User[]> GetUsersByPredicateAsync(System.Func<UserLibrary.User, bool> criteria);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : TestWcfServiceApplication.ServiceReference1.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<TestWcfServiceApplication.ServiceReference1.IService>, TestWcfServiceApplication.ServiceReference1.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string TestConnection() {
            return base.Channel.TestConnection();
        }
        
        public System.Threading.Tasks.Task<string> TestConnectionAsync() {
            return base.Channel.TestConnectionAsync();
        }
        
        public void Add(UserLibrary.User user) {
            base.Channel.Add(user);
        }
        
        public System.Threading.Tasks.Task AddAsync(UserLibrary.User user) {
            return base.Channel.AddAsync(user);
        }
        
        public void Delete(int userId) {
            base.Channel.Delete(userId);
        }
        
        public System.Threading.Tasks.Task DeleteAsync(int userId) {
            return base.Channel.DeleteAsync(userId);
        }
        
        public UserLibrary.User GetUserByPredicate(System.Func<UserLibrary.User, bool> criteria) {
            return base.Channel.GetUserByPredicate(criteria);
        }
        
        public System.Threading.Tasks.Task<UserLibrary.User> GetUserByPredicateAsync(System.Func<UserLibrary.User, bool> criteria) {
            return base.Channel.GetUserByPredicateAsync(criteria);
        }
        
        public UserLibrary.User[] GetUsersByPredicate(System.Func<UserLibrary.User, bool> criteria) {
            return base.Channel.GetUsersByPredicate(criteria);
        }
        
        public System.Threading.Tasks.Task<UserLibrary.User[]> GetUsersByPredicateAsync(System.Func<UserLibrary.User, bool> criteria) {
            return base.Channel.GetUsersByPredicateAsync(criteria);
        }
    }
}
