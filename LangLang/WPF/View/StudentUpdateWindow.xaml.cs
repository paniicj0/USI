using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LangLang.View
{
    public partial class StudentUpdateWindow : Window
    {
        private Student student;
        public StudentWindow studentWindow;
        private StudentController studentController;
        private StudentExamCancellation studentExamCancellation;
        public List<StudentCourseRequest> requests;
        public StudentCourseRequestController studentCourseRequestController = new StudentCourseRequestController();

        public StudentUpdateWindow(Student student, StudentWindow studentWindow)
        {
            InitializeComponent();
            this.student = student;
            this.studentWindow = studentWindow;
            this.requests = studentCourseRequestController.GetAll();
            Prefill();
        }

        private void StudentUpdateWindow_Load(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithGender();
            FillComboBoxWithProfession();
            studentController = new StudentController();
        }

        private void FillComboBoxWithGender()
        {
            Array genders = Enum.GetValues(typeof(GenderEnum.Gender));

            foreach (var gender in genders)
            {
                cbGender.Items.Add(gender);
            }
        }

        private void FillComboBoxWithProfession()
        {
            Array professions = Enum.GetValues(typeof(StudentEnum.Profession));

            foreach (var profession in professions)
            {
                cbProfession.Items.Add(profession);
            }
        }

        private void Prefill()
        {
            if (student != null)
            {
                tbName.Text = student.Name;
                tbSurname.Text = student.Surname;
                cbGender.SelectedItem = student.Gender;
                dpBirthday.SelectedDate = student.Birthday;
                tbPhoneNumber.Text = student.PhoneNumber.ToString();
                tbEmail.Text = student.Email;
                tbPassword.Text = student.Password;
                cbProfession.SelectedItem = student.Profession;
            }
        }
        private void tbPhoneNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            foreach(StudentCourseRequest request in requests)
            {
                if (student.AppliedForCourse)
                {
                    MessageBox.Show("You can't update your data if you are applied on exam!");
                    return;
                }
                string name = tbName.Text;
                string surname = tbSurname.Text;
                GenderEnum.Gender? gender = null;
                if (cbGender.SelectedIndex != -1)
                {
                    gender = (GenderEnum.Gender)cbGender.SelectedItem;
                }
                DateTime? birthdayValue = dpBirthday.SelectedDate;
                DateTime birthday = DateTime.MinValue;
                if (birthdayValue.HasValue)
                {
                    birthday = birthdayValue.Value;
                }
                int phoneNumber = 0;
                if (!string.IsNullOrWhiteSpace(tbPhoneNumber.Text))
                {
                    phoneNumber = Int32.Parse(tbPhoneNumber.Text);
                }
                string email = tbEmail.Text;
                string password = tbPassword.Text;
                StudentEnum.Profession? profession = null;
                StudentAppliedOnCourseEnum.StudentAppliedOnCourse? studentAppliedOnCourseEnum = null;
                if (cbProfession.SelectedIndex != -1)
                {
                    profession = (StudentEnum.Profession)cbProfession.SelectedIndex;
                }

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) ||
                    string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) ||
                    gender == null || profession == null || phoneNumber == 0 || birthday == DateTime.MinValue)
                {
                    lblFail.Content = "Missing data!";
                }
                Student newStudent = new Student(student.Id, name, surname, gender.Value, birthday, phoneNumber, email, password, profession.Value, student.RegisteredCourses, student.RegisteredExams, student.PenaltyPoints, student.AppliedForCourse);
                studentController.Update(newStudent);

                studentWindow.Show();
                studentWindow.WriteWelcome(newStudent);
                this.Hide();
            } 
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                studentWindow.Show();
            }
        }
    }
}
