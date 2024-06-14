using Microsoft.VisualBasic.ApplicationServices;
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
    /// Lógica de interacción para EditUserModalWindow.xaml
    /// </summary>
    public partial class EditUserModalWindow : Window
    {
        private User usuarioSeleccionado;

        public EditUserModalWindow(User usuarioSeleccionado) // filtrar usuario y poder editar informacion y/o borrarlo
        {
            InitializeComponent();
            selectUser(null, null);
            cbUsuarios.Items.Add(usuarioSeleccionado.email);
            cbUsuarios.SelectedItem = usuarioSeleccionado.email;
            nombre.Text = usuarioSeleccionado.nombre;
            primer_apellido.Text = usuarioSeleccionado.apellido1;
            segundo_apellido.Text = usuarioSeleccionado.apellido2;
            email.Text = usuarioSeleccionado.email;
            telefono.Text = usuarioSeleccionado.phone;
            permisos.Text = usuarioSeleccionado.permisos;
        }
       
        public string connectionString = "Data Source = C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw";
        public void OpenConnection() // conexion a la base de datos
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
        }
        private void selectUser(object sender, RoutedEventArgs e) // desplegable donde seleccionamos usuarios en el modificador
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
        private void Guardar_Click(object sender, RoutedEventArgs e) // guardamos la nueva información
        {
            if (cbUsuarios.SelectedItem != null)
            {
            string selectedUser = cbUsuarios.SelectedItem.ToString();
            string nombre = this.nombre.Text;
            string primer_apellido = this.primer_apellido.Text;
            string segundo_apellido = this.segundo_apellido.Text;
            string email = this.email.Text;
            string telefono = this.telefono.Text;
            string permisos = this.permisos.Text;
            string password = this.password.Password;
            string confirmarpassword = this.comprobarpassword.Password;

            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCeCommand command = new SqlCeCommand($"UPDATE users SET nombre = '{nombre}', apellido1 = '{primer_apellido}', apellido2 = '{segundo_apellido}', email = '{email}', phone = '{telefono}', permisos = '{permisos}', password = '{password}' WHERE email = '{selectedUser}';", connection))
                    {
                        if (password != confirmarpassword)
                        {
                            MessageBox.Show("La contraseña debe coincidir");
                            this.password.Password = "";
                            this.comprobarpassword.Password = "";
                        }
                        else
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Usuario editado");
                            cbUsuarios.SelectedItem = null;
                            this.nombre.Text = "";
                            this.primer_apellido.Text = "";
                            this.segundo_apellido.Text = "";
                            this.email.Text = "";
                            this.telefono.Text = "";
                            this.permisos.Text = "";
                            this.password.Password = "";
                            this.comprobarpassword.Password = "";
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
    }
        private void Tarjetas_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Acerque tarjeta al lector. Mantener en el lector hasta acabar proceso.", "Confirmación", MessageBoxButton.OK);

                AddCard window4 = new AddCard(usuarioSeleccionado);
                window4.ShowDialog();
            
        }
        private void Borrar_Click(object sender, RoutedEventArgs e) // button to delete selected users
        {
            MessageBoxResult result = MessageBox.Show("¿Estás seguro de que quieres eliminar este usuario?", "Confirmación", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string selectedUser = cbUsuarios.SelectedItem.ToString();

                using (SqlCeConnection connection = new SqlCeConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        using (SqlCeCommand command = new SqlCeCommand($"DELETE FROM users WHERE email = '{selectedUser}'", connection))
                        {

                            command.ExecuteNonQuery();
                            MessageBox.Show("Usuario eliminado con éxito");
                            cbUsuarios.SelectedItem = null;
                        }
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
                }
                selectUser(null, null);
            }
        }

    }
}
