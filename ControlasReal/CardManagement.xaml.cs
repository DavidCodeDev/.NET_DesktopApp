using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ControlasReal
{
    public partial class CardManagement : UserControl
    {
        private NFCReader nfcReader;

        public CardManagement()
        {
            InitializeComponent();
            nfcReader = new NFCReader(this); // Pasa la instancia de CardManagement a NFCReader

            // Suscribirse al evento Loaded
            this.Loaded += UserControl2_Loaded;
        }

        private void UserControl2_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Llamar a la función para leer la tarjeta cuando el UserControl esté completamente cargado
            nfcReader.ReadCardUID();
        }

        // Método para actualizar el Label con el UID de la tarjeta
        public void MostrarUIDTarjeta(string cardUID)
        {
            // Usar Dispatcher para actualizar la UI
            uidLabel.Dispatcher.Invoke(() => {
                uidLabel.Content = cardUID;
            });
        }
    }
}
