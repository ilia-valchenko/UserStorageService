using System;
using System.Collections.Generic;

namespace MyServiceLibrary
{
    /// <summary>
    /// This class represents a simple user which contains information about it's first name, last name and a date of birth.
    /// </summary>
    public class User : IEquatable<User>, IComparable<User>
    {
        #region Public properties
        /// <summary>
        /// User's id property.
        /// </summary>
        public int Id
        {
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

        /// <summary>
        /// User's first name property.
        /// </summary>
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

        /// <summary>
        /// User's last name property.
        /// </summary>
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

        /// <summary>
        /// User's gender.
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// User's date of birth property.
        /// </summary>
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

                dateOfBirth = value;
            }
        }

        /// <summary>
        /// User's visa records.
        /// </summary>
        public IEnumerable<VisaRecord> VisaRecords
        {
            get
            {
                return visaRecords;
            }
            set
            {
                if (value == null)
                    throw new InvalidUserException();

                visaRecords = value;
            }
        } 
        #endregion

        /// <summary>
        /// Default constructor which create a default user.
        /// </summary>
        public User() : this("unknown", "unknown", Gender.Unknown, DateTime.Now, new List<VisaRecord>()) { }

        /// <summary>
        /// Constructor that creates a user by using input parameters.
        /// </summary>
        /// <param name="firstname">The first name of a future user.</param>
        /// <param name="lastname">The last name of a future user.</param>
        /// <param name="gender">The gender of a future user.</param>
        /// <param name="dateOfBirth">The date of birth of a future user.</param>
        /// <param name="visaRecords">User's collection of visa records.</param>
        /// <param name="id">The id of a future user.</param>
        public User(string firstname, string lastname, Gender gender, DateTime dateOfBirth, IEnumerable<VisaRecord> visaRecords, int id = 0)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Gender = gender;
            DateOfBirth = dateOfBirth;
            VisaRecords = visaRecords;
        }

        /// <summary>
        /// This method determines if the current user is equals to another one.
        /// </summary>
        /// <param name="other">Another user.</param>
        /// <returns>Returns true if they are equal.</returns>
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

        /// <summary>
        /// This method compares the current user to another one.
        /// </summary>
        /// <param name="other">Another user.</param>
        /// <returns>Returns 0 if users are equal. Returns -1 if the current user is less than another. Returns 1 if the current user is greater than given another.</returns>
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

        /// <summary>
        /// This method returns string representation of the current user.
        /// </summary>
        /// <returns>String that represent the current user.</returns>
        public override string ToString() => String.Format("Id: " + Id + "; First name: " + FirstName + "; Last name: " + LastName + "; Date of birth: " + DateOfBirth);

        #region Private fields
        /// <summary>
        /// User's Id.
        /// </summary>
        private int id;

        /// <summary>
        /// User's first name.
        /// </summary>
        private string firstName;

        /// <summary>
        /// User's last name.
        /// </summary>
        private string lastName;

        /// <summary>
        /// User's date of birth.
        /// </summary>
        private DateTime dateOfBirth;

        /// <summary>
        /// The list of the user's visa records.
        /// </summary>
        private IEnumerable<VisaRecord> visaRecords; 
        #endregion
    }

    /// <summary>
    /// Represents a gender of a user.
    /// </summary>
    public enum Gender
    {
        Male,
        Female,
        Unknown
    }

    /// <summary>
    /// This structure represetns a visa record. Record contains information country which was visited by user and visit date.
    /// </summary>
    public struct VisaRecord
    {
        #region Public properties
        /// <summary>
        /// Country's property
        /// </summary>
        public string Country
        {
            get
            {
                return country;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new InvalidVisaRecordException();

                country = value;
            }
        }

        /// <summary>
        /// Start's property
        /// </summary>
        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                if (value > DateTime.Now)
                    throw new InvalidVisaRecordException();

                start = value;
            }
        }

        /// <summary>
        /// Start's property
        /// </summary>
        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                if (value > DateTime.Now)
                    throw new InvalidVisaRecordException();

                end = value;
            }
        } 
        #endregion

        /// <summary>
        /// Custom constructor for visa record.
        /// </summary>
        /// <param name="country">The country which was visited.</param>
        /// <param name="start">The date of arrival.</param>
        /// <param name="end">The date of departure.</param>
        public VisaRecord(string country, DateTime start, DateTime end)
        {
            this.country = default(String);
            this.start = default(DateTime);
            this.end = default(DateTime);

            Country = country;
            Start = start;
            End = end;
        }

        #region Private fields
        /// <summary>
        /// The country which was visited by user.
        /// </summary>
        private string country;

        /// <summary>
        /// The date when the user has arrived in the country. 
        /// </summary>
        private DateTime start;

        /// <summary>
        /// The date when the user left the country
        /// </summary>
        private DateTime end; 
        #endregion
    }
}
