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
using Npgsql;

namespace _1101P6
{
    public partial class EditEmployeeWindow : Window
    {
        private string login;
        private string connectionString;

        public EditEmployeeWindow(string surname, string name, string patronymic, string login, int experience, string position, string connectionString)
        {
            InitializeComponent();
            SurnameTextBox.Text = surname;
            NameTextBox.Text = name;
            PatronymicTextBox.Text = patronymic;
            LoginTextBox.Text = login;
            ExperienceTextBox.Text = experience.ToString();
            PositionTextBox.Text = position;

            this.login = login;
            this.connectionString = connectionString;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string surname = SurnameTextBox.Text;
            string name = NameTextBox.Text;
            string patronymic = PatronymicTextBox.Text;
            string newLogin = LoginTextBox.Text;
            int experience = int.Parse(ExperienceTextBox.Text);
            string position = PositionTextBox.Text;

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE Employees 
                    SET Surname = @surname, 
                        Name = @name, 
                        Patronymic = @patronymic, 
                        Login = @newLogin, 
                        Experience = @experience, 
                        Position = @position 
                    WHERE Login = @login";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("surname", surname);
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("patronymic", patronymic);
                    cmd.Parameters.AddWithValue("newLogin", newLogin);
                    cmd.Parameters.AddWithValue("experience", experience);
                    cmd.Parameters.AddWithValue("position", position);
                    cmd.Parameters.AddWithValue("login", login);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Данные успешно обновлены.");
            this.Close();
        }
    }
}
