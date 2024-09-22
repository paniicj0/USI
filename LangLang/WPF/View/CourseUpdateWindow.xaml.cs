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
    /// Interaction logic for UpdateCourseWindow.xaml
    /// </summary>
    public partial class CourseUpdateWindow : Window
    {
        private CourseController courseController;
        private TutorsCoursesWindow tutorsCoursesWindow;
        private Tutor tutor;


        public CourseUpdateWindow(TutorsCoursesWindow tutorsCoursesWindow, Tutor tutor)
        {
            InitializeComponent();
            this.tutorsCoursesWindow = tutorsCoursesWindow;
            this.tutor = tutor;
            courseController = new CourseController();
        }


        private void CourseUpdateWindow_Loaded(object sender, EventArgs e)
        {
            cbLanguage.SelectedItem = tutorsCoursesWindow.selectedLanguage;
            cbLanguageLevel.SelectedItem = tutorsCoursesWindow.selectedLanguageLevel;
            cbRealization.SelectedItem = tutorsCoursesWindow.selectedRealization;
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevels();
            tbDuration.Text = tutorsCoursesWindow.selectedDuration.ToString();
            FillCheckListBoxDays();
            dpStart.DisplayDateStart = DateTime.Today;
            FillComboBoxWithRealization();
            tbMaxNumberOfStudents.Text = tutorsCoursesWindow.selectedMaxNumberOfStudents.ToString();
            dpStart.SelectedDate = tutorsCoursesWindow.selectedStart;
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

        private void cbRealization_SelectionChangedUpdate(object sender, SelectionChangedEventArgs e)
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            int id = tutorsCoursesWindow.selectedId;
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
            int students = int.Parse(tbMaxNumberOfStudents.Text);

            Course courseUpadte = new Course(id, language, languageLevel, weeks, days, start.Value, realization, students, tutor.Id, 0);
            courseController.Update(courseUpadte);

            this.Close();

        }

    }
}