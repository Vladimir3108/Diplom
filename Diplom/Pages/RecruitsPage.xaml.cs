using System;
using System.Collections.Generic;
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
using Diplom.Entities;
using Microsoft.Win32;
using OfficeOpenXml;

namespace Diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для RecruitsPage.xaml
    /// </summary>
    public partial class RecruitsPage : Page
    {
        Recruit recruit = new Recruit();
        string surname, name, patronymic, address;
        public RecruitsPage()
        {
            InitializeComponent();
            UpdateRecruit();
            if (App.CurrentUser.RoleId != 1)
            {
                AddRecruitBtn.Visibility = Visibility.Collapsed;
                LoadFileBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateRecruit()
        {
            var recruits = App.Context.Recruit.ToList();
            ListRecruits.ItemsSource = recruits;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentRecruits = (sender as Button).DataContext as Recruit;
            NavigationService.Navigate(new AddEditRecruitPage(currentRecruits));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentRecruits = (sender as Button).DataContext as Recruit;
            if (MessageBox.Show($"Вы уверены, что хотите удалить пользователя: {currentRecruits.Surname} {currentRecruits.Name}?",
                    "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Recruit.Remove(currentRecruits);
                App.Context.SaveChanges();
                UpdateRecruit();
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

        private void AddRecruitBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditRecruitPage(null));
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите выйти?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateRecruit();
        }

        private void MedCardBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentRecruits = (sender as Button).DataContext as Recruit;
            NavigationService.Navigate(new MedPage(currentRecruits));
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
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Получаем доступ к первому листу

                    //MessageBox.Show(worksheet.Name);
                    int row = worksheet.Dimension.Rows; // Получаем количество строк
                    int col = worksheet.Dimension.Columns;

                    for (int i = 2; i <= row; i++)
                    {
                        for (int j = 2; j <= col; j++)
                        {
                            recruit = new Recruit();
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
                                    address = worksheet.Cells[i, j].Value.ToString();
                                    break;
                                default:
                                    break;
                            }
                        }
                        recruit.Surname = surname;
                        recruit.Name = name;
                        recruit.Patronymic = patronymic;
                        recruit.Address = address;
                        recruit.MilitaryTerritoryId = null;
                        recruit.DraftDate = null;

                        App.Context.Recruit.Add(recruit);
                    }
                    try
                    {
                        App.Context.SaveChanges();
                        MessageBox.Show("Информация сохранена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                        UpdateRecruit();
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
