using LangLang.Controllers;
using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
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

    public partial class DirectorChoosePriorityForMail : Window
    {
        private AppliedExamRepository appliedExamRepository;
        private AppliedExamController appliedExamController;
        private StudentRepository studentRepository;
        private StudentController studentController;
        private List<Student> students;
        public int courseId;
        private CourseController courseController = new CourseController();
        public List<Notification> notifications;
        public NotificationController notificationController = new NotificationController();
        public DirectorChoosePriorityForMail(int courseId)
        {
            InitializeComponent();
            appliedExamRepository = AppliedExamRepository.GetInstance();
            studentRepository = StudentRepository.GetInstance();
            this.students = studentRepository.GetAll();
            this.courseId = courseId;
        }

        private void KnowledgeBtn_Click(object sender, RoutedEventArgs e)
        {
            appliedExamController = new AppliedExamController();
            List<Student> bestStudents = appliedExamController.GetBestStudentsKnowledge(courseId);
            foreach (Student student in bestStudents)
            {
                SendThankYouMail(student);
            }
            MessageBox.Show("Mails successfully sent!");
        }

        private void ActivityBtn_Click(object sender, RoutedEventArgs e)
        {
            appliedExamController = new AppliedExamController();
            List<Student> bestStudents = appliedExamController.GetBestStudentsActivity(courseId);
            foreach (Student student in bestStudents)
            {
                SendThankYouMail(student);
            }
            MessageBox.Show("Mails successfully sent!");
        }

        private void SendThankYouMail(Student student)
        {
            Course course = courseController.GetById(courseId);
            try
            {
                using (var client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.Credentials = new NetworkCredential("testic.usii@gmail.com", "cgoxbbyhtduliywv");
                    client.EnableSsl = true;

                    string message = "Congratulations for getting the best grade in course " + course.Language + " " + course.LanguageLevel + "!";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("testic.usii@gmail.com"),
                        Subject = "Thank you card",
                        Body = message,
                    };
                    mailMessage.To.Add(student.Email);

                    client.Send(mailMessage);

                    Notification notification = new Notification(-1, student.Id, course.Id, false, message);
                    notificationController.Create(notification);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while sending the email: " + ex.Message);
            }
        }
    }
}
