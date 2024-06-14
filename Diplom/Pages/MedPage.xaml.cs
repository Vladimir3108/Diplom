using Diplom.Entities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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
    /// Логика взаимодействия для MedPage.xaml
    /// </summary>
    public partial class MedPage : Page
    {
        Recruit recruit = new Recruit();
        MedCard medCardNew = new MedCard();
        string medOpinion;
        string destFileForVaccinatiion = "NULL";
        string destFileForAnalysys = "NULL";
        public MedPage(Recruit _recruit)
        {
            InitializeComponent();
            recruit = _recruit;
            var medCard = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).ToList();
            if (medCard.Count == 0)
            {
                medCardNew.CategoryId = null;
                medCardNew.Vaccinations = null;
                medCardNew.Result = null;
                medCardNew.RecruitId = recruit.RecruitId;
                App.Context.MedCard.Add(medCardNew);
                App.Context.SaveChanges();
            }
            DataContext = recruit;
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            var medcardID = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).Select(p => p.MedCardId).First();
            var visibleVaccination = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId && p.Vaccinations == null).ToList();
            if (visibleVaccination.Count == 0)
                AddVaccinationsBtn.Content = "Заменить";
            else BrowseBtn.Visibility = Visibility.Collapsed;
            var visibleAnalyse = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId && p.Analyse == null).ToList();
            if (visibleAnalyse.Count == 0)
                AddTestsBtn.Content = "Заменить";
            else BrowseTestsBtn.Visibility = Visibility.Collapsed;
            if (DermatDoctor.Text == App.CurrentUser.RoleName)
            {
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (NarcoDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (NeuroDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (OculistDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (OtorDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (PsychDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (DentistDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (TherapistDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddSurgeonConclusionBtn.Visibility = Visibility.Collapsed;
            }
            else if (SurgeonDoctor.Text == App.CurrentUser.RoleName)
            {
                AddDermatConclusionBtn.Visibility = Visibility.Collapsed;
                AddNarcoConclusionBtn.Visibility = Visibility.Collapsed;
                AddNeuroConclusionBtn.Visibility = Visibility.Collapsed;
                AddOculistConclusionBtn.Visibility = Visibility.Collapsed;
                AddOtorConclusionBtn.Visibility = Visibility.Collapsed;
                AddPsychConclusionBtn.Visibility = Visibility.Collapsed;
                AddDentistConclusionBtn.Visibility = Visibility.Collapsed;
                AddTherapistConclusionBtn.Visibility = Visibility.Collapsed;
            }
            var dermat = App.Context.MedicalOpinions.Where(p => p.DoctorId == 9 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (dermat.Count != 0)
            {
                int numDermat = dermat[0];
                var dermatOpinion = App.Context.Opinion.Where(p => p.OpinionId == numDermat).Select(p => p.Opinion1).ToList();
                DermatStatus.Text = dermatOpinion[0];
            }
            else DermatStatus.Text = "Заключение отсутствует";
            var narco = App.Context.MedicalOpinions.Where(p => p.DoctorId == 10 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (narco.Count != 0)
            {
                int numNarco = narco[0];
                var narcoOpinion = App.Context.Opinion.Where(p => p.OpinionId == numNarco).Select(p => p.Opinion1).ToList();
                NarcoStatus.Text = narcoOpinion[0];
            }
            else NarcoStatus.Text = "Заключение отсутствует";
            var neuro = App.Context.MedicalOpinions.Where(p => p.DoctorId == 4 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (neuro.Count != 0)
            {
                int numNeuro = neuro[0];
                var neuroOpinion = App.Context.Opinion.Where(p => p.OpinionId == numNeuro).Select(p => p.Opinion1).ToList();
                NeuroStatus.Text = neuroOpinion[0];
            }
            else NeuroStatus.Text = "Заключение отсутствует";
            var oculist = App.Context.MedicalOpinions.Where(p => p.DoctorId == 6 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (oculist.Count != 0)
            {
                int numOculist = oculist[0];
                var oculistOpinion = App.Context.Opinion.Where(p => p.OpinionId == numOculist).Select(p => p.Opinion1).ToList();
                OculistStatus.Text = oculistOpinion[0];
            }
            else OculistStatus.Text = "Заключение отсутствует";
            var otor = App.Context.MedicalOpinions.Where(p => p.DoctorId == 7 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (otor.Count != 0)
            {
                int numOtor = otor[0];
                var otorOpinion = App.Context.Opinion.Where(p => p.OpinionId == numOtor).Select(p => p.Opinion1).ToList();
                OtorStatus.Text = otorOpinion[0];
            }
            else OtorStatus.Text = "Заключение отсутствует";
            var psych = App.Context.MedicalOpinions.Where(p => p.DoctorId == 5 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (psych.Count != 0)
            {
                int numPsych = psych[0];
                var psychOpinion = App.Context.Opinion.Where(p => p.OpinionId == numPsych).Select(p => p.Opinion1).ToList();
                PsychStatus.Text = psychOpinion[0];
            }
            else PsychStatus.Text = "Заключение отсутствует";
            var dentist = App.Context.MedicalOpinions.Where(p => p.DoctorId == 8 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (dentist.Count != 0)
            {
                int numDentist = dentist[0];
                var dentistOpinion = App.Context.Opinion.Where(p => p.OpinionId == numDentist).Select(p => p.Opinion1).ToList();
                DentistStatus.Text = dentistOpinion[0];
            }
            else DentistStatus.Text = "Заключение отсутствует";
            var therapist = App.Context.MedicalOpinions.Where(p => p.DoctorId == 3 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (therapist.Count != 0)
            {
                int numTherapist = therapist[0];
                var therapistOpinion = App.Context.Opinion.Where(p => p.OpinionId == numTherapist).Select(p => p.Opinion1).ToList();
                TherapistStatus.Text = therapistOpinion[0];
            }
            else TherapistStatus.Text = "Заключение отсутствует";
            var surgeon = App.Context.MedicalOpinions.Where(p => p.DoctorId == 2 && p.MedCardId == medcardID).Select(p => p.OpinionId).ToList();

            if (surgeon.Count != 0)
            {
                int numSurgeon = surgeon[0];
                var surgeonOpinion = App.Context.Opinion.Where(p => p.OpinionId == numSurgeon).Select(p => p.Opinion1).ToList();
                SurgeonStatus.Text = surgeonOpinion[0];
            }
            else SurgeonStatus.Text = "Заключение отсутствует";
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите вернуться?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                NavigationService.GoBack();
            }
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите выйти?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            var medCard = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).ToList();
            Process.Start(medCard[0].Vaccinations);
        }

        private void AddVaccinationsBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Documents (*.pdf)|*.pdf";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.SafeFileName;
                string sourcePath = ofd.FileName;

                destFileForVaccinatiion = @"..\Resources\MedicalOpinion\" + fileName;
                if (!File.Exists(destFileForVaccinatiion))
                    File.Copy(sourcePath, destFileForVaccinatiion);
                var medCard = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).First();
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VovaDiplom;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"Update MedCard set Vaccinations='{destFileForVaccinatiion}' where MedCardId={medCard.MedCardId}", connection);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Информация сохранена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
            UpdateVisibility();
        }

        private void BrowseTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            var medCard = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).ToList();
            Process.Start(medCard[0].Analyse);
        }

        private void AddTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PDF Documents (*.pdf)|*.pdf";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                string fileName = ofd.SafeFileName;
                string sourcePath = ofd.FileName;

                destFileForAnalysys = @"..\Resources\MedicalOpinion\" + fileName;
                if (!File.Exists(destFileForAnalysys))
                    File.Copy(sourcePath, destFileForAnalysys);
                var medCard = App.Context.MedCard.Where(p => p.RecruitId == recruit.RecruitId).First();
                string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=VovaDiplom;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"Update MedCard set Analyse='{destFileForAnalysys}' where MedCardId={medCard.MedCardId}", connection);
                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        MessageBox.Show("Информация сохранена", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            UpdateVisibility();
        }

        private void AddDermatConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddNarcoConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddNeuroConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddOculistConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddOtorConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddPsychConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddDentistConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddTherapistConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void AddSurgeonConclusionBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditOpinionDoctorPage((sender as Button).DataContext as Recruit));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateVisibility();
        }
    }
}
