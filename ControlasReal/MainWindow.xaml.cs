using System;
using System.IO.Ports;
using System.Windows;

namespace ControlasReal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort serialPort = new SerialPort();
        User user = new User();
        public bool UsuarioLogueado {  get; set; }
        private LogoutManagement logoutManagement;
        public MainWindow()
        {
            InitializeComponent();
            serialPort.BaudRate = 9600;
            serialPort.PortName = "COM3";
            serialPort.DataReceived += SerialPort_DataReceived;
            ManagementDB ManagementDB = new ManagementDB();
            ManagementDB.OpenConnection();
            //conectarArduino();
            UsuarioLogueado = true; //controlamos el acceso dando un valor false, cuando te registras de forma exitosa pasa a true
            ActualizarVisibilidadMenu();   //cambia la view cuando el UsuarioLogueado = true;
            logoutManagement = new LogoutManagement(this); //  Cierra sesión

        }          
        private void Archivos_Click(object sender, RoutedEventArgs e)
        { //4
            if (UsuarioLogueado)
            {
                MainContent.Content = new FileControler();
            }
        }
        private void Tarjetas_Click(object sender, RoutedEventArgs e)
        {//2
        
        }
        private void Usuarios_Click(object sender, RoutedEventArgs e)
        {
            if (UsuarioLogueado)
            {
                MainContent.Content = new UserContainerSeeker();
            }
        }
        private void Reservas_Click(object sender, RoutedEventArgs e)
        {//3
            if (UsuarioLogueado)
            {
                MainContent.Content = new ReservationsList();
            }
        }
        private void EliminarTarjeta_Click(object sender, RoutedEventArgs e) 
        {
            if (UsuarioLogueado)
            {
                DeleteCardWindow deleteCardWindow = new DeleteCardWindow();
                deleteCardWindow.Show();
            }
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {//5       
            LoginPanel userControl5 = new LoginPanel(this);
            MainContent.Content = userControl5;
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            UsuarioLogueado = false;
            ActualizarVisibilidadMenu();
            MainContent.Content = new WelcomeLayout();
        }
        private void ConsultarInformacion_Click(object sender, RoutedEventArgs e)
        { //1
            if (UsuarioLogueado)
            {
                MainContent.Content = new UserContainerSeeker();
            }
        }
        private void IntroducirUsuarios_Click(object sender, RoutedEventArgs e)
        { //1
            if (UsuarioLogueado)
            {
                RegisterUserModalWindow window3 = new RegisterUserModalWindow();
                window3.ShowDialog();
            }
        }
        private void CardManagement_Click (object sender, RoutedEventArgs e)
        {

        }
        private void AdministradorTarjetas_Click(object sender, RoutedEventArgs e)
        { //1
            MessageBoxResult result = MessageBox.Show("Acerque tarjeta al lector. Mantener en el lector hasta acabar proceso.", "Confirmación", MessageBoxButton.OK);
            if (UsuarioLogueado)
            {
                AddCard window4 = new AddCard(user);
                window4.ShowDialog();
            }
        }
        public void conectarArduino()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.Open();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("Acceso denegado al puerto serial: " + ex.Message);
            }
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort1 = (SerialPort)sender;
            String data = serialPort1.ReadLine();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                serialPort.Close();
            }
            catch
            {

            }
        }
       private void RastreoTarjetaPerdida_Click(object sender, RoutedEventArgs e)
        {
            if (UsuarioLogueado)
            {
                MessageBox.Show("Coloca la tarjeta a rastrear en el lector");
               CardTrackerWindow cardTrackerWindow = new CardTrackerWindow();
                cardTrackerWindow.ShowDialog();
            }
        }
       private void controlRele_Click(object sender, RoutedEventArgs e)
        {
            conectarArduino(); // CAMBIAR EN EL IDE ARDUINO COMANDO PARA ACT Y DESACT LOS RELE  
            serialPort.WriteLine("1");
        }
        public void ActualizarVisibilidadMenu()
        {
            ArchivosMenuItem.Visibility = UsuarioLogueado ? Visibility.Visible : Visibility.Collapsed;
            UsuariosMenuItem.Visibility = UsuarioLogueado ? Visibility.Visible : Visibility.Collapsed;
            ReservasMenuItem.Visibility = UsuarioLogueado ? Visibility.Visible : Visibility.Collapsed;
            LogoutMenuItem.Visibility = UsuarioLogueado ? Visibility.Visible : Visibility.Collapsed;
            LoginMenuItem.Visibility = UsuarioLogueado ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}