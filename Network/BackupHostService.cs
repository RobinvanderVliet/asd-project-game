using System;

namespace Network
{

    public class BackupHostService : IBackupHostService
    {
        private bool _isBackupHost;
        
        public BackupHostService() 
        {
            _isBackupHost = false;
        }

        public void EnableBackupHost()
        {
            _isBackupHost = true;
        }
        
        public void DisableBackupHost()
        {
            _isBackupHost = false;
        }

        public bool IsBackupHost()
        {
            return _isBackupHost;
        }
    }
}
