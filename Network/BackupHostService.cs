using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Network
{
    public class BackupHostService : IBackupHostService
    {
        private bool _isBackupHost;
        private DbConnection _db { get; set; }

        public BackupHostService() 
        {
            _isBackupHost = false;
            _db = new();
        }

        public virtual void UpdateBackupDatabase(PacketDTO data)
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
            }
            catch (Exception)
            {
                Console.WriteLine("Some data did not save succesfull, please contact your host!");
            }

        }

        public virtual Object ConvertDataToPoco(PacketDTO packet)
        {
            //convert the payload to poco for inserting
            switch (packet.Header.PacketType)
            {
                //todo change to proper enums
                case PacketType.Move:
                    return JsonConvert.DeserializeAnonymousType(packet.Payload, new { PocoType = "PlayerPoco", PlayerGUID = "", GameGUID = "", X = "", Y = "" });
                case PacketType.Session:
                    return JsonConvert.DeserializeAnonymousType(packet.Payload, new { PocoType = "GamePoco", PlayerHostGUID = "", Seed = ""});
                default:
                    throw new UnknownEnumException("an unhandled action type is recieved");
            }
        }

        public virtual void AlterBackupDatabase(Object data, ILiteDatabaseAsync conn)
        {
            var collection = conn.GetCollection < Type.GetType(data.PocoType) >("");
            collection.Update(data);
        }

        public virtual void AddToBackupDatabase(Object data, ILiteDatabaseAsync conn) {
            var collection = conn.GetCollection < Type.GetType(data.PocoType) > ("");
            collection.Insert(data);
        }

        public virtual bool CheckExists(Object data)
        {
            var connection = _db.GetConnectionAsync();
            var collection = connection.GetCollection<Type.GetType(data.PocoType)>("");
            var exists = collection.Where().FirstOrDefault() != null;
            _db.Close();
            return exists;
        }

        public virtual void EnableBackupHost()
        {
            _isBackupHost = true;
        }

        public virtual bool IsBackupHost()
        {
            return _isBackupHost;
        }

        public virtual void DisableBackupHost()
        {
            _isBackupHost = false;
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
