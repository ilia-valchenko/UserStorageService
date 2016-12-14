using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NotificationMessagesLibrary
{
    [Serializable]
    public class NotificationMessage : MarshalByRefObject
    {
        public Commands Command { get; set; }
        //public object User { get; private set; }

        public NotificationMessage(Commands command, object user)
        {
            Command = command;
            //User = user;
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
                return null;

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);

            return obj;
        }
    }

    [Serializable]
    public enum Commands
    {
        Add,
        Delete,
        Find,
        Stop
    }
}
