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

namespace Diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentUser = App.Context.Users
                .Where(p => p.Login == LoginTxt.Text && p.Password == PassTxt.Password).FirstOrDefault();
            if (LoginTxt.Text == "" || PassTxt.Password == "")
                ErrorTxt.Text = "Не оставляйте поля не заполненными";
            else if (currentUser != null)
            {
                App.CurrentUser = currentUser;
                if (currentUser.RoleId == 1)
                {
                    NavigationService.Navigate(new NavigationPage());

                }
                else
                {
                    NavigationService.Navigate(new RecruitsPage());
                }
            }
            else
            {
                ErrorTxt.Text = "Неправильный логин или пароль";
            }
        }

        private void PassTxt_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (PassTxt.SecurePassword.Length == 0)
            {
                PlaceholderTxt.Visibility = Visibility.Visible;
            }
            else
            {
                PlaceholderTxt.Visibility = Visibility.Collapsed;
            }
        }
    }
}
