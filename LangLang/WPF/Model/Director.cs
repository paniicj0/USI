using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Model
{
    public class Director
    {
        private int id;
        private string name;
        private string surname;
        private GenderEnum.Gender gender;
        private DateTime birthday;
        private int phoneNumber;
        private string email;
        private string password;

        public Director(int id, string name, string surname, GenderEnum.Gender gender, DateTime birthday, int phoneNumber, string email, string password)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.gender = gender;
            this.birthday = birthday;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.password = password;

        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        public GenderEnum.Gender Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        public DateTime Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }
        public int PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public String toString()
        {
            return name + " " + surname;
        }
        public String StringToCsv()
        {
            return id + "|" + name + "|" + surname + "|" + gender + "|" + birthday.ToString("yyyy-MM-dd") + "|" + phoneNumber + "|" + email + "|"
                    + password + "|";
        }
    }
}