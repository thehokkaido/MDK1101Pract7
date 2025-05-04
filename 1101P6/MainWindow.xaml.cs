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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Npgsql;

namespace _1101P6
{

    public partial class MainWindow : Window
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=student;Database=itproduction";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT * FROM employees WHERE Login = @login AND Password = @password", conn);
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("password", password);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string position = reader["Position"].ToString();
                            LogLoginAttempt(login, password, true);

                            if (position == "admin")
                            {
                                AdminWindow adminWindow = new AdminWindow();
                                adminWindow.Show();
                                this.Close();
                            }
                            else
                            {
                                UserWindow userWindow = new UserWindow();
                                userWindow.Show();
                                this.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неверный логин или пароль.");
                        LogLoginAttempt(login, password, false);
                    }
                }
            }
        }

        private void LogLoginAttempt(string login, string password, bool isSuccessful)
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("INSERT INTO loginhistory (Login, Password, IsSuccessful) VALUES (@login, @password, @isSuccessful)", conn);
                cmd.Parameters.AddWithValue("login", login);
                cmd.Parameters.AddWithValue("password", password);
                cmd.Parameters.AddWithValue("isSuccessful", isSuccessful);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
