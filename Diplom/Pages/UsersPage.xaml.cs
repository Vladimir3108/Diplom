using Diplom.Entities;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        Users user;
        string roleName, surname, name, patronymic, phone, login, password;
        public UsersPage()
        {
            InitializeComponent();
            UpdateUser();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateUser();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentUsers = (sender as Button).DataContext as Entities.Users;
            NavigationService.Navigate(new AddEditUserPage(currentUsers));
        }

        private void UpdateUser()
        {
            var users = App.Context.Users.ToList();
            ListUsers.ItemsSource = users;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentUsers = (sender as Button).DataContext as Entities.Users;
            if (MessageBox.Show($"Вы уверены, что хотите удалить пользователя: {currentUsers.Surname} {currentUsers.Name}?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Users.Remove(currentUsers);
                App.Context.SaveChanges();
                UpdateUser();
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

        private void AddUserBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditUserPage(null));
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите выйти?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private void LoadFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files|*.xlsx;*.xls";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(ofd.FileName);
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Получаем доступ к первому листу

                    //MessageBox.Show(worksheet.Name);
                    int row = worksheet.Dimension.Rows; // Получаем количество строк
                    int col = worksheet.Dimension.Columns;

                    for (int i = 2; i <= row; i++)
                    {
                        for (int j = 2; j <= col; j++)
                        {
                            user = new Users();
                            switch (j)
                            {
                                case 2:
                                    //MessageBox.Show(worksheet.Cells[i, j].Value.ToString());
                                    surname = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                case 3:
                                    name = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                case 4:
                                    patronymic = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                case 5:
                                    roleName = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                case 6:
                                    phone = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                case 7:
                                    login = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                case 8:
                                    password = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                        var roles = App.Context.Role.Where(p => p.Role1 == roleName).Select(p => p.RoleId).ToList();
                        int role = roles[0];
                        user.Surname = surname;
                        user.Name = name;
                        user.Patronymic = patronymic;
                        user.RoleId = role;
                        user.Phone = phone;
                        user.Login = login;
                        user.Password = password;

                        App.Context.Users.Add(user);

                    }
                    try
                    {
                        App.Context.SaveChanges();
                        MessageBox.Show("Информация сохранена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateUser();
                    }
                    catch
                    {
                        MessageBox.Show("Таблица сделана не по шаблону", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
    }
}
