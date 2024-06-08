using System.Windows.Controls;
using System.Windows.Threading;

namespace ControlasReal
{
    public partial class UserControl2 : UserControl
    {
        private NFCReader nfcReader;

        public UserControl2()
        {
            InitializeComponent();
            nfcReader = new NFCReader(this); // Pasa la instancia de UserControl2 a NFCReader

            // funcion para leer
            nfcReader.ReadCardUID();
        }

        // Método para actualizar el Label con el UID de la tarjeta
        public void MostrarUIDTarjeta(string cardUID)
        {
            // no funciona
            uidLabel.Dispatcher.Invoke(() => {
                uidLabel.Content = cardUID;
            });
        }
    }
}
