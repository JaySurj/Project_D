/*using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_D.ViewModels
{
    public partial class LoginPageView : BaseView
    {
        private SQLiteConnection _connection;

        public LoginPageView()
        {
            // Establish connection to SQLite database
            string databasePath = "accounts/local_db.db";
            _connection = new SQLiteConnection(databasePath);
            _connection.CreateTable<User>(); // Ensure User table is created
        }
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;
         
        [ICommand]
        public async void Login()
        {
            if (!string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password))
            {
                // Query to retrieve user based on username and password
                var user = _connection.Table<User>().Where(u => u.Email == _username && u.Password == _password).FirstOrDefault();

                if (user != null)
                {
                    // Login successful
                    // You can perform further actions here, such as navigating to another page
                    Console.WriteLine("Login successful");
                }
                else
                {
                    // Login failed
                    Console.WriteLine("Invalid username or password");
                }
            }
            else
            {
                Console.WriteLine("Please enter both username and password");
            }
        }
    }
}
*/