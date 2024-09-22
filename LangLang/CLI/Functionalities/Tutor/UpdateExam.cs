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
    public class UpdateExam
    {
        ExamController examController;

        public UpdateExam()
        {
            examController = new ExamController();
        }

        public void Run(int tutorId, int examId)
        {
            LanguageEnum.Language language = GetLanguage();
            LanguageLevelEnum.LanguageLevel languageLevel = GetLanguageLevel();
            int numOfStudents = GetNumOfStudents();
            DateTime examDate = GetExamDate();
            TimeSpan examTime = GetExamTime();
            int examDuration = GetExamDuration();
            int numberOfAppliedStudents = GetNumberOfAppliedStudents();
            bool confirmed = GetConfirmationStatus();

            Exam exam = new Exam(examId, language, languageLevel, numOfStudents, examDate, examTime, examDuration, tutorId, numberOfAppliedStudents, confirmed);
            examController.Update(exam);

            Console.WriteLine("Exam updated successfully!");
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

        private int GetNumberOfAppliedStudents()
        {
            return GetIntegerInput("Enter the number of applied students:");
        }

        private bool GetConfirmationStatus()
        {
            while (true)
            {
                Console.WriteLine("Is the exam confirmed? (yes/no):");
                string input = Console.ReadLine().ToLower();
                if (input == "yes")
                {
                    return true;
                }
                else if (input == "no")
                {
                    return false;
                }
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }
        }
    }
}
