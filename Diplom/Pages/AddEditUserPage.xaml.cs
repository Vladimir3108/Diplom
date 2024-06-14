using Diplom.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditUserPage.xaml
    /// </summary>
    public partial class AddEditUserPage : Page
    {
        byte[] image = null;
        Users user = new Users();
        public AddEditUserPage(Users _user)
        {
            InitializeComponent();
            if (_user != null)
            {
                user = _user;
                Title = "Редактирование сотрудника";
            }
            DataContext = user;
            CBRole.ItemsSource = App.Context.Role.ToList();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if (String.IsNullOrEmpty(user.Name))
                errorBuilder.AppendLine("Введите имя");
            if (String.IsNullOrEmpty(user.Surname))
                errorBuilder.AppendLine("Введите фамилию");
            if (String.IsNullOrEmpty(user.Patronymic))
                errorBuilder.AppendLine("Введите отчество");
            if (String.IsNullOrEmpty(user.Phone))
                errorBuilder.AppendLine("Введите номер телефона");
            if (!Regex.IsMatch(user.Phone, @"^89[0-9]{9}$"))
                errorBuilder.AppendLine("Номер телефона не валиден");
            if (String.IsNullOrEmpty(user.Login))
                errorBuilder.AppendLine("Введите логин");
            if (String.IsNullOrEmpty(user.Password))
                errorBuilder.AppendLine("Введите пароль");
            if (CBRole.SelectedItem == null)
                errorBuilder.AppendLine("Выберите роль");
            if (user.UserId == 0)
                App.Context.Users.Add(user);
            if (errorBuilder.Length > 0)
            {
                errorBuilder.Insert(0, "Устраните следующие ошибки:\n");
                MessageBox.Show(errorBuilder.ToString());
            }
            else
            {
                try
                {
                    App.Context.SaveChanges();
                    MessageBox.Show("Информация сохранена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.GoBack();
                }
                catch
                {
                    MessageBox.Show(errorBuilder.ToString());
                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите вернуться?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите выйти?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private void SelectImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image | *.png; *.jpg; *.jpeg";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                image = File.ReadAllBytes(ofd.FileName);
                UserImage.Source = new ImageSourceConverter().ConvertFrom(image) as ImageSource;
                user.Image = image;
            }
        }
    }
}
