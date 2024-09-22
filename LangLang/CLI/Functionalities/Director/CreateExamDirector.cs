using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Director
{
    internal class CreateExamDirector
    {
        private List<Course> courses;
        private ExamController examController;
        private CourseController courseController;
        private List<Model.Tutor> tutors;

        public CreateExamDirector()
        {
            courseController = new CourseController();
            examController = new ExamController();
            courses = courseController.GetAll();
            tutors = new TutorController().GetAll();

        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create a new exam");
                Console.WriteLine("2. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddExam();
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        private void AddExam()
        {
            Console.WriteLine("Select language:");
            var languages = Enum.GetValues(typeof(LanguageEnum.Language)).Cast<LanguageEnum.Language>().ToList();
            for (int i = 0; i < languages.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {languages[i]}");
            }
            int languageChoice = int.Parse(Console.ReadLine()) - 1;
            LanguageEnum.Language selectedLanguage = languages[languageChoice];

            Console.WriteLine("Select language level:");
            var levels = Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel)).Cast<LanguageLevelEnum.LanguageLevel>().ToList();
            for (int i = 0; i < levels.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {levels[i]}");
            }
            int levelChoice = int.Parse(Console.ReadLine()) - 1;
            LanguageLevelEnum.LanguageLevel selectedLevel = levels[levelChoice];

            Console.WriteLine("Enter number of students:");
            int students = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter exam date (yyyy-MM-dd):");
            DateTime examDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Enter exam time (HH:mm):");
            TimeSpan examTime;
            while (!TimeSpan.TryParse(Console.ReadLine(), out examTime))
            {
                Console.WriteLine("Invalid time format. Please enter exam time (HH:mm):");
            }

            int tutorId = examController.SmartChoiceTutorForExam(selectedLanguage, selectedLevel, examDate, examTime);
            if (tutorId == -1)
            {
                Console.WriteLine("There is no available tutors for this exam.");
                return;
            }

            Exam exam = new Exam(-1, selectedLanguage, selectedLevel, students, examDate, examTime, 4, tutorId, 0);
            examController.Create(exam);
            Console.WriteLine("Exam created successfully.");
        }
    }
}
