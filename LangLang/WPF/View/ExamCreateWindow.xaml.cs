using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for CreateExamForm.xaml
    /// </summary>
    public partial class ExamCreateWindow : Window
    {
        private Tutor tutor;
        private List<Course> courses;
        public ExamCreateWindow(Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
        }

        private ExamController examController;

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbLanguage.SelectedItem != null && cbLanguageLevel.SelectedItem != null && !string.IsNullOrWhiteSpace(tbNumOfStudents.Text) && dtpExam.SelectedDate.HasValue)
            {
                LanguageEnum.Language language = (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), cbLanguage.SelectedItem.ToString());
                LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)cbLanguageLevel.SelectedItem;
                int students = int.Parse(tbNumOfStudents.Text);
                DateTime? dateExam = dtpExam.SelectedDate;
                TimeSpan? examTime = ParseExamTime();

                if (dateExam.HasValue || !examTime.HasValue)
                {
                    Exam exam = new Exam(-1, language, languageLevel, students, dateExam.Value, examTime.Value, 4, tutor.Id, 0);
                    examController.Create(exam);
                    ClearFields();
                }
                else
                {
                    MessageBox.Show("Please select a valid exam date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

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
            FillComboBoxWithLanguage(tutor.Languages);
            FillComboBoxWithLanguageLevels(tutor.LanguageLevel);
            // dtpExam.MinDate = DateTime.Today;
            dtpExam.DisplayDateStart = DateTime.Today;
            examController = new ExamController();
            courses = CourseRepository.GetInstance().LoadFromFile();
        }

        private void FillComboBoxWithLanguage(Language languages)
        {
            cbLanguage.Items.Add(languages);
        }

        private void FillComboBoxWithLanguageLevels(LanguageLevel level)
        {
            cbLanguageLevel.Items.Add(level);
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

        //keep this fror laiter update 
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
