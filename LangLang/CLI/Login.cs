using LangLang.CLI.Functionalities.Tutor;
using LangLang.CLI.Functionalities.Director;
using LangLang.Controllers;
using LangLang.Model;
using LangLang.Service;
using LangLang.View;
using LangLang.WPF.RepositorySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using USIProject.View;

namespace LangLang.CLI
{
    public class Login
    {
        public UserController userController = new UserController();
        public Student student;
        public LangLang.Model.Tutor tutor;
        public Director director;

        public CreateCourseTutor createCourse = new CreateCourseTutor();
        public UpdateCourse updateCourse = new UpdateCourse();
        public DisplayCourses displayCourses = new DisplayCourses();
        public CreateExamTutor createExam = new CreateExamTutor();
        public UpdateExam updateExam = new UpdateExam();
        public DisplayExams displayExams = new DisplayExams();

        public void Startup()
        {
            ShowWelcomeText();
            EnterLoginInformation();
        }

        public void ShowWelcomeText()
        {
            Console.WriteLine("Welcome to School Of Languages Application!");
            Console.WriteLine("Please login.");
        }

        public void EnterLoginInformation()
        {
            bool loggedIn = false;
            while (!loggedIn)
            {
                Console.Write("Enter your email address: ");
                string email = Console.ReadLine();

                Console.Write("Enter your email password: ");
                string password = Console.ReadLine();

                (object user, string role) = userController.IsLoggedIn(email, password);

                if (user != null)
                {
                    switch (role)
                    {
                        
                        case "Student":
                            student = userController.ConvertToUserType<Student>(user);
                            Console.WriteLine("There are no functionalities for student.");
                            break;
                        case "Tutor":
                            tutor = userController.ConvertToUserType<LangLang.Model.Tutor>(user);
                            loggedIn = true;
                            menuTutor(tutor.Id);
                            break;
                        case "Director":
                            director = userController.ConvertToUserType<Director>(user);
                            loggedIn = true;
                            menuDirector(director);
                            break;
                        default:
                            break;
                    }
                       
                }
                else
                {
                    Console.WriteLine("Non-existent user. Please try again.");
                }
            }
                        
        }

        public void menuTutor(int tutorId)
        {
            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Create a new course");
                Console.WriteLine("2. List all courses");
                Console.WriteLine("3. Create a new exam");
                Console.WriteLine("4. List all exams");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        createCourse.Run(tutorId);
                        break;
                    case "2":
                        displayCourses.Run(tutorId);
                        break;
                    case "3":
                        createExam.Run(tutorId);
                        break;
                    case "4":
                        displayExams.Run(tutorId);
                        break;
                    case "5":
                        Console.WriteLine("Exiting program...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 1 to 5.");
                        break;
                }
            }
        }

        public void menuDirector(Model.Director director)
        {
            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Create a new tutor");
                Console.WriteLine("2. List all tutors");
                Console.WriteLine("3. Create a new course");
                Console.WriteLine("4. Create a new exam");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateTutor add = new CreateTutor();
                        add.Run();
                        break;
                    case "2":
                        DisplayTutors display = new DisplayTutors();
                        display.Run();
                        break;
                    case "3":
                        CreateCourseDirector directorCreateCourse = new CreateCourseDirector(director);
                        directorCreateCourse.Run();
                        break;
                    case "4":
                        CreateExamDirector directorCreateExam = new CreateExamDirector();
                        directorCreateExam.Run();
                        break;
                    case "5":
                        Console.WriteLine("Exiting program...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a number from 1 to 5.");
                        break;
                }
            }
        }
    }
}
