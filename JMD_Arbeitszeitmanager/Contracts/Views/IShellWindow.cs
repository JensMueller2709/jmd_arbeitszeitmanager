using System.Windows.Controls;

namespace JMD_Arbeitszeitmanager.Contracts.Views
{
    public interface IShellWindow
    {
        Frame GetNavigationFrame();

        void ShowWindow();

        void CloseWindow();
    }
}
