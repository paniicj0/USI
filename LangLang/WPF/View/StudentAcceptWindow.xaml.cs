using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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
    /// Interaction logic for StudentAccept.xaml
    /// </summary>
    public partial class StudentAcceptWindow : Window
    {
        StudentCourseRequest request;
        StudentCourseRequestController requestController = new StudentCourseRequestController();
        NotificationController notificationController = new NotificationController();
        StudentController studentController = new StudentController();
        public StudentAcceptWindow(StudentCourseRequest request)
        {
            InitializeComponent();
            this.request = request;
        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            request.Status = StatusEnum.Status.Accepted;
            requestController.Update(request);
            Notification notification = new Notification(-1, request.IdStudent, request.IdCourse, false, "Cestitam! Primljeni ste na kurs!");
            notificationController.Create(notification);
            Student student = studentController.GetById(request.IdStudent);
            student.AppliedForCourse = true;
            student.RegisteredCourses.Add(request.IdCourse);
            studentController.Update(student);
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
