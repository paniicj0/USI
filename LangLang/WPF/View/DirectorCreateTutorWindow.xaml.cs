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

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for DirectorCreateTutorWindow.xaml
    /// </summary>
    public partial class DirectorCreateTutorWindow : Window
    {
        private TutorController tutorController;
        public event EventHandler<Tutor> TutorCreated;

        public DirectorCreateTutorWindow()
        {
            InitializeComponent();
            tutorController = new TutorController();
        }

        private void DirectorCreateTutorWindow_Load(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithGender();
            FillComboBoxWithLanguages();
            FillComboBoxWithLanguageLevel();

        }

        private void FillComboBoxWithGender()
        {
            Array genders = Enum.GetValues(typeof(GenderEnum.Gender));

            foreach (var gender in genders)
            {
                cbGender.Items.Add(gender);
            }
        }

        private void FillComboBoxWithLanguages()
        {
            Array languages = Enum.GetValues(typeof(LanguageEnum.Language));

            foreach (var language in languages)
            {
                cbLanguage.Items.Add(language);
            }
        }

        private void FillComboBoxWithLanguageLevel()
        {
            Array languageLevel = Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel));

            foreach (var languagelevel in languageLevel)
            {
                cbLanguageLevel.Items.Add(languagelevel);
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text) || string.IsNullOrWhiteSpace(tbSurname.Text) ||
            cbGender.SelectedItem == null || string.IsNullOrWhiteSpace(tbPhoneNumber.Text) ||
            string.IsNullOrWhiteSpace(tbEmail.Text) || string.IsNullOrWhiteSpace(tbPassword.Text) ||
            cbLanguage.SelectedItem == null || cbLanguageLevel.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }


            int id = -1;
            string name = tbName.Text;
            string surname = tbSurname.Text;
            GenderEnum.Gender gender = (GenderEnum.Gender)cbGender.SelectedItem;
            DateTime? birthday = dpBirthday.SelectedDate;
            if (!birthday.HasValue)
            {
                MessageBox.Show("Please select a valid birthday.");
                return;
            }
            int phoneNumber = Convert.ToInt32(tbPhoneNumber.Text);
            string email = tbEmail.Text;
            string password = tbPassword.Text;
            LanguageEnum.Language language = (LanguageEnum.Language)cbLanguage.SelectedItem;
            LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)cbLanguageLevel.SelectedItem;
            DateTime tutorCreationDate = DateTime.Now;

            Tutor tutor = new Tutor(id, name, surname, gender, birthday.Value, phoneNumber, email, password, language, languageLevel, tutorCreationDate);
            tutorController.Create(tutor);
            MessageBox.Show("Successfully added the tutor.");
            TutorCreated?.Invoke(this, tutor);
            this.Close();

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


    }
}