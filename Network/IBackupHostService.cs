using System;

namespace Network
{
    public interface IBackupHostService
    {
        public void EnableBackupHost();
        
        public void DisableBackupHost();
        
        public Boolean IsBackupHost();
    }
}