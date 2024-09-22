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
    internal class CreateCourseDirector
    {
        private Model.Director director;
        private CourseController courseController;
        private TutorController tutorController;
        private List<Model.Tutor> tutors;
        private List<Course> courses;
        private Dictionary<string, int> tutorNameToIdMap;

        public CreateCourseDirector(Model.Director director)
        {
            this.director = director;
            courseController = new CourseController();
            tutorController = new TutorController();
            tutors = tutorController.GetAll();
            courses = courseController.GetAll();
            tutorNameToIdMap = new Dictionary<string, int>();
            Console.WriteLine("Tutors:");
            foreach (var tutor in tutors)
            {
                Console.WriteLine($"{tutor.Name} {tutor.Surname} - {tutor.Languages} - {tutor.LanguageLevel}");
            }
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Create a new course");
                Console.WriteLine("2. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCourse();
                        break;
                    case "2":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        private void AddCourse()
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

            Console.WriteLine("Enter duration in weeks:");
            int duration = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter start date (yyyy-MM-dd):");
            DateTime startDate = DateTime.Parse(Console.ReadLine());

            Console.WriteLine("Select days:");
            var days = Enum.GetValues(typeof(DaysEnum.Days)).Cast<DaysEnum.Days>().ToList();
            for (int i = 0; i < days.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {days[i]}");
            }
            List<DaysEnum.Days> selectedDays = new List<DaysEnum.Days>();
            string dayChoice;
            while ((dayChoice = Console.ReadLine()) != string.Empty)
            {
                selectedDays.Add(days[int.Parse(dayChoice) - 1]);
            }

            Console.WriteLine("Select realization (1. live, 2. online):");
            RealizationEnum.Realization realization = (RealizationEnum.Realization)(int.Parse(Console.ReadLine()) - 1);

            int maxStudents = 0;
            if (realization == RealizationEnum.Realization.live)
            {
                Console.WriteLine("Enter maximum number of students:");
                maxStudents = int.Parse(Console.ReadLine());
            }

            UpdateAvailableTutors(selectedLanguage, selectedLevel, duration, startDate);
            Console.WriteLine("Select tutor:");
            var availableTutors = tutorNameToIdMap.Keys.ToList();
            for (int i = 0; i < availableTutors.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableTutors[i]}");
            }
            int tutorChoice = int.Parse(Console.ReadLine()) - 1;
            string selectedTutorName = availableTutors[tutorChoice];
            int tutorId = tutorNameToIdMap[selectedTutorName];

            Course course = new Course(-1, selectedLanguage, selectedLevel, duration, selectedDays, startDate, realization, maxStudents, tutorId, 0);
            courseController.Create(course);
            Console.WriteLine("Course created successfully.");
        }

        private void UpdateAvailableTutors(LanguageEnum.Language selectedLanguage, LanguageLevelEnum.LanguageLevel selectedLevel, int duration, DateTime startDate)
        {
            var competentTutors = tutors.Where(tutor =>
                tutor.Languages.HasFlag(selectedLanguage) &&
                tutor.LanguageLevel == selectedLevel).ToList();

            var availableTutors = competentTutors.Where(tutor =>
                IsTutorAvailable(tutor, startDate, duration)).ToList();

            tutorNameToIdMap.Clear();
            foreach (var tutor in availableTutors)
            {
                var tutorFullName = $"{tutor.Name} {tutor.Surname}";
                tutorNameToIdMap[tutorFullName] = tutor.Id;
            }

            Console.WriteLine("Available tutors:");
            foreach (var tutor in availableTutors)
            {
                Console.WriteLine($"{tutor.Name} {tutor.Surname}");
            }
        }

        private bool IsTutorAvailable(Model.Tutor tutor, DateTime startDate, int duration)
        {
            var endDate = startDate.AddDays(duration * 7);
            var tutorCourses = courses.Where(course => course.TutorId == tutor.Id);

            foreach (var course in tutorCourses)
            {
                var courseEndDate = course.Start.AddDays(course.Duration * 7);
                var overlap = !(endDate <= course.Start || startDate >= courseEndDate);

                if (overlap)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
