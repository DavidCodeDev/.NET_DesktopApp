using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ControlasReal
{
    /// <summary>
    /// Lógica de interacción para Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }
        private void RegisterButton_Click(object sender, RoutedEventArgs e) // Register form for new users
        {
            string username = usernameText.Text;
            string firstName = FirstName.Text;
            string secondSurname = SecondSurname.Text;
            string email = Email.Text;
            string phoneNumber = PhoneNumber.Text;
            string password = Password.Password;
            string permisos = Permisos.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(secondSurname) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(permisos))
            {
                MessageBox.Show("Por favor, complete todos los campos.");
                return;
            }
            try
            {
                string connectionString = "Data Source = C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw";

                using (SqlCeConnection connection = new SqlCeConnection(connectionString))
                {
                    connection.Open();

                    string checkUserQuery = "SELECT COUNT(*) FROM users WHERE email = @email";
                    using (SqlCeCommand checkUserCommand = new SqlCeCommand(checkUserQuery, connection))
                    {
                        checkUserCommand.Parameters.AddWithValue("@email", email);
                        int userCount = (int)checkUserCommand.ExecuteScalar();

                        if (userCount > 0)
                        {
                            MessageBox.Show("El usuario ya existe.");
                            return;
                        }
                    }
                    string insertUserQuery = "INSERT INTO users (nombre, apellido1, apellido2, email, phone, permisos, password) VALUES (@nombre, @apellido1, @apellido2, @email, @phone, @permisos, @password)";
                    using (SqlCeCommand insertUserCommand = new SqlCeCommand(insertUserQuery, connection))
                    {
                        insertUserCommand.Parameters.AddWithValue("@nombre", username);
                        insertUserCommand.Parameters.AddWithValue("@apellido1", firstName);
                        insertUserCommand.Parameters.AddWithValue("@apellido2", secondSurname);
                        insertUserCommand.Parameters.AddWithValue("@email", email);
                        insertUserCommand.Parameters.AddWithValue("@phone", phoneNumber);
                        insertUserCommand.Parameters.AddWithValue("@permisos", permisos);
                        insertUserCommand.Parameters.AddWithValue("@password", password);

                        insertUserCommand.ExecuteNonQuery();
                    }
                    connection.Close();
                }
                MessageBox.Show("Usuario registrado exitosamente.");
                usernameText.Text = "";
                FirstName.Text = "";
                SecondSurname.Text = "";
                Email.Text = "";
                PhoneNumber.Text = "";
                Password.Password = "";
                Permisos.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al registrar el usuario: " + ex.Message);
            }
        }
    }
}
