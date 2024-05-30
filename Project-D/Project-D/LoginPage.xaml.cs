using SQLite;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace Project_D
{
    public partial class LoginPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private DropboxClient _dbx;
        private readonly string _databasePath;

        public LoginPage()
        {
            InitializeComponent();
            _databasePath = DatabaseConfig.DatabasePath;
            InitializeDatabase();
        }

        private async void InitializeDatabase()
{
    try
    {
        _dbx = DropboxClientFactory.GetClient();

        // Ensure directory exists
        var directory = Path.GetDirectoryName(_databasePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Check if the Dropbox folder is empty
        var listFolderResponse = await _dbx.Files.ListFolderAsync("/Project_D");
        if (listFolderResponse.Entries.Count == 0)
        {
            // Wipe clean the local database
            await WipeLocalDatabase();
            return;
        }

        // Not empty, proceed with downloading the database
        await DownloadDatabaseFromDropbox("/Project_D", DatabaseConfig.DatabaseName, _databasePath);
        _connection = new SQLiteAsyncConnection(_databasePath);

        // Ensure the admin account exists
        await EnsureAdminAccount();
    }
    catch (Exception ex)
    {
        await DisplayAlert("Error", $"An error occurred while initializing database: {ex.Message}", "OK");
    }
}


        private async Task WipeLocalDatabase()
        {
            try
            {
                // Delete the local database file
                if (File.Exists(_databasePath))
                {
                    File.Delete(_databasePath);
                    await DisplayAlert("Info", "Local database wiped clean.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while wiping local database: {ex.Message}", "OK");
            }
        }

        private async Task DownloadDatabaseFromDropbox(string folder, string fileName, string localFilePath)
        {
            try
            {
                var response = await _dbx.Files.DownloadAsync(folder + "/" + fileName);

                using (var fileContentStream = await response.GetContentAsStreamAsync())
                {
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileContentStream.CopyToAsync(fileStream);
                    }
                }

                await DisplayAlert("Success", $"File downloaded: {fileName}", "OK");
            }
            catch (Dropbox.Api.ApiException<DownloadError> ex)
            {
                await DisplayAlert("Dropbox API Error", $"Error: {ex.ErrorResponse}", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Download failed: {ex.Message}", "OK");
            }
        }

        private async Task EnsureAdminAccount()
        {
            try
            {
                // Check if the admin account exists
                var adminEmail = "admin@example.com";
                var adminUser = await _connection.Table<User>().FirstOrDefaultAsync(u => u.Email == adminEmail);

                if (adminUser == null)
                {
                    // Admin account does not exist, create it
                    adminUser = new User
                    {
                        Email = adminEmail,
                        Password = "adminpassword" 
                    };
                    await _connection.InsertAsync(adminUser);
                    DisplayAlert("Info", "Admin account created.", "OK");
                    // Upload the updated database back to Dropbox
                    await UploadDatabaseToDropbox("/Project_D", DatabaseConfig.DatabaseName, _databasePath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred while ensuring admin account: {ex.Message}", "OK");
            }
        }

        private async Task UploadDatabaseToDropbox(string folder, string fileName, string localFilePath)
        {
            try
            {
                using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read))
                {
                    using (var memStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memStream);
                        memStream.Position = 0; 

                        var updated = await _dbx.Files.UploadAsync(
                            folder + "/" + fileName,
                            WriteMode.Overwrite.Instance,
                            body: memStream);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Upload failed: {ex.Message}", "OK");
            }
        }

        public async void Login_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Ensure the database is ready
                if (_connection == null)
                {
                    await DisplayAlert("Error", "Database is not initialized yet. Please try again later.", "OK");
                    return;
                }

                string email = EmailEntry.Text;
                string password = passwordEntry.Text;

                var user = await _connection.Table<User>().FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
                //if user is an admin
                if (user != null && user.Email == "admin@example.com")
                {
                    var allUsers = await _connection.Table<User>().ToListAsync();

                    await Navigation.PushAsync(new AdminPage());
                }

                else if (user != null)
                {
                    await DisplayAlert("Success", "Login successful!", "OK");
                    await Navigation.PushAsync(new HomePage(user));
                }
                else
                {
                    await DisplayAlert("Error", "Invalid email or password.", "OK");
                }
                

                if (user != null)
                {
                    await DisplayAlert("Success", "Login successful!", "OK");
                    // Navigate to a success page/ home page
                    await Navigation.PushAsync(new HomePage(user));
                }
                else
                {
                    await DisplayAlert("Error", "Invalid email or password.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        private async void SignUp(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupPage());
        }
    }
}