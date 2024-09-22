using LangLang.Controllers;
using LangLang.RepositorySQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Director
{
    internal class DisplayTutors
    {
        private TutorController tutorController;

        public TutorRepositorySQL tutorRepositorySQL = new TutorRepositorySQL();
        public DisplayTutors()
        {
            tutorController = new TutorController();
        }

        public void Run()
        {
            while (true)
            {
                DisplayTutorsTable();
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Update a tutor");
                Console.WriteLine("2. Delete a tutor");
                Console.WriteLine("3. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UpdateTutor();
                        break;
                    case "2":
                        DeleteTutor();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        private void DisplayTutorsTable()
        {
            List<Model.Tutor> tutors = tutorRepositorySQL.GetAll();
            if (tutors.Count == 0)
            {
                Console.WriteLine("The list of tutors is empty.");
                return;
            }

            Console.WriteLine("Tutors:");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("ID    | Name           | Surname        | Gender   | Birthday   | Phone Number    | Email                     | Language     | Language Level  |Date Created|");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------");

            foreach (Model.Tutor tutor in tutors)
            {
                Console.WriteLine($"{tutor.Id,-5} | {tutor.Name,-14} | {tutor.Surname,-14} | {tutor.Gender,-8} | {tutor.Birthday:yyyy-MM-dd} | {tutor.PhoneNumber,-15} | {tutor.Email,-25} | {tutor.Languages,-12} | {tutor.LanguageLevel,-15} | {tutor.TutorCreationDate:yyyy-MM-dd} |");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------------------------------------------------------");
            }
        }


        private void UpdateTutor()
        {
            Console.Write("Enter the ID of the tutor to update: ");
            if (int.TryParse(Console.ReadLine(), out int tutorId))
            {
                Model.Tutor selectedTutor = tutorController.GetById(tutorId);
                if (selectedTutor != null)
                {
                    UpdateTutor updateCLI = new UpdateTutor(selectedTutor);
                    updateCLI.Run();
                }
                else
                {
                    Console.WriteLine("Tutor not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }

        private void DeleteTutor()
        {
            Console.Write("Enter the ID of the tutor to delete: ");
            if (int.TryParse(Console.ReadLine(), out int tutorId))
            {
                Model.Tutor selectedTutor = tutorController.GetById(tutorId);
                if (selectedTutor != null)
                {
                    tutorController.Delete(tutorId);
                    Console.WriteLine("Successfully deleted the tutor.");
                }
                else
                {
                    Console.WriteLine("Tutor not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
        }
    }
}
