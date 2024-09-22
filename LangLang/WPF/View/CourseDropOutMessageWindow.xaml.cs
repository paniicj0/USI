using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
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
   
    public partial class CourseDropOutMessageWindow : Window
    {
        public int requestId;
        public int courseId;
        public StudentCourseCancellationController studentCourseCancellationController = new StudentCourseCancellationController();

        public Student student;
        StudentAppliedCoursesWindow studentAppliedCoursesWindow;
        public CourseDropOutMessageWindow(int requestId, int courseId, Student student)
        {
            InitializeComponent();
            this.student = student;
            this.requestId = requestId;
            this.courseId = courseId;
            studentAppliedCoursesWindow = new StudentAppliedCoursesWindow(student);
        }

        private void btnSumbit_Click(object sender, RoutedEventArgs e)
        {
            string message = tbMessage.Text;
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("You need to enter a reason for dropping out!");
                return;
            } else
            {
                StudentCourseCancellation studentCourseCancellation = new StudentCourseCancellation(requestId, courseId, student.Id, StatusEnum.Status.Pending, message);
                studentCourseCancellationController.Create(studentCourseCancellation);
                MessageBox.Show("Successfully sent request for dropping out!");
                this.Close();
            }
        }
    }
}
