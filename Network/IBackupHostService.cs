using System;

namespace Network
{
    public interface IBackupHostService
    {
        public void enableBackupHost();
        public Boolean isBackupHost();
    }
}