using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public partial class SignUpWindow : Window
    {
        public static StudentController studentController;
        LogInWindow logInWindow = new LogInWindow();

        public SignUpWindow()
        {
            InitializeComponent();

        }

        private void SignUpWindow_Load(object sender, RoutedEventArgs e)
        {
            studentController = new StudentController();
            FillComboBoxWithGender();
            FillComboBoxWithProfession();
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

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbGender.SelectedItem == null && cbProfession.SelectedItem == null && string.IsNullOrWhiteSpace(tbName.Text) &&
                string.IsNullOrWhiteSpace(tbSurname.Text) && string.IsNullOrWhiteSpace(tbEmail.Text) &&
                string.IsNullOrWhiteSpace(tbPassword.Text) && !dpBirthday.SelectedDate.HasValue)
            {
                lblMissingData.Content = "Missing data!";
                return;
            }
            GenderEnum.Gender gender = (GenderEnum.Gender)cbGender.SelectedItem;
            DateTime birthday = dpBirthday.SelectedDate.Value;

            // Check if birthday is not a future date
            if (birthday > DateTime.Today)
            {
                lblMissingData.Content = "Birthday cannot be a future date!";
                return;
            }

            int phoneNumber = 0;
            if (!string.IsNullOrWhiteSpace(tbPhoneNumber.Text))
            {
                int.TryParse(tbPhoneNumber.Text, out phoneNumber);
            }

            StudentEnum.Profession profession = (StudentEnum.Profession)cbProfession.SelectedItem;

            List<int> list1 = new List<int>() { -1 };
            List<int> list2 = new List<int>() { -1 };

            Student student = new Student(-1, tbName.Text, tbSurname.Text, gender, birthday, phoneNumber, tbEmail.Text,
                tbPassword.Text, profession, list1, list2, 0, false);
            studentController.Create(student);

            logInWindow.Show();
            this.Hide();
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

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                logInWindow.Show();
            }
        }
    }
}
