using System;
using System.Data.SqlServerCe;

namespace ControlasReal
{
    internal class ManagementDB     
    {
        public string connectionString = "Data Source = C:\\Users\\User\\Desktop\\Bases de datos prácticas\\ControlasDB.sdf; Password=zDcL$Ss672kvw";
        public void OpenConnection()
        {
            using (SqlCeConnection connection = new SqlCeConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    CrearTablaSiNoExiste(connection);
                    CrearTablaSiNoExiste1(connection);
                    CrearTablaSiNoExiste2(connection);
                    AlterarTabla(connection);
                    AlterarTabla2(connection);

                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
        }
        public void CrearTablaSiNoExiste(SqlCeConnection connection) // CREAMOS TABLA 'users' en caso de que no exista
        {
            string cmdText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'users'";
            using (SqlCeCommand cmd = new SqlCeCommand(cmdText, connection))
            {
                object result = cmd.ExecuteScalar();

                if (result == null)
                {
                    cmdText = @"CREATE TABLE users (
                                    id INT NOT NULL IDENTITY(1,1),
                                    nombre NVARCHAR(200) NOT NULL,
                                    apellido1 NVARCHAR(200) NOT NULL,
                                    apellido2 NVARCHAR(200) NOT NULL,
                                    email NVARCHAR(200) NOT NULL,
                                    phone NVARCHAR(20) NOT NULL,
                                    permisos NVARCHAR (200),
                                    PRIMARY KEY(id)
                                );";
                    cmd.CommandText = cmdText;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void CrearTablaSiNoExiste1 (SqlCeConnection connection) // CREAMOS TABLA 'reservations' en caso de que no exista
        {
            string cmdText1 = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'reservations'";
            using (SqlCeCommand cmd1 = new SqlCeCommand(cmdText1, connection))
            {
                object result1 = cmd1.ExecuteScalar();

                if (result1 == null)
                {
                    cmdText1 = @"CREATE TABLE reservations (
                                    id INT NOT NULL IDENTITY(1,1),
                                    user_id INT NOT NULL,
                                    zone NVARCHAR(200) NOT NULL,
                                    creation_date DATETIME NOT NULL,
                                    start_date DATETIME NOT NULL,
                                    end_date DATETIME NOT NULL, 
                                    acceso_mode NVARCHAR(5),
                                    acceso_data NVARCHAR(50),
                                    PRIMARY KEY(id)
                                );";
                    cmd1.CommandText = cmdText1;
                    cmd1.ExecuteNonQuery();
                }
            }              
        }
       public void CrearTablaSiNoExiste2(SqlCeConnection connection) // CREAMOS TABLA 'cards' en caso de que no exista
       {

            string cmdText3 = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'cards'";
            using (SqlCeCommand cmd3 = new SqlCeCommand(cmdText3, connection))
            {
                object result3 = cmd3.ExecuteScalar();

                if (result3 == null)
                {
                    cmdText3 = @"CREATE TABLE cards (
                                    id INT NOT NULL IDENTITY(1,1),
                                    user_id INT NOT NULL,
                                    uid NVARCHAR(15) NOT NULL,
                                    state NVARCHAR(5) NOT NULL,
                                    PRIMARY KEY(id)
                                );";
                    cmd3.CommandText = cmdText3;
                    cmd3.ExecuteNonQuery();
                }
            }         
       }
        public void AlterarTabla(SqlCeConnection connection) // ALTERAMOS TABLA 'reservations' para crearle la FK
        {
            string cmdText2 = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'reservations'";

            using (SqlCeCommand cmd2 = new SqlCeCommand(cmdText2, connection))
            {
                object result2 = cmd2.ExecuteScalar();

                if (result2 != null)
                {
                    cmdText2 = @"ALTER TABLE reservations
                        ADD CONSTRAINT FK_UserID FOREIGN KEY (user_id) REFERENCES users(id);";
                    cmd2.CommandText = cmdText2;
                    cmd2.ExecuteNonQuery();
                }
            }
        }
        public void AlterarTabla2(SqlCeConnection connection) // ALTERAMOS TABLA 'cards' para crearle la FK
        {
            string cmdText4 = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'cards'";

            using (SqlCeCommand cmd4 = new SqlCeCommand(cmdText4, connection))
            {
                object result2 = cmd4.ExecuteScalar();

                if (result2 != null)
                {
                    cmdText4 = @"ALTER TABLE cards
                        ADD CONSTRAINT FK_UserID FOREIGN KEY (user_id) REFERENCES users(id);";
                    cmd4.CommandText = cmdText4;
                    cmd4.ExecuteNonQuery();
                }
            }
        }
    }
}