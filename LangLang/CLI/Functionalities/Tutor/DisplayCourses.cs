using LangLang.Controllers;
using LangLang.Model;
using LangLang.WPF.RepositorySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Tutor
{
    public class DisplayCourses
    {
        private CourseController courseController;
        private CreateCourseTutor createCourse;
        private UpdateCourse updateCourse;
        private DeleteCourse deleteCourse;

        public DisplayCourses()
        {
            courseController = new CourseController();
            createCourse = new CreateCourseTutor();
            updateCourse = new UpdateCourse();
            deleteCourse = new DeleteCourse();
        }

        public void Run(int tutorId)
        {
            while (true)
            {
                DisplayCoursesTable(tutorId);
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create course");
                Console.WriteLine("2. Update course");
                Console.WriteLine("3. Delete course");
                Console.WriteLine("4. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateCourse(tutorId);
                        break;
                    case "2":
                        UpdateCourse();
                        break;
                    case "3":
                        DeleteCourse();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        private void DisplayCoursesTable(int tutorId)
        {
            List<Course> courses = courseController.GetAll();
            if (courses.Count == 0)
            {
                Console.WriteLine("The list of courses is empty.");
                return;
            }

            Console.WriteLine("Courses:");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("ID    | Language  | Language Level | Duration | Days of Week                | Start Date | Realization  | Max Students | Number of Students |"); 
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");

            foreach (Course course in courses)
            {
                if (course.TutorId == tutorId)
                {
                    string daysString = string.Join(",", course.Days.Select(d => d.ToString()));
                    Console.WriteLine($"{course.Id,-5} | {course.Language,-9} | {course.LanguageLevel,-14} | {course.Duration,-8} | {daysString,-27} | {course.Start:yyyy-MM-dd} | {course.Realization,-12} | {course.MaxStudents,-12} | {course.NumberOfStudents,-18} |");
                    Console.WriteLine("---------------------------------------------------------------------------------------------------------------------------------------------");
                }
            }
        }

        private void CreateCourse(int tutorId)
        {
            createCourse.Run(tutorId);
        }

        private void UpdateCourse()
        {
            Console.Write("Enter the ID of the course to update: ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                Course selectedCourse = courseController.GetById(courseId);
                if (selectedCourse != null)
                {
                    updateCourse.Run(selectedCourse.TutorId, selectedCourse.Id);
                }
                else
                {
                    Console.WriteLine("Course not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void DeleteCourse()
        {
            Console.Write("Enter the ID of the course to delete: ");
            if (int.TryParse(Console.ReadLine(), out int courseId))
            {
                deleteCourse.Run(courseId);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }
    }
}
