using Npgsql;
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


namespace _1101P6
{

    public partial class MenuWindow : Window
    {
        private string connectionString = "Host=localhost;Username=postgres;Password=student;Database=itproduction";
        private string currentTableName = ""; // Текущая таблица (итили Hardware, или Software)

        public MenuWindow()
        {
            InitializeComponent();
        }

        // Загрузка данных из Hardware
        private void ViewTable1_Click(object sender, RoutedEventArgs e)
        {
            currentTableName = "ithardware";
            LoadTableData("SELECT * FROM ithardware");
        }

        // Загрузка данных из Software
        private void ViewTable2_Click(object sender, RoutedEventArgs e)
        {
            currentTableName = "itsoftware";
            LoadTableData("SELECT * FROM itsoftware");
        }

        private void ViewTable3_Click(object sender, RoutedEventArgs e)
        {
            currentTableName = "loginhistory";
            LoadTableData("SELECT * FROM loginhistory");
        }

        // Общий метод для загрузки данных в DataGrid
        private void LoadTableData(string query)
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(query, conn);
                    var adapter = new NpgsqlDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    TableDataGrid.ItemsSource = table.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}");
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentTableName))
            {
                MessageBox.Show("Выберите таблицу для добавления записи.");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Запрос для добавления записи без указания id
                    string insertQuery = $@"
                INSERT INTO {currentTableName} (name, count)
                VALUES (@name, @count)";

                    using (var cmd = new NpgsqlCommand(insertQuery, conn))
                    {
                        // Открываем диалоговое окно для ввода данных
                        var inputDialogName = new InputDialog("Введите название:");
                        if (inputDialogName.ShowDialog() == true && !string.IsNullOrEmpty(inputDialogName.UserInput))
                        {
                            string name = inputDialogName.UserInput;

                            var inputDialogCount = new InputDialog("Введите количество:", "0");
                            if (inputDialogCount.ShowDialog() == true && int.TryParse(inputDialogCount.UserInput, out int count))
                            {
                                cmd.Parameters.AddWithValue("name", name);
                                cmd.Parameters.AddWithValue("count", count);

                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                MessageBox.Show("Неверный формат количества.");
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Название не может быть пустым.");
                            return;
                        }
                    }

                    // Обновление данных в DataGrid
                    LoadTableData($"SELECT * FROM {currentTableName}");
                    MessageBox.Show("Запись успешно добавлена!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentTableName) || TableDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для изменения.");
                return;
            }

            try
            {
                DataRowView selectedRow = (DataRowView)TableDataGrid.SelectedItem;
                int id = Convert.ToInt32(selectedRow["id"]);

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Открываем диалоговое окно для изменения данных
                    var inputDialogName = new InputDialog("Введите новое название:", selectedRow["name"].ToString());
                    if (inputDialogName.ShowDialog() == true && !string.IsNullOrEmpty(inputDialogName.UserInput))
                    {
                        string name = inputDialogName.UserInput;

                        var inputDialogCount = new InputDialog("Введите новое количество:", selectedRow["count"].ToString());
                        if (inputDialogCount.ShowDialog() == true && int.TryParse(inputDialogCount.UserInput, out int count))
                        {
                            // Пример запроса для изменения записи
                            string updateQuery = $@"
                        UPDATE {currentTableName}
                        SET name = @name, count = @count
                        WHERE id = @id";

                            using (var cmd = new NpgsqlCommand(updateQuery, conn))
                            {
                                cmd.Parameters.AddWithValue("id", id);
                                cmd.Parameters.AddWithValue("name", name);
                                cmd.Parameters.AddWithValue("count", count);

                                cmd.ExecuteNonQuery();
                            }

                            // Обновление данных в DataGrid
                            LoadTableData($"SELECT * FROM {currentTableName}");
                            MessageBox.Show("Запись успешно изменена!");
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат количества.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении записи: {ex.Message}");
            }
        }

        // Удаление записи
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentTableName) || TableDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите запись для удаления.");
                return;
            }

            try
            {
                DataRowView selectedRow = (DataRowView)TableDataGrid.SelectedItem;
                int id = Convert.ToInt32(selectedRow["id"]);

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    // Пример запроса для удаления записи
                    string deleteQuery = $@"
                        DELETE FROM {currentTableName}
                        WHERE id = @id";

                    using (var cmd = new NpgsqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                // Обновление данных в DataGrid
                LoadTableData($"SELECT * FROM {currentTableName}");
                MessageBox.Show("Запись успешно удалена!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении записи: {ex.Message}");
            }
        }
    }
}
