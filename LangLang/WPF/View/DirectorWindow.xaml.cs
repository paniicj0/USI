using LangLang.Controllers;
using LangLang.Model;
using LangLang.Reports;
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
using System.Windows.Shapes;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for DirectorWindow.xaml
    /// </summary>
    public partial class DirectorWindow : Window
    {
        private Director director;
        public DirectorWindow(Director director)
        {
            InitializeComponent();
            this.director = director;
        }

        public void DirectorWindow_Load(object sender, RoutedEventArgs e)
        {
            WriteWelcome(director);
        }
        public void WriteWelcome(Director director)
        {
            lblWelcome.Content = "Welcome " + director.Name + " " + director.Surname + "!";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DirectorCreateTutorWindow directorCreateTutorWindow = new DirectorCreateTutorWindow();
            directorCreateTutorWindow.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DirectorDisplayTutorWindow directorDisplayTutorWindow = new DirectorDisplayTutorWindow();
            directorDisplayTutorWindow.ShowDialog();

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DirectorSearchTutorsWindow directorSearchTutorsWindow = new DirectorSearchTutorsWindow();
            directorSearchTutorsWindow.ShowDialog();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Hide();
        }

        private void btnCourses_Click(object sender, RoutedEventArgs e)
        {
            DirectorDisplayCoursesWindow directorDisplayCoursesWindow = new DirectorDisplayCoursesWindow(director);
            directorDisplayCoursesWindow.Show();
            this.Hide();
        }


        private void btnSendReport_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(director);
            reportWindow.Show();
            this.Hide();
        }

        private void btnAddExam_Click(object sender, RoutedEventArgs e)
        {
            DirectorCreateExamWindow createExam = new DirectorCreateExamWindow();
            createExam.ShowDialog();

        }

        private void btnFinishedExams_Click(object sender, RoutedEventArgs e)
        {
            ExamsFinishedDirectorWindow examsFinishedDirectorWindow = new ExamsFinishedDirectorWindow(director);
            examsFinishedDirectorWindow.ShowDialog();
            this.Close();
        }

        private void btnSendThankYouMail_Click(object sender, RoutedEventArgs e)
        {
            DirectorSendThankYouMail thankYouMailWindow = new DirectorSendThankYouMail();
            thankYouMailWindow.ShowDialog();

        }
    }
}
