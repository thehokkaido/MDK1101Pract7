using Npgsql;
using System;
using System.Collections.Generic;
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

namespace _1101P6
{

    public partial class ChangePasswordWindow : Window
    {
        private string login;
        private string connectionString;

        public ChangePasswordWindow(string login, string connectionString)
        {
            InitializeComponent();
            this.login = login;
            this.connectionString = connectionString;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (newPassword == confirmPassword)
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Employees SET Password = @password WHERE Login = @login";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("password", newPassword);
                        cmd.Parameters.AddWithValue("login", login);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Пароль успешно изменен.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Пароли не совпадают.");
            }
        }
    }
}
