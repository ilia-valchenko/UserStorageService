using System;
using System.Collections.Generic;
using UserLibrary;
using MyServiceLibrary;

namespace WcfService
{
    public class Service : IService
    {
        static Service()
        {
            master = new Master(3000, "localhost", new int[0]);
        }

        public void Add(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            master.Add(user); 
        }

        public void Delete(int userId)
        {
            if (userId < 0)
                throw new ArgumentException(nameof(userId));

            master.Delete(userId);
        }

        public User GetUserByPredicate(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            return master.GetUserByPredicate(criteria);
        }

        public IEnumerable<User> GetUsersByPredicate(Func<User, bool> criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            return master.GetUsersByPredicate(criteria);
        }

        public string TestConnection()
        {
            return "OK";
        }

        private static Master master;
    }
}
