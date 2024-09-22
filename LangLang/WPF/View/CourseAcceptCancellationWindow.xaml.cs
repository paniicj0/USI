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
    /// <summary>
    /// Interaction logic for AcceptCourseCancellationWindow.xaml
    /// </summary>
    public partial class CourseAcceptCancellationWindow : Window
    {
        private Tutor tutor;
        private int requestId;

        private NotificationController notificationController = new NotificationController();
        private StudentCourseCancellationController cancellationController = new StudentCourseCancellationController();
        private List<StudentCourseCancellation> cancellationRequests;
        private StudentController studentController = new StudentController();
        private List<Student> students;
        private StudentCourseRequestController requestController = new StudentCourseRequestController();
        List<StudentCourseRequest> requests;

        public CourseAcceptCancellationWindow(Tutor tutor, int requestId)
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
                
            cancel.Status = StatusEnum.Status.Accepted;
            cancellationController.Update(cancel);

            StudentCourseRequest list = requestController.GetByCourseStudentId(cancel.IdCourse, cancel.IdStudent);

            Student student = studentController.GetById(list.IdStudent);

            foreach (StudentCourseRequest request in requests)
            {
                if (request.IdStudent == student.Id)
                {
                    if (request.Status == StatusEnum.Status.Pending)
                    {
                        request.Status = StatusEnum.Status.Waiting;
                    }
                }
            }

            if ( list.Status == StatusEnum.Status.Accepted)
            {
                                    
                list.Status = StatusEnum.Status.Expelled;
                requestController.Update(list);

                student.AppliedForCourse = false;
                student.RegisteredCourses.Remove(cancel.IdCourse);
                studentController.Update(student);

                foreach (StudentCourseRequest request in requests)
                {
                    if (request.IdStudent == student.Id)
                    {
                        if (request.Status == StatusEnum.Status.Waiting)
                        {
                            request.Status = StatusEnum.Status.Pending;
                        }
                    }
                }
            }

            Notification notification = new Notification(-1, cancel.IdStudent, cancel.IdCourse, false, "Your request to withdraw from the course has been accepted.");
            notificationController.Create(notification);
                    
            this.Close();
        }

        private void Button_Click_No(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
