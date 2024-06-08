using PCSC;
using Sydesoft.NfcDevice;

using System;
using System.Windows;

namespace ControlasReal
{
    internal class NFCReader
    {
        public ACR122U acr122u; // Instancia del lector de tarjetas NFC
        public string cardUID; // Variable para almacenar el UID de la tarjeta
        private UserControl2 userControl; // Instancia del UserControl2

        public NFCReader(UserControl2 userControl)
        {
            // Inicializar la instancia del lector de tarjetas NFC
            acr122u = new ACR122U();
            this.userControl = userControl; // Guardar la instancia de UserControl2
        }
        
        public void ReadCardUID()
        {
            try
            {
                // Crear una instancia de SCardReader
                var contextFactory = ContextFactory.Instance;
                using var context = contextFactory.Establish(SCardScope.System);
                using var reader = new SCardReader(context);

                // Realizar casting explícito de SCardReader a ICardReader
                ICardReader cardReader = (ICardReader)reader;

                // Llamar al método Read de acr122u pasando cardReader  
                byte[] uidBytes = acr122u.Read(cardReader, 0, 0);

                string uidString = BitConverter.ToString(uidBytes).Replace("-", "");

                userControl.MostrarUIDTarjeta(uidString);

            }
            catch (Exception ex)
            {
                // Mostrar un diálogo de error en caso de excepción
                MessageBox.Show(ex.Message);
            }
        }
    }
}
