using LangLang.Controllers;
using LangLang.Model;
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
    /// Interaction logic for TutorGradeStudentExamWindow.xaml
    /// </summary>
    public partial class TutorGradeStudentExamWindow : Window
    {
        private AppliedExam selectedAppliedExam;
        private CourseController courseController;
        private NotificationController notificationController;

        private AppliedExamController appliedExamController = new AppliedExamController();
        public TutorGradeStudentExamWindow(AppliedExam selectedAppliedExam)
        {
            InitializeComponent();
            this.selectedAppliedExam = selectedAppliedExam;
            courseController = new CourseController();
            notificationController = new NotificationController();
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtReading.Text) ||
            string.IsNullOrWhiteSpace(txtWriting.Text) ||
            string.IsNullOrWhiteSpace(txtListening.Text) ||
            string.IsNullOrWhiteSpace(txtSpeaking.Text))
            {
                MessageBox.Show("Please enter all grades before checking!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            int pointsReading, pointsWriting, pointsListening, pointsSpeaking;
            if (!int.TryParse(txtReading.Text, out pointsReading) ||
                !int.TryParse(txtWriting.Text, out pointsWriting) ||
                !int.TryParse(txtListening.Text, out pointsListening) ||
                !int.TryParse(txtSpeaking.Text, out pointsSpeaking))
            {
                MessageBox.Show("Please enter correct numerical values for grades!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            // Check if points are within allowed range
            if (pointsReading > 60 || pointsWriting > 60 || pointsListening > 40 || pointsSpeaking > 50)
            {
                MessageBox.Show("Please enter grades within the allowed range!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return; 
            }

            selectedAppliedExam.Reading = pointsReading;
            selectedAppliedExam.Writting = pointsWriting;
            selectedAppliedExam.Listening = pointsListening;
            selectedAppliedExam.Speaking = pointsSpeaking;
            appliedExamController.Update(selectedAppliedExam);

            MessageBox.Show("You have successfully graded the student!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

            bool passedReading = (pointsReading >= 30);
            bool passedWriting = (pointsWriting >= 30);
            bool passedListening = (pointsListening >= 20);
            bool passedSpeaking = (pointsSpeaking >= 25);

            bool overallPassed = ((pointsReading + pointsWriting + pointsListening + pointsSpeaking) >= 160);

            if (passedReading && passedWriting && passedListening && passedSpeaking && overallPassed)
            {
                MessageBox.Show("Exam passed!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                selectedAppliedExam.Passed = true;
                appliedExamController.Update(selectedAppliedExam);
            }
            else
            {
                MessageBox.Show("Exam not passed!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
