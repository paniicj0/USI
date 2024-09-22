using LangLang.Controllers;
using LangLang.Model;
using LangLang.Repository;
using LangLang.Service;
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
using USIProject.View;

namespace LangLang.View
{
    public partial class LogInWindow : Window
    {
        public List<User> users;
        public List<Student> students;
        public static StudentWindow studentWindow;
        public static TutorWindow tutorWindow;
        public static DirectorWindow directorWindow;
        public UserService userService;
        private UserController userController = new UserController();
        private StudentController studentController = new StudentController();
        private TutorController tutorController = new TutorController();
        private DirectorController directorController = new DirectorController();
        private AppliedExam appliedExam;
        private StudentRepository studentRepository;

        public LogInWindow()
        {
            InitializeComponent();
            Closing += Window_Closing;

            userService = UserService.GetInstance();
            StudentService studentService = StudentService.GetInstance();
            users = userService.GetAll();
            students = studentService.GetAll();
            studentRepository = StudentRepository.GetInstance();
            removePenaltyPoints();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            Student student = null;
            Tutor tutor = null;
            Director director = null;

            string email = tbEmail.Text;
            string password = tbPassword.Password;

            if (email != "" && password != "")
            {
                (object user, string role) = userController.IsLoggedIn(email, password);
                if (user != null)
                {
                    switch (role)
                    {
                        case "Student":
                            student = userController.ConvertToUserType<Student>(user);
                            studentWindow = new StudentWindow(student);
                            lblLogIn.Content = "";
                            studentWindow.Show();
                            this.Hide();
                            tbClear();
                            break;
                        case "Tutor":
                            tutor = userController.ConvertToUserType<Tutor>(user);
                            tutorWindow = new TutorWindow(tutor);
                            lblLogIn.Content = "";
                            tutorWindow.Show();
                            this.Hide();
                            tbClear();
                            break;
                        case "Director":
                            director = userController.ConvertToUserType<Director>(user);
                            directorWindow = new DirectorWindow(director);
                            lblLogIn.Content = "";
                            directorWindow.Show();
                            this.Hide();
                            tbClear();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    lblLogIn.Content = "Non-existent user. Please try again.";
                    tbClear();
                }
            }
            else
            {
                lblLogIn.Content = "Please enter email and password!";
            }
        }

        private void btnSignUp_Click(object sender, RoutedEventArgs e)
        {
            {
                SignUpWindow signUpWindow = new SignUpWindow();
                signUpWindow.Show();
                this.Hide();
            }
        }

        private void tbClear()
        {
            tbEmail.Text = "";
            tbPassword.Password = "";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!e.Cancel)
            {
                Application.Current.Shutdown();
            }
        }

        //add function for penalty points here
        private void removePenaltyPoints()
        {
            foreach (Student student in students)
            {
                if (student.PenaltyPoints > 0)
                {
                    if (student.PenaltyPoints >= 3)
                    {
                        if (appliedExam != null)
                        {
                            appliedExam.Banned = true;
                        }
                        studentRepository.Delete(student.Id);                   
                    }
                    else
                    {
                        if (DateTime.Today.Day == 1)
                        {
                            student.PenaltyPoints--;
                        }
                    }
                }
            }
        }

    }
}

