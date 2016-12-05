using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyServiceLibrary
{
    /// <summary>
    /// This is the class that provides communications between services.
    /// </summary>
    [Serializable]
    public abstract class NotificationMessage
    {
        /// <summary>
        /// Command.
        /// </summary>
        public Commands Command { get; private set; }

        /// <summary>
        /// Constructor that takes a command.
        /// </summary>
        /// <param name="command">Command.</param>
        protected NotificationMessage(Commands command)
        {
            Command = command;
        }

        /// <summary>
        /// This method transfroms the given array of bytes to the notification message.
        /// </summary>
        /// <param name="array">The array of bytes which represent our message.</param>
        /// <param name="offset">The start index.</param>
        /// <param name="count">The end index.</param>
        /// <returns>Returns the class derived from NotificationMessage.</returns>
        public static NotificationMessage TransfromBytesToNotificationMessage(byte[] array, int offset, int count)
        {
            if(array == null)
                throw new ArgumentNullException(nameof(array));

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                stream.Write(array, offset, count);
                stream.Seek(0, SeekOrigin.Begin);
                // return binFormatter.Deserialize(stream) as NotificationMessage;

                try
                {
                    var test = binFormatter.Deserialize(stream);
                }
                catch (Exception exc)
                {
                    Console.WriteLine("tut: " + exc.Message);
                }

                //return = binFormatter.Deserialize(stream) as NotificationMessage;

                return new StopNotificationMessage();
            }
        }

        /// <summary>
        /// This method transfroms the given notification message to an array of bytes.
        /// </summary>
        /// <param name="msg">The given notification message.</param>
        /// <returns>The array of bytes which represents our notification message.</returns>
        public static byte[] TransformMessageToBytes(NotificationMessage msg)
        {
            if (msg == null)
                throw new ArgumentNullException(nameof(msg));

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter binFormatter = new BinaryFormatter();
                binFormatter.Serialize(stream, msg);
                return stream.ToArray();
            }
        }
    }

    /// <summary>
    /// There are commands which our service will take from client.
    /// </summary>
    public enum Commands
    {
        Add,
        Delete,
        GetUserByPredicate,
        GetUsersByPredicate,
        Stop
    };
}
