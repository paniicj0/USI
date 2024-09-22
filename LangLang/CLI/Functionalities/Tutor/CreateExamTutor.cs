using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;

namespace LangLang.CLI.Functionalities.Tutor
{
    public class CreateExamTutor
    {
        ExamController examController;

        public CreateExamTutor() 
        {
            examController = new ExamController();
        }
        public void Run(int tutorId)
        {
            Console.WriteLine("Create new exam:");

            LanguageEnum.Language language = GetLanguage();
            LanguageLevelEnum.LanguageLevel languageLevel = GetLanguageLevel();
            int numOfStudents = GetNumOfStudents();
            DateTime examDate = GetExamDate();
            TimeSpan examTime = GetExamTime();
            int examDuration = GetExamDuration();

            Exam exam = new Exam(-1, language, languageLevel, numOfStudents, examDate, examTime, examDuration, tutorId, 0, false);
            examController.Create(exam);

            Console.WriteLine("Exam created successfully!");

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

        private int GetExamDuration()
        {
            return GetIntegerInput("Enter Duration:");
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

        private int GetNumOfStudents()
        {
            return GetIntegerInput("Enter the number of students:");
        }

        private DateTime GetExamDate()
        {
            while (true)
            {
                Console.WriteLine("Enter Exam Date (YYYY-MM-DD):");
                string input = Console.ReadLine();
                if (DateTime.TryParse(input, out DateTime result))
                {
                    return result;
                }
                Console.WriteLine("Invalid input. Please enter a valid date.");
            }
        }

        private TimeSpan GetExamTime()
        {
            while (true)
            {
                Console.WriteLine("Enter Exam Time (HH:MM):");
                string input = Console.ReadLine();
                if (TimeSpan.TryParse(input, out TimeSpan result))
                {
                    return result;
                }
                Console.WriteLine("Invalid input. Please enter a valid time.");
            }
        }


    }
}
