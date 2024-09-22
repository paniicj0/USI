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
    /// Interaction logic for CreateCourseWindow.xaml
    /// </summary>
    public partial class CourseCreateWindow : Window
    {
        private Tutor tutor;
        public CourseCreateWindow(Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
        }

        private CourseController courseController;

        private void CourseCreateWindow_Load(object sender, EventArgs e)
        {
            courseController = new CourseController();
            dpStart.DisplayDateStart = DateTime.Today;
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevels();
            FillComboBoxWithRealization();
            FillCheckListBoxDays();
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

        private void FillCheckListBoxDays()
        {
            foreach (DaysEnum.Days day in Enum.GetValues(typeof(DaysEnum.Days)))
            {
                lbDays.Items.Add(day);
            }
        }

        private void FillComboBoxWithRealization()
        {
            Array realizations = Enum.GetValues(typeof(RealizationEnum.Realization));

            foreach (var realization in realizations)
            {
                cbRealization.Items.Add(realization);
            }
        }

        private void cbRealization_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRealization.SelectedItem != null && cbRealization.SelectedItem.ToString() == "live")
            {
                tbMaxNumberOfStudents.IsEnabled = true;
            }
            else
            {
                tbMaxNumberOfStudents.IsEnabled = false;
                tbMaxNumberOfStudents.Text = "";
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {

            if (cbLanguage.SelectedItem != null && cbLanguageLevel.SelectedItem != null && !string.IsNullOrWhiteSpace(tbDuration.Text) && dpStart.SelectedDate.HasValue)
            {
                LanguageEnum.Language language = (LanguageEnum.Language)cbLanguage.SelectedItem;

                LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)cbLanguageLevel.SelectedItem;
                int weeks = int.Parse(tbDuration.Text);
                DateTime? start = dpStart.SelectedDate;

                List<DaysEnum.Days> days = new List<DaysEnum.Days>();
                foreach (DaysEnum.Days day in lbDays.SelectedItems)
                {
                    days.Add(day);
                }

                RealizationEnum.Realization realization = (RealizationEnum.Realization)cbRealization.SelectedItem;

                int students = 0;
                if (realization.ToString() == "online")
                {
                    students = 0;
                }
                else
                {
                    students = int.Parse(tbMaxNumberOfStudents.Text);
                }

                if (start.HasValue)
                {
                    Course course = new Course(-1, language, languageLevel, weeks, days, start.Value, realization, students, tutor.Id, 0);
                    courseController.Create(course);
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Please select a valid start date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
