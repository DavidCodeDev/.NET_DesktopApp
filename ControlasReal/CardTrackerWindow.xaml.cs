using ControlasReal;
using System;
using System.Data.SqlServerCe;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ControlasReal
{
    public partial class CardTrackerWindow : Window
    {
        private MifareReader mifareReader;
        public string connectionString = "Data Source=C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw";

        public CardTrackerWindow()
        {
            InitializeComponent();
            mifareReader = new MifareReader();
            this.Loaded += CardTrackerWindow_Loaded;
            this.Closing += CardTrackerWindow_Closing;
            mifareReader.CardUIDRead += OnCardUIDRead; // Suscribirse al evento de lectura de UID
        }

        private void CardTrackerWindow_Loaded(object sender, RoutedEventArgs e)
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

        private void CardTrackerWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mifareReader.Disconnect();
        }

        private void OnCardUIDRead(string uid)
        {
            Dispatcher.Invoke(() =>
            {
                UidReceived.Text = uid;
            });
        }

        public (string nombre, string apellido1, string apellido2, string phone, string email) ObtenerDetallesUsuario(int userId)
        {
            string nombre = string.Empty, apellido1 = string.Empty, apellido2 = string.Empty, phone = string.Empty, email = string.Empty;

            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCeCommand command = new SqlCeCommand("SELECT nombre, apellido1, apellido2, phone, email FROM users WHERE id = @UserId", connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);

                        using (SqlCeDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nombre = reader.GetString(0);
                                apellido1 = reader.GetString(1);
                                apellido2 = reader.GetString(2);
                                phone = reader.GetString(3);
                                email = reader.GetString(4);
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

            return (nombre, apellido1, apellido2, phone, email);
        }

        public int ObtenerUserIdPorUid(string uid)
        {
            int userId = -1; // Variable para almacenar el ID del usuario
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCeCommand command = new SqlCeCommand("SELECT user_id FROM cards WHERE uid = @Uid", connection))
                    {
                        command.Parameters.AddWithValue("@Uid", uid);

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

        private void Rastrear_Click(object sender, RoutedEventArgs e)
        {
            string uid = UidReceived.Text; // Asume que tienes un TextBox con el UID
            int userId = ObtenerUserIdPorUid(uid);

            if (userId != -1)
            {
                var detallesUsuario = ObtenerDetallesUsuario(userId);

                string mensaje = $"Nombre: {detallesUsuario.nombre}\n" +
                                 $"Apellido 1: {detallesUsuario.apellido1}\n" +
                                 $"Apellido 2: {detallesUsuario.apellido2}\n" +
                                 $"Teléfono: {detallesUsuario.phone}\n" +
                                 $"Email: {detallesUsuario.email}";

                MessageBox.Show(mensaje, "Detalles del Usuario");
            }
            else
            {
                MessageBox.Show("No se encontró ningún user_id para el UID proporcionado.");
            }
        }
    }
}
