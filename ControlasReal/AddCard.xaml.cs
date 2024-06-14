using ControlasReal;
using System;
using System.Data.SqlServerCe;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlasReal
{
    public partial class AddCard : Window
    {
        private MifareReader mifareReader;
        private User usuarioSeleccionado;
        public string connectionString = "Data Source=C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw";

        public AddCard(User usuarioSeleccionado)
        {
            InitializeComponent();
            this.usuarioSeleccionado = usuarioSeleccionado;
            mifareReader = new MifareReader();
            selectUser(null, null);
            this.Loaded += AddCard_Loaded;
            this.Closing += AddCard_Closing;
            mifareReader.CardUIDRead += OnCardUIDRead; // Suscribirse al evento de lectura de UID
        }

        private void AddCard_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    mifareReader.InitializeReader(); // Inicializar el lector
                    mifareReader.StartMonitoring();  // Comenzar a monitorear después de la inicialización
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => MessageBox.Show($"Error initializing reader: {ex.Message}"));
                }
            });
        }

        private void AddCard_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mifareReader.Disconnect();
        }

        private void selectUser(object sender, RoutedEventArgs e)
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
                            cbUsuarios.Items.Add(""); // Añadir un primer elemento vacío
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

        private void OnCardUIDRead(string uid)
        {
            Dispatcher.Invoke(() =>
            {
                UidReceived.Text = uid;
            });
        }

        public int ObtenerIdEmail(string email)
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

        private bool UserHasCard(int userId)
        {
            bool hasCard = false;
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCeCommand command = new SqlCeCommand("SELECT COUNT(*) FROM cards WHERE user_id = @UserId", connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        hasCard = (int)command.ExecuteScalar() > 0;
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
            return hasCard;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            string email = null;

            if (cbUsuarios.SelectedItem != null && !string.IsNullOrEmpty(cbUsuarios.SelectedItem.ToString()))
            {
                email = cbUsuarios.SelectedItem.ToString();
            }
            else if (!string.IsNullOrEmpty(this.email.Text))
            {
                email = this.email.Text;
            }

            string uid = UidReceived.Text;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(uid))
            {
                int userId = ObtenerIdEmail(email);

                if (userId != -1)
                {
                    if (!UserHasCard(userId))
                    {
                        using (SqlCeConnection connection = new SqlCeConnection(connectionString))
                        {
                            try
                            {
                                connection.Open();
                                using (SqlCeCommand command = new SqlCeCommand("INSERT INTO cards(user_id, uid, state) VALUES (@UserId, @Uid, @State)", connection))
                                {
                                    command.Parameters.AddWithValue("@UserId", userId);
                                    command.Parameters.AddWithValue("@Uid", uid);
                                    command.Parameters.AddWithValue("@State", "Activa");

                                    command.ExecuteNonQuery();
                                }
                                connection.Close();
                                MessageBox.Show("Guardado correctamente");
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.ToString());
                                MessageBox.Show($"Error al guardar: tarjeta asignada a otro usuario");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Este usuario ya tiene una tarjeta asignada.");
                    }
                }
                else
                {
                    MessageBox.Show("Id incorrecto");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecciona un usuario o ingresa un correo electrónico y asegúrate de que el UID no esté vacío.");
            }
        }
    }
}
