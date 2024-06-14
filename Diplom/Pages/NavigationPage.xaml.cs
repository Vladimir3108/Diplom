using iTextSharp.text.pdf;
using iTextSharp.text;
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
using Paragraph = iTextSharp.text.Paragraph;
using Rectangle = iTextSharp.text.Rectangle;

namespace Diplom.Pages
{
    /// <summary>
    /// Логика взаимодействия для NavigationPage.xaml
    /// </summary>
    public partial class NavigationPage : Page
    {
        public NavigationPage()
        {
            InitializeComponent();
        }

        private void UsersBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UsersPage());
        }

        private void RecruitsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RecruitsPage());
        }

        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы уверены, что хотите выйти?",
                "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private void PDFRecruitBtn_Click(object sender, RoutedEventArgs e)
        {
            Document doc = new Document();
            string timeNow = DateTime.Now.ToShortDateString();

            try
            {
                PdfWriter.GetInstance(doc, new FileStream($"..\\..\\RecruitsReports\\report_{timeNow}.pdf", FileMode.Create));
                doc.Open();
                BaseFont baseFont = BaseFont.CreateFont("C:\\Windows\\Fonts\\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, 14);
                Font font1 = new Font(baseFont, 24, 1, BaseColor.BLACK);
                Paragraph report = new Paragraph($"Отчет на {timeNow}", font);
                report.Alignment = Element.ALIGN_CENTER;
                doc.Add(report);
                var millitaryTerritory = App.Context.MilitaryTerritory.ToList();
                for (int i = 0; i < millitaryTerritory.Count; i++)
                {
                    Paragraph paragraph = new Paragraph($"ФИО поступивших в военную часть {millitaryTerritory[i].TerritoryName}:", font);
                    paragraph.Alignment = Element.ALIGN_LEFT;
                    doc.Add(paragraph);
                    var recruitsInTerritory = App.Context.Recruit.Where(p => p.MilitaryTerritoryId == i).ToList();
                    foreach (var recruit in recruitsInTerritory)
                    {
                        Paragraph recruits = new Paragraph($"{recruit.Surname} {recruit.Name} {recruit.Patronymic}", font);
                        recruits.Alignment = Element.ALIGN_LEFT;
                        doc.Add(recruits);
                    }
                }
                MessageBox.Show("Отчет успешно сформирован", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            finally
            {
                doc.Close();
            }
        }
    }
}
