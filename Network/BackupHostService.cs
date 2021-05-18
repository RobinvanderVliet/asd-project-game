using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{

    public class BackupHostService : IBackupHostService
    {
        private Boolean _isBackupHost;

        // //database shit
        // private Database database;
        //
        public BackupHostService() 
        {
            _isBackupHost = false;
        }
        //
        // public void UpdateBackupDatabase(DatabaseModel data)
        // {
        //     try
        //     {
        //         if (CheckExists(data))
        //         {
        //             //todo update database
        //         }
        //         else
        //         {
        //             AddToBackupDatabase(data);
        //         }
        //
        //         //save in database
        //     }
        //     catch (Exception)
        //     {
        //         Console.WriteLine("Some data did not save succesfull, please contact your host!");
        //     }
        //
        // }
        //
        // private void AddToBackupDatabase(DatabaseModel data) {
        // //todo call add function
        // }
        //
        // private Boolean CheckExists(DatabaseModel data)
        // {
        //     return false; //todo
        // }
        //

        public void enableBackupHost()
        {
            _isBackupHost = true;
        }

        public Boolean isBackupHost()
        {
            return _isBackupHost;
        }
    }
}
