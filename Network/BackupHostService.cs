using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Network
{
    public interface IBackupHostService {

        public void UpdateBackupDatabase(PacketDTO data);
        public void EnableBackupHost();
        public Boolean IsBackupHost();
    }

    public class BackupHostService : IBackupHostService
    {
        private Boolean _isBackupHost;
        private DbConnection _db;

        public BackupHostService() 
        {
            _isBackupHost = false;
            _db = new();
        }

        public void UpdateBackupDatabase(PacketDTO data)
        {
            try
            {
                var poco = ConvertDataToPoco(data);
                
                if (CheckExists(poco))
                {
                    var connection = _db.GetConnectionAsync();
                    AlterBackupDatabase(poco, connection);
                    connection.Close();
                }
                else
                {
                    var connection = _db.GetConnectionAsync();
                    AddToBackupDatabase(poco, connection);
                    connection.Close();
                }

                //save in database
            }
            catch (Exception)
            {
                Console.WriteLine("Some data did not save succesfull, please contact your host!");
            }

        }

        private Object ConvertDataToPoco(PacketDTO packet)
        {
            //convert the payload to poco for inserting
            switch (packet.Header.PacketType)
            {
                //todo change to proper enums
                case PacketType.Move:
                    var definition = new { PocoType="PlayerPoco",PlayerGUID = "", GameGUID="" , X = "", Y="" };
                    return JsonConvert.DeserializeAnonymousType(packet.Payload, definition);
                default:
                    throw new UnknownEnumException("an unhandled action type is recieved");
            }
        }

        private void AlterBackupDatabase(Object data, ILiteDatabaseAsync conn)
        {
            var collection = conn.GetCollection < Type.GetType(data.PocoType) >("");
            collection.Update(data);
        }

        private void AddToBackupDatabase(Object data, ILiteDatabaseAsync conn) {
            var collection = conn.GetCollection < Type.GetType(data.PocoType) > ("");
            collection.Insert(data);
        }

        private Boolean CheckExists(Object data)
        {
            var connection = _db.GetConnectionAsync();
            var collection = connection.GetCollection<Type.GetType(data.PocoType)>("");
            var exists = collection.Where().FirstOrDefault() != null;
            _db.Close();
            return exists;
        }

        public void EnableBackupHost()
        {
            _isBackupHost = true;
        }

        public bool IsBackupHost()
        {
            return _isBackupHost;
        }
    }

    [Serializable]
    internal class UnknownEnumException : Exception
    {
        public UnknownEnumException()
        {
        }

        public UnknownEnumException(string message) : base(message)
        {
            Console.WriteLine(message);
        }
    }
}
