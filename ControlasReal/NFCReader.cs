using System;
using System.Windows;

namespace ControlasReal
{
    internal class NFCReader
    {
        private MifareReader _mifareReader;
        private CardManagement _userControl;

        public NFCReader(CardManagement userControl)
        {
            _mifareReader = new MifareReader();
            _userControl = userControl;
        }
        public void ReadCardUID()
        {
            try
            {
                _mifareReader.InitializeReader();
                byte[] uidBytes = _mifareReader.ReadCardUID();
                string uidString = BitConverter.ToString(uidBytes).Replace("-", "");

                _userControl.MostrarUIDTarjeta(uidString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
