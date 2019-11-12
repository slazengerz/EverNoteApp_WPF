using EverNoteApp.ViewModel;
using System;
using System.Windows;

namespace EverNoteApp.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            loginStackPanel.Visibility = Visibility.Visible;
            registerStackPanel.Visibility = Visibility.Collapsed;
            LoginVM vm = new LoginVM();
            containerGrid.DataContext = vm;
            LoginVM.HasLoggedIn += UserIsLoggedIn;
        }

        private void UserIsLoggedIn(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DisplayRegisterPage_Click(object sender, RoutedEventArgs e)
        {
            loginStackPanel.Visibility = Visibility.Collapsed;
            registerStackPanel.Visibility = Visibility.Visible;
        }

        private void DispLoginPage_Click(object sender, RoutedEventArgs e)
        {
            loginStackPanel.Visibility = Visibility.Visible;
            registerStackPanel.Visibility = Visibility.Collapsed;
        }
    }
}
