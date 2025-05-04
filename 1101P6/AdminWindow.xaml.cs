using System;
using System.Collections.Generic;
using System.Data;
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

    public partial class AdminWindow : Window
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=student;Database=itproduction";

        public AdminWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new NpgsqlCommand("SELECT * FROM Employees", conn);
                var adapter = new NpgsqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                EmployeesDataGrid.ItemsSource = table.DefaultView;
            }
        }

        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = EmployeesDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                string surname = selectedRow["Surname"].ToString();
                string name = selectedRow["Name"].ToString();
                string patronymic = selectedRow["Patronymic"].ToString();
                string login = selectedRow["Login"].ToString();
                int experience = int.Parse(selectedRow["Experience"].ToString());
                string position = selectedRow["Position"].ToString();

                // Передаем connectionString в конструктор EditEmployeeWindow
                var editEmployeeWindow = new EditEmployeeWindow(surname, name, patronymic, login, experience, position, connectionString);
                editEmployeeWindow.ShowDialog();

                // Обновляем таблицу после закрытия окна редактирования
                LoadEmployees();
            }
            else
            {
                MessageBox.Show("Выберите сотрудника для редактирования.");
            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = EmployeesDataGrid.SelectedItem as DataRowView;
            if (selectedRow != null)
            {
                string login = selectedRow["Login"].ToString();
                var changePasswordWindow = new ChangePasswordWindow(login, connectionString);
                changePasswordWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите сотрудника.");
            }
        }
        private void OpenMenuWindow_Click(object sender, RoutedEventArgs e)
        {
            var menuWindow = new MenuWindow();
            menuWindow.Show();
        }

      
    }
}
