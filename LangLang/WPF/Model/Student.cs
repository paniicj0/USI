using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LangLang.ModelEnum.StudentAppliedOnCourseEnum;

namespace LangLang.Model
{
    public class Student
    {
        private int id;
        private string name;
        private string surname;
        private GenderEnum.Gender gender;
        private DateTime birthday;
        private int phoneNumber;
        private string email;
        private string password;
        private StudentEnum.Profession profession;
        private List<int> registeredCourses;
        private List<int> registeredExams;
        private int penaltyPoints;
        private bool appliedForCourse;

        public Student(int id, string name, string surname, GenderEnum.Gender gender,
                DateTime birthday, int phoneNumber, string email, string password,
                StudentEnum.Profession profession, List<int> registeredCourses, List<int> registeredExams, int penaltyPoints,bool appliedForCourse)
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.gender = gender;
            this.birthday = birthday;
            this.phoneNumber = phoneNumber;
            this.email = email;
            this.password = password;
            this.profession = profession;
            this.registeredCourses = registeredCourses;
            this.registeredExams = registeredExams;
            this.penaltyPoints = penaltyPoints;
            this.appliedForCourse = appliedForCourse;
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
        public StudentEnum.Profession Profession
        {
            get { return profession; }
            set { profession = value; }
        }

        public List<int> RegisteredCourses
        {
            get { return registeredCourses; }
            set { registeredCourses = value; }
        }

        public List<int> RegisteredExams
        {
            get { return registeredExams; }
            set { registeredExams = value; }
        }

        public int PenaltyPoints
        {
            get { return penaltyPoints; }
            set {  penaltyPoints = value; }
        }

        public bool AppliedForCourse
        {
            get { return appliedForCourse; }
            set {  appliedForCourse = value; }
        }

        
        public String StringToCsv()
        {
            string registeredCoursesString = string.Join(",", registeredCourses);
            string registeredExamsString = string.Join(",", registeredExams);

            return $"{id}|{name}|{surname}|{gender}|{birthday.ToString("yyyy-MM-dd")}|{phoneNumber}|{email}|{password}|{profession}|{registeredCoursesString}|{registeredExamsString}|{penaltyPoints}|{appliedForCourse}";

        }
    }
}