using Diplom.Entities;
using Microsoft.Win32;
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

namespace Diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditRecruitPage.xaml
    /// </summary>
    public partial class AddEditRecruitPage : Page
    {
        byte[] image = null;
        Recruit recruit = new Recruit();
        public AddEditRecruitPage(Recruit _recruit)
        {
            InitializeComponent();
            if (_recruit != null)
            {
                recruit = _recruit;
                Title = "Редактирование сотрудника";
            }
            DataContext = recruit;
            CBMilitaryTerritory.ItemsSource = App.Context.MilitaryTerritory.ToList();
        }
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if (String.IsNullOrEmpty(recruit.Name))
                errorBuilder.AppendLine("Введите имя");
            if (String.IsNullOrEmpty(recruit.Surname))
                errorBuilder.AppendLine("Введите фамилию");
            if (String.IsNullOrEmpty(recruit.Patronymic))
                errorBuilder.AppendLine("Введите отчество");
            if (String.IsNullOrEmpty(recruit.Address))
                errorBuilder.AppendLine("Введите адрес");
            if (recruit.RecruitId == 0)
                App.Context.Recruit.Add(recruit);
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
                RecruitImage.Source = new ImageSourceConverter().ConvertFrom(image) as ImageSource;
                recruit.Image = image;
            }
        }
    }
}
