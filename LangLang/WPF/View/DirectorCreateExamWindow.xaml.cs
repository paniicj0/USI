using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
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
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for DirectorCreateExamWindow.xaml
    /// </summary>
    public partial class DirectorCreateExamWindow : Window
    {
        private Tutor tutor;
        private List<Course> courses;
        private ExamController examController;
        private CourseController courseController;

        public DirectorCreateExamWindow()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbLanguage.SelectedItem == null && cbLanguageLevel.SelectedItem == null && string.IsNullOrWhiteSpace(tbNumOfStudents.Text) && !dtpExam.SelectedDate.HasValue)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
               
            }
            LanguageEnum.Language language = (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), cbLanguage.SelectedItem.ToString());
            LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)cbLanguageLevel.SelectedItem;
            int students = int.Parse(tbNumOfStudents.Text);
            DateTime? dateExam = dtpExam.SelectedDate;
            TimeSpan? examTime = ParseExamTime();
            int tutorId = examController.SmartChoiceTutorForExam(language, languageLevel, dateExam, examTime);
            if (tutorId == -1)
            {
                MessageBox.Show("There is no available tutors for this exam.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!dateExam.HasValue || !examTime.HasValue)
            {
                MessageBox.Show("Please select a valid exam date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
                
            }
            Exam exam = new Exam(-1, language, languageLevel, students, dateExam.Value, examTime.Value, 4, tutorId, 0);
            examController.Create(exam);
            ClearFields();

        }

        private TimeSpan? ParseExamTime()
        {
            if (cbExamTime.SelectedItem != null)
            {
                string timeString = ((ComboBoxItem)cbExamTime.SelectedItem).Content.ToString();
                if (TimeSpan.TryParse(timeString, out TimeSpan examTime))
                {
                    return examTime;
                }
            }
            return null;
        }

        private void ClearFields()
        {
            tbNumOfStudents.Text = "";
            dtpExam.SelectedDate = null;
            cbLanguage.SelectedItem = null;
            cbLanguageLevel.SelectedItem = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            courseController = new CourseController();
            examController = new ExamController();
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevels();
            dtpExam.DisplayDateStart = DateTime.Today;

        }

        private void FillComboBoxWithLanguage()
        {
            Array languages = Enum.GetValues(typeof(LanguageEnum.Language));

            foreach (var l in languages)
            {
                cbLanguage.Items.Add(l);
            }
            
        }

        private void FillComboBoxWithLanguageLevels()
        {
            Array languageLevels = Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel));

            foreach (var level in languageLevels)
            {
                cbLanguageLevel.Items.Add(level);
            }
        }

        private void tbNumOfStudents_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }

        private void cbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LanguageEnum.Language selectedLanguage = (LanguageEnum.Language)cbLanguage.SelectedItem;
            var coursesForSelectedLanguage = courses.Where(course => course.Language == selectedLanguage);
            var availableLevels = coursesForSelectedLanguage.Select(course => course.LanguageLevel).Distinct();

            cbLanguageLevel.Items.Clear();

            foreach (var level in availableLevels)
            {
                cbLanguageLevel.Items.Add(level);
            }
        }
    }
}

