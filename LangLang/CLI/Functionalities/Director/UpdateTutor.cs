using LangLang.Controllers;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.CLI.Functionalities.Director
{
    internal class UpdateTutor
    {
        private Model.Tutor tutor;
        private TutorController tutorController;

        public UpdateTutor(Model.Tutor tutor)
        {
            this.tutor = tutor;
            tutorController = new TutorController();
            Prefill();
        }

        public void Run()
        {
            Console.WriteLine("Update Tutor Details:");

            Console.WriteLine($"Current Name: {tutor.Name}");
            Console.Write("New Name (leave empty to keep current): ");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                tutor.Name = name;
            }

            Console.WriteLine($"Current Surname: {tutor.Surname}");
            Console.Write("New Surname (leave empty to keep current): ");
            string surname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(surname))
            {
                tutor.Surname = surname;
            }

            Console.WriteLine($"Current Gender: {tutor.Gender}");
            Console.WriteLine("Select Gender:");
            foreach (GenderEnum.Gender gender in Enum.GetValues(typeof(GenderEnum.Gender)))
            {
                Console.WriteLine($"{(int)gender} - {gender}");
            }
            Console.Write("New Gender (leave empty to keep current): ");
            string genderInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(genderInput) && int.TryParse(genderInput, out int genderValue))
            {
                tutor.Gender = (GenderEnum.Gender)genderValue;
            }

            Console.WriteLine($"Current Birthday: {tutor.Birthday:yyyy-MM-dd}");
            Console.Write("New Birthday (yyyy-MM-dd, leave empty to keep current): ");
            string birthdayInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(birthdayInput) && DateTime.TryParse(birthdayInput, out DateTime birthday))
            {
                tutor.Birthday = birthday;
            }

            Console.WriteLine($"Current Phone Number: {tutor.PhoneNumber}");
            Console.Write("New Phone Number (leave empty to keep current): ");
            string phoneNumberInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(phoneNumberInput) && int.TryParse(phoneNumberInput, out int phoneNumber))
            {
                tutor.PhoneNumber = phoneNumber;
            }

            Console.WriteLine($"Current Email: {tutor.Email}");
            Console.Write("New Email (leave empty to keep current): ");
            string email = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(email))
            {
                tutor.Email = email;
            }

            Console.WriteLine($"Current Password: {tutor.Password}");
            Console.Write("New Password (leave empty to keep current): ");
            string password = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(password))
            {
                tutor.Password = password;
            }

            tutorController.Update(tutor);

            Console.WriteLine("Successfully updated the tutor.");
        }

        private void Prefill()
        {
            if (tutor != null)
            {
                Console.WriteLine("Prefilling Tutor Details:");
                Console.WriteLine($"Name: {tutor.Name}");
                Console.WriteLine($"Surname: {tutor.Surname}");
                Console.WriteLine($"Gender: {tutor.Gender}");
                Console.WriteLine($"Birthday: {tutor.Birthday:yyyy-MM-dd}");
                Console.WriteLine($"Phone Number: {tutor.PhoneNumber}");
                Console.WriteLine($"Email: {tutor.Email}");
                Console.WriteLine($"Password: {tutor.Password}");
            }
        }
    }
}
