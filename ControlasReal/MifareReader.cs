using PCSC;
using PCSC.Exceptions;
using PCSC.Utils;
using System;
using System.Threading;

namespace ControlasReal
{
    public class MifareReader
    {
        private ISCardContext _context;
        private SCardReader _reader;
        private SCardProtocol _protocol;
        private bool _connected;
        private Thread _monitoringThread;
        private bool _keepMonitoring;

        public event Action<string> CardUIDRead;

        private byte[] SendBuff = new byte[263];
        private byte[] RecvBuff = new byte[263];

        public MifareReader()
        {
            _context = ContextFactory.Instance.Establish(SCardScope.System);
            _reader = new SCardReader(_context);
        }

        public void InitializeReader()
        {
            var readers = _context.GetReaders();
            if (readers.Length <= 0)
            {
                throw new PCSCException(SCardError.NoReadersAvailable, "No readers found.");
            }

            // Print available readers
            foreach (var reader in readers)
            {
                Console.WriteLine("Reader: " + reader);
            }

            // Assuming the first reader
            var sc = _reader.Connect(readers[0], SCardShareMode.Shared, SCardProtocol.Any);
            if (sc != SCardError.Success)
            {
                Console.WriteLine("Reader initialized but no card present.");
                return; // No need to throw an exception here, just return
            }

            _connected = true;
            _protocol = _reader.ActiveProtocol;
        }

        public void StartMonitoring()
        {
            _keepMonitoring = true;
            _monitoringThread = new Thread(MonitorCard);
            _monitoringThread.Start();
        }

        private void MonitorCard()
        {
            while (_keepMonitoring)
            {
                try
                {
                    if (_reader != null && _connected)
                    {
                        var status = _reader.Status(out _, out _, out _, out _);
                        if (status == SCardError.Success)
                        {
                            byte[] uidBytes = ReadCardUID();
                            string uidString = BitConverter.ToString(uidBytes).Replace("-", "");
                            CardUIDRead?.Invoke(uidString);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading card: {ex.Message}");
                }
                Thread.Sleep(1000); // Polling interval
            }
        }

        public byte[] ReadCardUID()
        {
            ClearBuffers();
            SendBuff[0] = 0xFF;
            SendBuff[1] = 0xCA; // Command for Get Data
            SendBuff[2] = 0x00;
            SendBuff[3] = 0x00;
            SendBuff[4] = 0x00; // Le

            var sendPci = SCardPCI.GetPci(_protocol);
            var receivePci = new SCardPCI();
            int receiveLength = RecvBuff.Length;

            var sc = _reader.Transmit(
                sendPci,
                SendBuff,
                5, // Length of command APDU
                receivePci,
                RecvBuff,
                ref receiveLength);

            if (sc != SCardError.Success)
            {
                throw new PCSCException(sc, SCardHelper.StringifyError(sc));
            }

            if (RecvBuff[receiveLength - 2] != 0x90 || RecvBuff[receiveLength - 1] != 0x00)
            {
                throw new Exception($"Failed to read UID. SW1: {RecvBuff[receiveLength - 2]:X2}, SW2: {RecvBuff[receiveLength - 1]:X2}");
            }

            byte[] uid = new byte[receiveLength - 2];
            Array.Copy(RecvBuff, uid, uid.Length);
            return uid;
        }

        public void Disconnect()
        {
            _keepMonitoring = false;
            _monitoringThread?.Join();
            if (_connected)
            {
                _reader.Disconnect(SCardReaderDisposition.Reset);
                _connected = false;
            }
        }

        private void ClearBuffers()
        {
            Array.Clear(SendBuff, 0, SendBuff.Length);
            Array.Clear(RecvBuff, 0, RecvBuff.Length);
        }
    }
}
