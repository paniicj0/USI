using LangLang.Controllers;
using LangLang.Model;
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
    public partial class StudentWindow : Window
    {
        private Student student;
        public StudentUpdateWindow studentUpdateWindow;
        public CoursesDisplayForStudent coursesDisplayForStudent;
        public ExamsDisplayForStudent examsDisplayForStudent;
        public StudentAppliedCoursesWindow studentAppliedCoursesWindow;
        public NotificationsForStudentWindow notificationsForStudentWindow;
        public CoursesCompletedWindow coursesCompletedWindow;
        public NotificationController notificationController;
        public int unreadMessages;

        public StudentWindow(Student student)
        {
            InitializeComponent();
            this.student = student;
            notificationController = new NotificationController();
        }

        public void SetNotificationCount()
        {
            notificationsForStudentWindow = new NotificationsForStudentWindow(student);
            int unreads = notificationController.CountUnreadMessages(student.Id);
            lblUnread.Content = "Unread: " + unreads;
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            SetNotificationCount();
        }

        public void StudentWindow_Load(object sender, RoutedEventArgs e)
        {
            WriteWelcome(student);
            SetNotificationCount();
        }

        public void WriteWelcome(Student student)
        {
            lblWelcome.Content = "Welcome " + student.Name + " " + student.Surname + "!";
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (student.RegisteredCourses.All(courseId => courseId != -1) || student.RegisteredExams.All(examId => examId != -1))
            {
                MessageBox.Show("Data modification prohibited");
                return;
            }

            studentUpdateWindow = new StudentUpdateWindow(student, this);
            studentUpdateWindow.Show();
            this.Hide();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            LogInWindow logInWindow = new LogInWindow();
            logInWindow.Show();
            this.Hide();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            StudentDeleteWindow studentDeleteWindow = new StudentDeleteWindow(student);
            studentDeleteWindow.Show();
        }

        private void btnCourses_Click(object sender, RoutedEventArgs e)
        {
            coursesDisplayForStudent = new CoursesDisplayForStudent(student);
            coursesDisplayForStudent.Show();
            this.Hide();
        }

        private void btnExam_Click(object sender, RoutedEventArgs e)
        {
            examsDisplayForStudent = new ExamsDisplayForStudent(student);
            examsDisplayForStudent.Show();
            this.Hide();
        }

        private void btnAppliedCourses_Click(object sender, RoutedEventArgs e)
        {
            studentAppliedCoursesWindow = new StudentAppliedCoursesWindow(student);
            studentAppliedCoursesWindow.Show();
            this.Hide();
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            notificationsForStudentWindow.Show();
            this.Hide();
        }

        private void btnCompletedCourses_Click(object sender, RoutedEventArgs e)
        {
            coursesCompletedWindow = new CoursesCompletedWindow(student);
            coursesCompletedWindow.Show();
            this.Hide();
        }
    }
}

