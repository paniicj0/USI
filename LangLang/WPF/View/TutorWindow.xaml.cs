using LangLang.Controllers;
using LangLang.Model;
using LangLang.View;
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

namespace USIProject.View
{
    /// <summary>
    /// Interaction logic for TutorWindow.xaml
    /// </summary>
    public partial class TutorWindow : Window
    {
        private Tutor tutor;
        public TutorWindow(Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
        }

        public void TutorWindow_Load(object sender, RoutedEventArgs e)
        {
            WriteWelcome(tutor);
        }

        public void WriteWelcome(Tutor tutor)
        {
            lblWelcome.Content = "Welcome " + tutor.Name + " " + tutor.Surname + "!";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExamDisplayWindow manageExamWindow = new ExamDisplayWindow(tutor);
            manageExamWindow.Show();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TutorsCoursesWindow tutorsCoursesWindow = new TutorsCoursesWindow(tutor);
            tutorsCoursesWindow.Show();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Hide();
        }

        private void Button_Start_Course_Click(object sender, RoutedEventArgs e)
        {
            CourseStartWindow startCourseWindow = new CourseStartWindow(tutor);
            startCourseWindow.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            StartExamWindow startExamWindow = new StartExamWindow(tutor);
            startExamWindow.ShowDialog();
        }


        private void Button_Ongoing_Course_Click(object sender, RoutedEventArgs e)
        {
            CoursesOngoingWindow ongoingCoursesWindow = new CoursesOngoingWindow(tutor);
            ongoingCoursesWindow.Show();
        }

        private void Button_Cancellation_Request_Click(object sender, RoutedEventArgs e)
        {
            CourseCancellationRequestsWindow courseCancellationRequestsWindow = new CourseCancellationRequestsWindow(tutor);
            courseCancellationRequestsWindow.Show();
        }


        private void Button_Finished_Course_Click(object sender, RoutedEventArgs e)
        {
            CourseFinishedTutorWindow finishedCourse = new CourseFinishedTutorWindow(tutor);
            finishedCourse.Show();
        }

        private void Button_Ongoing_Exam_Click(object sender, RoutedEventArgs e)
        {
            ExamsOngoingWindow examsOngoingWindow = new ExamsOngoingWindow(tutor);
            examsOngoingWindow.Show();
        }

        private void Button_Finished_Exam_Click(object sender, RoutedEventArgs e)
        {
            ExamFinishedTutorWindow finishedExam = new ExamFinishedTutorWindow(tutor);
            finishedExam.Show();
        }
    }
}
