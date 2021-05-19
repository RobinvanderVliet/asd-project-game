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

        //database shit

        public BackupHostService() 
        {
            _isBackupHost = false;
        }

        public void UpdateBackupDatabase(PacketDTO data)
        {
            try
            {
                //convert packet to poco
                var poco = ConvertDataToPoco(data);

                if (CheckExists(poco))
                {
                    AlterBackupDatabase(poco);
                }
                else
                {
                    AddToBackupDatabase(poco);
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
                    var definition = new { PlayerGUID = "", GameGUID="" , X = "", Y="" };
                    return JsonConvert.DeserializeAnonymousType(packet.Payload, definition);
                default:
                    throw new UnknownEnumException("an unhandled action type is recieved");
            }
        }

        private void AlterBackupDatabase(Object data)
        {
            //todo call update function
        }

        private void AddToBackupDatabase(Object data) {
        //todo call add function
        }

        private Boolean CheckExists(Object data)
        {
            return false; //todo
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
