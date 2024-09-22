using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
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
    public partial class CourseDeclineCancellationWindow : Window
    {
        private Tutor tutor;
        private int requestId;
        private StudentCourseCancellationController cancellationController = new StudentCourseCancellationController();
        private StudentController studentController = new StudentController();
        private NotificationController notificationController = new NotificationController();
        private List<StudentCourseCancellation> cancellationRequests;
        private List<Student> students;

        private StudentCourseRequestController requestController = new StudentCourseRequestController();
        List<StudentCourseRequest> requests;

        public CourseDeclineCancellationWindow(Tutor tutor, int requestId)
        {
            InitializeComponent();
            this.tutor = tutor;
            this.requestId = requestId;
            this.cancellationRequests = cancellationController.GetAll();
            this.students = studentController.GetAll();
            this.requests = requestController.GetAll();
        }

        private void Button_Click_Yes(object sender, RoutedEventArgs e)
        {
            StudentCourseCancellation cancel = cancellationController.GetById(requestId);
            cancel.Status = StatusEnum.Status.Declined;
            cancellationController.Update(cancel);

            Student student = studentController.GetById(cancel.IdStudent);
                        
            student.PenaltyPoints += 1;
            studentController.Update(student);

            StudentCourseRequest request = requestController.GetByCourseStudentId(cancel.IdCourse, cancel.IdStudent);

            if (request.Status == StatusEnum.Status.Accepted)
            {
                request.Status = StatusEnum.Status.Expelled;
                requestController.Update(request);
                student.AppliedForCourse = false;
                student.RegisteredCourses.Remove(cancel.IdCourse);
                studentController.Update(student);

            }               
                        
            Notification notification = new Notification(-1, cancel.IdStudent, cancel.IdCourse, false, "Your request to withdraw from the course has been denied. You have received 1 penalty point!");
            notificationController.Create(notification);
                    
            this.Close();

        }

        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
