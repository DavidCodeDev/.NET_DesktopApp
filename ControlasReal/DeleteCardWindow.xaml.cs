using System;
using System.Data.SqlServerCe;
using System.Windows;
using System.Windows.Controls;

namespace ControlasReal
{
    public partial class DeleteCardWindow : Window
    {
        public string connectionString = "Data Source=C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw";

        public DeleteCardWindow()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCeCommand command = new SqlCeCommand("SELECT email FROM users;", connection))
                    {
                        using (SqlCeDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string emailUser = reader.GetString(0);
                                cbUsuarios.Items.Add(emailUser);
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
        }
        private int ObtenerIdEmail(string email)
        {
            int userId = -1; // Variable para almacenar el ID del usuario
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCeCommand command = new SqlCeCommand("SELECT id FROM users WHERE email = @Email", connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);

                        using (SqlCeDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                userId = reader.GetInt32(0);
                            }
                        }
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
            return userId;
        }

        private void EliminarTarjeta_Click(object sender, RoutedEventArgs e)
        {
            string email = cbUsuarios.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Por favor, selecciona un usuario.");
                return;
            }

            int userId = ObtenerIdEmail(email);

            if (userId != -1)
            {
                MessageBoxResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar esta tarjeta?", "Confirmación", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (SqlCeConnection connection = new SqlCeConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            using (SqlCeCommand command = new SqlCeCommand("DELETE FROM cards WHERE user_id = @UserId", connection))
                            {
                                command.Parameters.AddWithValue("@UserId", userId);
                                command.ExecuteNonQuery();
                            }
                            connection.Close();
                            MessageBox.Show("Tarjeta eliminada correctamente.");
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.ToString());
                            MessageBox.Show($"Error al eliminar la tarjeta: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Usuario no encontrado.");
            }
        }
    }
}
