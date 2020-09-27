using MySql.Data.MySqlClient;
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

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        

        const string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=test;";
        User currUser = null;
        public MainWindow()
        {
            InitializeComponent();
            //listUsers();
          // lvUsers.Items.Add(new User { id = 1, name = "Mamamo", address = "Isla" });
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
         {
             if (lvUsers.SelectedIndex > -1)
             {
                 currUser = (User)lvUsers.SelectedItem;
                 btnEdit.IsEnabled = true;
                 btnDelete.IsEnabled = true;
                 txtID.Text = currUser.id.ToString();
                 txtName.Text = currUser.name;
                 txtAddress.Text = currUser.address;
             }
             else {
                 currUser = null;
                 btnEdit.IsEnabled = false;
                 btnDelete.IsEnabled = false;
                 txtID.Text = "";
                 txtName.Text = "";
                 txtAddress.Text = "";
             }

         }
     
        class User
        {
            public int id { get; set; }
            public string name { get; set; }
            public string address { get; set; }
        }

        private void listUsers()
        {
            lvUsers.Items.Clear();

            string query = "SELECT * FROM tbl_users";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();
                // Success, now list 

                // If there are available rows
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        User _tmpUser = new User { id = reader.GetInt32(0), name = reader.GetString(1), address = reader.GetString(2) };
                        lvUsers.Items.Add(_tmpUser);
                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                }

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd(object sender, RoutedEventArgs e)
        {
            string query = "INSERT INTO tbl_users(`name`, `address`) VALUES (@NAME, @ADDRESS)";
            // Which could be translated manually to :
            // INSERT INTO user(`id`, `first_name`, `last_name`, `address`) VALUES (NULL, 'Bruce', 'Wayne', 'Wayne Manor')

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.Parameters.AddWithValue("@NAME", txtName.Text);
            commandDatabase.Parameters.AddWithValue("@ADDRESS", txtAddress.Text);
            commandDatabase.CommandTimeout = 60;

            try
            {
                databaseConnection.Open();
                MySqlDataReader myReader = commandDatabase.ExecuteReader();

                MessageBox.Show("User succesfully registered");

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Show any error message.
                MessageBox.Show(ex.Message);
            }
            finally
            {
                listUsers();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            // Update the properties of the row with ID 1
            string query = "UPDATE `tbl_users` SET `name`=@NAME,`address`=@ADDRESS WHERE id = @ID";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);

            commandDatabase.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
            commandDatabase.Parameters.AddWithValue("@NAME", txtName.Text);
            commandDatabase.Parameters.AddWithValue("@ADDRESS", txtAddress.Text);
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                // Succesfully updated

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Ops, maybe the id doesn't exists ?
                MessageBox.Show(ex.Message);
            }
            finally
            {
                listUsers();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Delete the item with ID 1
            string query = "DELETE FROM `tbl_users` WHERE id = @ID";

            MySqlConnection databaseConnection = new MySqlConnection(connectionString);
            MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
            commandDatabase.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
            commandDatabase.CommandTimeout = 60;
            MySqlDataReader reader;

            try
            {
                databaseConnection.Open();
                reader = commandDatabase.ExecuteReader();

                // Succesfully deleted

                databaseConnection.Close();
            }
            catch (Exception ex)
            {
                // Ops, maybe the id doesn't exists ?
                MessageBox.Show(ex.Message);
            }
            finally
            {
                listUsers();
            }
        }
    }

}

