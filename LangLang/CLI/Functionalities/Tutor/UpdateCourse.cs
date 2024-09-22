using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Tutor
{
    public class UpdateCourse
    {
        CourseController courseController;

        public UpdateCourse()
        {
            courseController = new CourseController();
        }

        public void Run(int tutorId, int courseId)
        {
            LanguageEnum.Language language = GetLanguage();
            LanguageLevelEnum.LanguageLevel languageLevel = GetLanguageLevel();
            int weeks = GetDuration();
            DateTime start = GetStartDate();
            List<DaysEnum.Days> days = GetDays();
            RealizationEnum.Realization realization = GetRealization();
            int students = GetMaxNumberOfStudents(realization);

            Course course = new Course(courseId, language, languageLevel, weeks, days, start, realization, students, tutorId, 0);
            courseController.Update(course);

            Console.WriteLine("Course updated successfully!");
        }

        private LanguageEnum.Language GetLanguage()
        {
            Console.WriteLine("Select Language:");
            foreach (LanguageEnum.Language language in Enum.GetValues(typeof(LanguageEnum.Language)))
            {
                Console.WriteLine($"{(int)language} - {language}");
            }
            int choice = GetIntegerInput("Enter your choice:");
            return (LanguageEnum.Language)choice;
        }

        private LanguageLevelEnum.LanguageLevel GetLanguageLevel()
        {
            Console.WriteLine("Select Language Level:");
            foreach (LanguageLevelEnum.LanguageLevel level in Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel)))
            {
                Console.WriteLine($"{(int)level} - {level}");
            }
            int choice = GetIntegerInput("Enter your choice:");
            return (LanguageLevelEnum.LanguageLevel)choice;
        }

        private int GetDuration()
        {
            return GetIntegerInput("Enter Duration (in weeks):");
        }

        private DateTime GetStartDate()
        {
            while (true)
            {
                Console.WriteLine("Enter Start Date (yyyy-mm-dd):");
                string input = Console.ReadLine();
                if (!DateTime.TryParse(input, out DateTime result))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date in yyyy-mm-dd format.");
                }
                if (result > DateTime.Today)
                {
                    return result;
                }
                return result;
            }
        }

        private List<DaysEnum.Days> GetDays()
        {
            List<DaysEnum.Days> days = new List<DaysEnum.Days>();

            while (true)
            {
                Console.WriteLine("Select Days (separate by comma, e.g., 1,3,5):");
                foreach (DaysEnum.Days day in Enum.GetValues(typeof(DaysEnum.Days)))
                {
                    Console.WriteLine($"{(int)day} - {day}");
                }
                string input = Console.ReadLine();
                string[] choices = input.Split(',');

                days.Clear();

                bool allValid = true;

                foreach (string choice in choices)
                {
                    if (!(int.TryParse(choice, out int dayInt) && Enum.IsDefined(typeof(DaysEnum.Days), dayInt)))
                    {
                        Console.WriteLine($"Invalid day option: {choice}. Please enter valid day options.");
                        allValid = false;
                        break;
                    }
                    days.Add((DaysEnum.Days)dayInt);
                }

                if (allValid)
                {
                    break;
                }
            }

            return days;
        }

        private RealizationEnum.Realization GetRealization()
        {
            Console.WriteLine("Select Realization:");
            foreach (RealizationEnum.Realization realization in Enum.GetValues(typeof(RealizationEnum.Realization)))
            {
                Console.WriteLine($"{(int)realization} - {realization}");
            }
            int choice = GetIntegerInput("Enter your choice:");
            return (RealizationEnum.Realization)choice;
        }

        private int GetMaxNumberOfStudents(RealizationEnum.Realization realization)
        {
            if (realization == RealizationEnum.Realization.live)
            {
                Console.WriteLine("Enter Max Number of Students:");
                return GetIntegerInput("Enter your choice:");
            }
            return 0;
        }

        private int GetIntegerInput(string prompt)
        {
            while (true)
            {
                Console.WriteLine(prompt);
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int result))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                }
                return result;
            }
        }
    }
}
