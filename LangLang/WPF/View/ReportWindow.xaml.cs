using LangLang.Model;
using LangLang.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class ReportWindow : Window
    {
        Director director;
        public DirectorWindow directorWindow;
        public ReportWindow(Director director)
        {
            InitializeComponent();
            this.director = director;
            directorWindow = new DirectorWindow(director);
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Hide();
        }

        private void btnReport1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var report = new PenaltyPointsReport(director);
                report.GenerateAndSendReport();
                MessageBox.Show("Report has been sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnReport2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var report = new CourseKnowledgeGeneratorReport(director);
                report.GenerateAndSendReport();
                MessageBox.Show("Report has been sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnReport3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var report = new ExamGeneratorReport(director);
                report.GenerateAndSendReport();
                MessageBox.Show("Report has been sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnReport4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var report = new LanguageStatisticsReport(director);
                report.GenerateAndSendReport();
                MessageBox.Show("Report has been sent successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                directorWindow.Show();
            }
        }
    }
}
