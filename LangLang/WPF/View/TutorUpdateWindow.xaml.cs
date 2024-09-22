using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using System;
using System.Collections.Generic;
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
using static LangLang.ModelEnum.GenderEnum;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for TutorUpdateWindow.xaml
    /// </summary>
    public partial class TutorUpdateWindow : Window
    {
        private Tutor tutor;
        private TutorController tutorController;
        public event EventHandler<Tutor> TutorUpdated;


        public TutorUpdateWindow(Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;

            Prefill();

        }
        private void TutorUpdateWindow_Load(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithGender();

            tutorController = new TutorController();
        }

        private void FillComboBoxWithGender()
        {
            Array genders = Enum.GetValues(typeof(GenderEnum.Gender));

            foreach (var gender in genders)
            {
                cbGender.Items.Add(gender);
            }
        }

        private void Prefill()
        {
            if (tutor != null)
            {
                tbName.Text = tutor.Name;
                tbSurname.Text = tutor.Surname;
                cbGender.SelectedItem = tutor.Gender;
                dpBirthday.SelectedDate = tutor.Birthday;
                tbPhoneNumber.Text = tutor.PhoneNumber.ToString();
                tbEmail.Text = tutor.Email;
                tbPassword.Text = tutor.Password;

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
            if (string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbSurname.Text) ||
                cbGender.SelectedItem == null || string.IsNullOrWhiteSpace(tbPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(tbEmail.Text) || string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            string name = tbName.Text;
            string surname = tbSurname.Text;
            Gender gender = (Gender)cbGender.SelectedItem;
            DateTime? birthday = dpBirthday.SelectedDate;
            if (!birthday.HasValue)
            {
                MessageBox.Show("Please select a valid birthday.");
                return;
            }
            int phoneNumber = Convert.ToInt32(tbPhoneNumber.Text);
            string email = tbEmail.Text;
            string password = tbPassword.Text;

            DateTime tutorCreationDate = dpCreation.SelectedDate ?? DateTime.Now;

            if (tutor != null)
            {
                tutor.Name = name;
                tutor.Surname = surname;
                tutor.Gender = gender;
                tutor.Birthday = birthday.Value;
                tutor.PhoneNumber = phoneNumber;
                tutor.Email = email;
                tutor.Password = password;

                tutorController.Update(tutor);

                MessageBox.Show("Successfully updated the tutor.");
                TutorUpdated?.Invoke(this, tutor);
                this.Close();

            }
        }

    }
}

