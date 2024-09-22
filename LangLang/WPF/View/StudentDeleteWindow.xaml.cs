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
    public partial class StudentDeleteWindow : Window
    {
        private Student student;
        private StudentController studentController = new StudentController();
        private StudentWindow studentWindow = LogInWindow.studentWindow;
        private LogInWindow logInWindow = new LogInWindow();
        private StudentExamCancellationController studentExamCancellationController = new StudentExamCancellationController();
        private StudentCourseCancellationController studentCourseCancellationController = new StudentCourseCancellationController();
        private StudentCourseRequestController studentCourseRequestController = new StudentCourseRequestController();
        public StudentDeleteWindow(Student student)
        {
            InitializeComponent();
            this.student = student;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            studentExamCancellationController.DeleteByStudentId(student.Id);
            studentCourseCancellationController.DeleteByStudentId(student.Id);
            studentCourseRequestController.DeleteByStudentId(student.Id);
            studentController.Delete(student.Id);
            studentWindow.Hide();
            logInWindow.Show();
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            studentWindow.Show();
        }
    }
}
