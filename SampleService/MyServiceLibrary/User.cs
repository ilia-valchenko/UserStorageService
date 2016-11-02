using System;
using System.Diagnostics;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represents a simple user which contains information about it's first name, last name and a date of birth.
    /// </summary>
    public class User : IEquatable<User>, IComparable<User>
    {
        public int Id {
            get
            {
                return id;
            }
            private set
            {
                if (id < 0)
                    throw new ArgumentException(nameof(id));

                id = value;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (value == String.Empty)
                    throw new ArgumentException(nameof(value));

                firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }
            private set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (value == String.Empty)
                    throw new ArgumentException(nameof(value));

                lastName = value;
            }
        }

        public DateTime DateOfBirth
        {
            get
            {
                return dateOfBirth;
            }
            private set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException(nameof(value));

                Debug.Print("Is date more than future: " + (value > DateTime.Now));

                dateOfBirth = value;
            }
        }

        public User() { }

        public User(string firstname, string lastname, DateTime dateOfBirth)
        {      
            FirstName = firstname;
            LastName = lastname;
            DateOfBirth = dateOfBirth;
        }

        public User(int id, string firstname, string lastname, DateTime dateOfBirth)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public bool Equals(User other)
        {
            if (other == null)
                return false;

            if (other.FirstName.ToLower() == this.FirstName.ToLower())
                if (other.LastName.ToLower() == this.LastName.ToLower())
                    if (other.DateOfBirth == this.DateOfBirth)
                        return true;

            return false;
        }

        public int CompareTo(User other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other.Id == this.Id)
                return 0;

            if (other.Id > this.Id)
                return -1;
            else
                return 1;
        }

        public override string ToString() => String.Format("Id: " + Id + "; First name: " + FirstName + "; Last name: " + LastName + "; Date of birth: " + DateOfBirth);

        private int id;
        private string firstName;
        private string lastName;
        private DateTime dateOfBirth;
    }
}
