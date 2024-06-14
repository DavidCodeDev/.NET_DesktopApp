using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlServerCe;
using System.Windows.Controls;
using System.Windows.Data;

namespace ControlasReal
{
    /// <summary>
    /// Lógica de interacción para ReservationsList.xaml
    /// </summary>
    public partial class ReservationsList : UserControl
    {
        private List<Reservation> reservations;

        public ReservationsList()
        {
            InitializeComponent();
            LoadData();
            User user = new User(); 
            Reservation reservation = new Reservation();


        }

        private void LoadData()
        {

            reservations = new List<Reservation>();
            using (SqlCeConnection conn = new SqlCeConnection("Data Source=C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw"))
            {
                conn.Open();
                using (SqlCeCommand query = new SqlCeCommand("SELECT id, user_id, zone, creation_date, end_date, acceso_mode, acceso_data FROM reservations;", conn))
                {
                    SqlCeDataReader reader = query.ExecuteReader();
                    while (reader.Read())
                    {
                        reservations.Add(new Reservation
                        {
                            ID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            Zone = reader.GetString(2),
                            CreationDate = reader.GetDateTime(3),
                            EndDate = reader.GetDateTime(4),
                            AccesoMode = reader.GetString(5),
                            AccesoData = reader.GetString(6)
                        });
                    }
                }
            }

            RowsContent.ItemsSource = reservations;
        }
            private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) // User search box for several parametres
            {
                reservations = new List<Reservation>();
                string searchTerm = SearchTextBox.Text.ToLower();
                ICollectionView view = CollectionViewSource.GetDefaultView(reservations);
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    view.Filter = item =>
                    {
                        if (item is Reservation reservation)
                        {
                            return reservation.ID.ToString().ToLower().Contains(searchTerm) ||
                               reservation.UserID.ToString().ToLower().Contains(searchTerm) ||
                               reservation.Zone.ToLower().Contains(searchTerm) ||
                               reservation.CreationDate.ToString().ToLower().Contains(searchTerm) ||
                               reservation.EndDate.ToString().ToLower().Contains(searchTerm) ||
                               reservation.AccesoMode.ToLower().Contains(searchTerm) ||
                               reservation.AccesoData.ToLower().Contains(searchTerm);
                        }
                        return false;
                    };
                }
                else
                {
                    view.Filter = null;
                }
            }
        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RowsContent.SelectedItem is Reservation selectedReservation)
            {
                // Maneja el evento de selección aquí
                System.Diagnostics.Debug.WriteLine($"Selected: {selectedReservation.Zone}, {selectedReservation.AccesoMode}");
            }
        }
    }
}
