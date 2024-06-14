using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlServerCe;
using System.Windows.Input;

namespace ControlasReal
{
    /// <summary>
    /// Lógica de interacción para LoginPanel.xaml
    /// </summary>
    public partial class LoginPanel : UserControl
    {
        private MainWindow mainWindow;
        public event Action LoginExitoso;
        public LoginPanel(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtener los valores de los campos de texto
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            // Realizar la consulta en la base de datos
            bool usuarioExiste = ComprobarUsuario(username, password);

            if (usuarioExiste)
            {
                // Usuario válido, hacer algo (por ejemplo, abrir una nueva ventana)
                MessageBox.Show("¡Bienvenido!");
                mainWindow.UsuarioLogueado = true;
                mainWindow.ActualizarVisibilidadMenu();
                mainWindow.MainContent.Content = new WelcomeLayout();
            }
            else
            {
                // Usuario inválido, mostrar un mensaje de error
                MessageBox.Show("Usuario o contraseña incorrectos");
            }
        }

        private bool ComprobarUsuario(string nombre, string password)
        {
            // Realizar la consulta en la base de datos para comprobar si el usuario existe
            string connectionString = "Data Source = C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw";
            string query = "SELECT COUNT(*) FROM users WHERE nombre = @Username AND password = @Password";

            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                using (SqlCeCommand command = new SqlCeCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", nombre);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }
}
