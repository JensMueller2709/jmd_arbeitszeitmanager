namespace JMD_Arbeitszeitmanager.Contracts.Services
{
    public interface IPersistAndRestoreService
    {
        void RestoreData();

        void PersistData();
    }
}
