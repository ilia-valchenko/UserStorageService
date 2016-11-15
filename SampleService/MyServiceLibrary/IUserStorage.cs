namespace MyServiceLibrary
{
    /// <summary>
    /// This interface prodives basic operations for storage such as CRUD and seeking operations.
    /// </summary>
    public interface IUserStorage
    {
        /// <summary>
        /// This method ads the given user to the storage if it doesn't exist.
        /// </summary>
        /// <param name="user">A user which must be added to the storage.</param>
        void Add(User user);
        /// <summary>
        /// This method removes the given user from the storage if it exists.
        /// </summary>
        /// <param name="user">A user which must be removed.</param>
        void Delete(User user);
        /// <summary>
        /// This method removes a user from the storage by using user's id.
        /// </summary>
        /// <param name="userId">An Id of the user which must be removed.</param>
        void Delete(int userId);
        /// <summary>
        /// This method defines if the given user exist into the storage.
        /// </summary>
        /// <returns>Returns true if the given user exists.</returns>
        bool Contains(User user);

        //User FindByValue();
    }
}
