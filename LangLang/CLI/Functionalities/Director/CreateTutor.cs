using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Director
{
    internal class CreateTutor
    {
        TutorController tutorController;

        public CreateTutor()
        {
            tutorController = new TutorController();
        }

        public void Run(){
            Console.WriteLine("Enter Tutor details:");

            Console.Write("Name: ");
                string name = Console.ReadLine();

            Console.Write("Surname: ");
                string surname = Console.ReadLine();

            Console.WriteLine("Select Gender:");
                foreach (GenderEnum.Gender gender in Enum.GetValues(typeof(GenderEnum.Gender)))
                {
                    Console.WriteLine($"{(int)gender} - {gender}");
                }
            GenderEnum.Gender selectedGender = (GenderEnum.Gender)int.Parse(Console.ReadLine());

            Console.Write("Birthday (yyyy-MM-dd): ");
                    DateTime birthday = DateTime.Parse(Console.ReadLine());

            Console.Write("Phone Number: ");
                    int phoneNumber = int.Parse(Console.ReadLine());

            Console.Write("Email: ");
                    string email = Console.ReadLine();

            Console.Write("Password: ");
                    string password = Console.ReadLine();

            Console.WriteLine("Select Language:");
                    foreach (LanguageEnum.Language language in Enum.GetValues(typeof(LanguageEnum.Language)))
                {
                    Console.WriteLine($"{(int)language} - {language}");
                }
            LanguageEnum.Language selectedLanguage = (LanguageEnum.Language)int.Parse(Console.ReadLine());

            Console.WriteLine("Select Language Level:");
            foreach (LanguageLevelEnum.LanguageLevel languageLevel in Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel)))
            {
                Console.WriteLine($"{(int)languageLevel} - {languageLevel}");
            }
            LanguageLevelEnum.LanguageLevel selectedLanguageLevel = (LanguageLevelEnum.LanguageLevel)int.Parse(Console.ReadLine());

            DateTime tutorCreationDate = DateTime.Now;

            Model.Tutor tutor = new Model.Tutor(-1, name, surname, selectedGender, birthday, phoneNumber, email, password, selectedLanguage, selectedLanguageLevel, tutorCreationDate);
            tutorController.Create(tutor);

            Console.WriteLine("Successfully added the tutor.");

        }
    }
}