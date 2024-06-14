using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data.SqlServerCe;
using ControlasReal;
using System.Data;
using static ControlasReal.UserContainerSeeker;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.VisualBasic.ApplicationServices;

namespace ControlasReal
{
    /// <summary>
    /// Lógica de interacción para UserContainerSeeker.xaml
    /// </summary>
    public partial class UserContainerSeeker : UserControl
    {
        private User usuarioSeleccionado;

        private MifareReader _mifareReader;
        public ObservableCollection<User> Users {  get; set; } = new ObservableCollection<User>();
        public UserContainerSeeker()
        {
            InitializeComponent();
            DataContext = this;
            TablaImpresor();
        }
        
        public void TablaImpresor()
        {
            using (SqlCeConnection conn = new SqlCeConnection("Data Source = C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw"))
            {
                conn.Open();
                using (SqlCeCommand cmd = new SqlCeCommand("SELECT id, nombre, apellido1, apellido2, email, phone, permisos FROM users;", conn))
                {
                    SqlCeDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Users.Add(new User
                        {
                            id = reader.GetInt32(0),
                            nombre = reader.GetString(1),
                            apellido1 = reader.GetString(2),
                            apellido2 = reader.GetString(3),
                            email = reader.GetString(4),
                            phone = reader.GetString(5),
                            permisos = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
                        });
                    }
                }
                conn.Close();
            } 
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) // User search box for several parametres
        {
            string searchTerm = SearchTextBox.Text.ToLower();
            ICollectionView view = CollectionViewSource.GetDefaultView(Users);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                view.Filter = item =>
                {
                    if (item is User user)
                    {
                        return user.nombre.ToLower().Contains(searchTerm) ||
                        user.apellido1.ToLower().Contains(searchTerm) ||
                        user.apellido2.ToLower().Contains(searchTerm) ||
                        user.email.ToLower().Contains(searchTerm) ||
                        user.phone.ToLower().Contains(searchTerm) ||
                        user.permisos.ToLower().Contains(searchTerm);
                    }
                    return false;
                };
            }
            else
            {
                view.Filter = null;
            }
        }
        private void addUser_Click(object sender, System.Windows.RoutedEventArgs e) //apartado del menú usuario donde podemos añadir users
        {
            MainContent.Content = new RegisterUserModalWindow();
        }

        private void dobleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) // double click to select users to management it
        {

            var grid = sender as DataGrid;
            usuarioSeleccionado = grid.SelectedItem as User;

            // Abre la ventana modal y pasa el usuario seleccionado
            abrirVentanaModal(usuarioSeleccionado);
        }
        private void abrirVentanaModal(User usuario)
        {
            var ventanaModal = new EditUserModalWindow(usuario);

            ventanaModal.nombre.Text = usuario.nombre;
            ventanaModal.primer_apellido.Text = usuario.apellido1;
            ventanaModal.segundo_apellido.Text = usuario.apellido2;
            ventanaModal.email.Text = usuario.email;
            ventanaModal.telefono.Text = usuario.phone;
            ventanaModal.permisos.Text = usuario.permisos;
            ventanaModal.password.Password = usuario.password; 
            ventanaModal.comprobarpassword.Password = usuario.password;

            ventanaModal.ShowDialog();
        }
        

    }
}