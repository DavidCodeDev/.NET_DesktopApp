using System.Windows;

namespace ControlasReal
{
    public class LogoutManagement
    {
        private MainWindow mainWindow;
        public LogoutManagement(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }
        public void Logout()
        {         
            mainWindow.UsuarioLogueado = false;
            mainWindow.ActualizarVisibilidadMenu();
            mainWindow.MainContent.Content = null;
            mainWindow.MainContent.Content = new MainWindow();
        }
    }
}
