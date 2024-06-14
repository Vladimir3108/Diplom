using Diplom.Entities;
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
    /// Логика взаимодействия для AddEditOpinionDoctorPage.xaml
    /// </summary>
    public partial class AddEditOpinionDoctorPage : Page
    {
        Recruit recruit = new Recruit();
        MedicalOpinions opinion = new MedicalOpinions();
        public AddEditOpinionDoctorPage(Recruit _recruit)
        {
            InitializeComponent();
            CategoryCb.ItemsSource = App.Context.CategoryIllness.ToList();
            OpinionCb.ItemsSource = App.Context.Opinion.ToList();
            recruit = _recruit;
            var medCard = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).ToList();
            var numMedCard = medCard[0].MedCardId;
            var medicalOpinion = App.Context.MedicalOpinions.Where(p => p.DoctorId == App.CurrentUser.UserId && p.MedCardId == numMedCard).ToList();
            if (medicalOpinion.Count != 0)
            {
                var numMedicalOpinion = medicalOpinion[0].IllnessID;
                opinion.OpinionId = medicalOpinion[0].OpinionId;
                opinion.Note = medicalOpinion[0].Note;
                opinion.DoctorId = medicalOpinion[0].DoctorId;
                opinion.MedCardId = medicalOpinion[0].MedCardId;
                opinion.IllnessID = medicalOpinion[0].IllnessID;
                IllnessStackPanel.Visibility = Visibility.Visible;
                OpinionCb.SelectedIndex = opinion.OpinionId;
                if (opinion.IllnessID != null)
                {
                    var illness = App.Context.Illness.Where(p => p.IllnessId == opinion.IllnessID).First();
                    IllnessCb.SelectedIndex = illness.IllnessId;
                    var category = App.Context.CategoryIllness.Where(p => p.CategoryIllnessId == illness.CategoryIllnessId).First();
                    CategoryCb.SelectedIndex = category.CategoryIllnessId;
                }
                if(opinion.Note != null)
                    NoteTxt.Text = opinion.Note;
            }
            DataContext = opinion;
        }

        private void CategoryCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IllnessCb.ItemsSource = App.Context.Illness.Where(p => p.CategoryIllnessId == CategoryCb.SelectedIndex + 1).ToList();
            IllnessStackPanel.Visibility = Visibility.Visible;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errorBuilder = new StringBuilder();
            if (OpinionCb.SelectedItem == null)
                errorBuilder.AppendLine("Выберите состояние здоровья");
            if (CategoryCb.SelectedItem != null && IllnessCb.SelectedItem == null)
                errorBuilder.AppendLine("Выберите болезнь");
            if (opinion.MedicalOpinionId == 0)
            {
                opinion.DoctorId = App.CurrentUser.UserId;
                var medCard = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).ToList();
                opinion.MedCardId = medCard[0].MedCardId;
                App.Context.MedicalOpinions.Add(opinion);
            }

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
    }
}
