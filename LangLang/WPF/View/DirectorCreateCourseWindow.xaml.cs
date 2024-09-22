using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
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

    public partial class DirectorCreateCourseWindow : Window
    {
        private Director director;
        private CourseController courseController=new CourseController();
        private TutorController tutorController=new TutorController();
        private List<Tutor> tutors;
        private List<Course> courses;
        private Dictionary<string, int> tutorNameToIdMap = new Dictionary<string, int>();
        public DirectorCreateCourseWindow(Director director)
        {
            InitializeComponent();
            this.director= director;
            tutors = tutorController.GetAll();
            foreach (var tutor in tutors)
            {
                Console.WriteLine($"{tutor.Name} {tutor.Surname} - {tutor.Languages} - {tutor.LanguageLevel}");
            }
            courses = courseController.GetAll();
            LoadComboBoxes();
        }

        private void DirectorCreateCourseWindow_Load(object sender, RoutedEventArgs e)
        {
            courseController = new CourseController();
            dpStart.DisplayDateStart = DateTime.Today;
            FillCheckListBoxDays();
            LoadComboBoxes();
            UpdateAvailableTutors();
        }


        private void FillCheckListBoxDays()
        {
            foreach (DaysEnum.Days day in Enum.GetValues(typeof(DaysEnum.Days)))
            {
                lbDays.Items.Add(day);
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
                    var selectedTutorName = cbTutorType.SelectedItem as string;
                    if (selectedTutorName != null && tutorNameToIdMap.TryGetValue(selectedTutorName, out int tutorId))
                    {
                        Course course = new Course(-1, language, languageLevel, weeks, days, start.Value, realization, students, tutorId, 0);
                        courseController.Create(course);
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Please select a valid tutor.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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

        private void cbTutorType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTutors();
        }

        private void UpdateAvailableTutors()
        {
            var selectedLanguage = (LanguageEnum.Language?)cbLanguage.SelectedItem;
            var selectedLevel = (LanguageLevelEnum.LanguageLevel?)cbLanguageLevel.SelectedItem;
            var duration = int.TryParse(tbDuration.Text, out int parsedDuration) ? parsedDuration : 0;
            var startDate = dpStart.SelectedDate ?? DateTime.MinValue;

            if (selectedLanguage.HasValue && selectedLevel.HasValue && duration > 0 && startDate != DateTime.MinValue)
            {
                // Filtriraj tutore prema jeziku i nivou jezika
                var competentTutors = tutors.Where(tutor =>
                    tutor.Languages.HasFlag(selectedLanguage.Value) &&
                    tutor.LanguageLevel == selectedLevel.Value).ToList();

                // Filtriraj kompetentne tutore prema dostupnosti za odabrani datum
                var availableTutors = competentTutors.Where(tutor =>
                    IsTutorAvailable(tutor, startDate, duration)).ToList();

                tutorNameToIdMap.Clear();
                foreach (var tutor in availableTutors)
                {
                    var tutorFullName = $"{tutor.Name} {tutor.Surname}";
                    tutorNameToIdMap[tutorFullName] = tutor.Id;
                }

                cbTutorType.ItemsSource = availableTutors.Select(tutor => $"{tutor.Name} {tutor.Surname}").ToList();

                Console.WriteLine("Available tutors:");
                foreach (var tutor in availableTutors)
                {
                    Console.WriteLine($"{tutor.Name} {tutor.Surname}");
                }
            }
            else
            {
                cbTutorType.ItemsSource = null;
                tutorNameToIdMap.Clear();
            }
        }

        private bool IsTutorAvailable(Tutor tutor, DateTime startDate, int duration)
        {
            var endDate = startDate.AddDays(duration * 7);
            var tutorCourses = courses.Where(course => course.TutorId == tutor.Id);

            foreach (var course in tutorCourses)
            {
                var courseEndDate = course.Start.AddDays(course.Duration * 7);
                var overlap = !(endDate <= course.Start || startDate >= courseEndDate);

                if (overlap)
                {
                    return false;
                }
            }

            return true;
        }



        private void LoadComboBoxes()
        {
            cbLanguage.ItemsSource = Enum.GetValues(typeof(LanguageEnum.Language)).Cast<LanguageEnum.Language>();
            cbLanguageLevel.ItemsSource = Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel)).Cast<LanguageLevelEnum.LanguageLevel>();
            cbRealization.ItemsSource = Enum.GetValues(typeof(RealizationEnum.Realization)).Cast<RealizationEnum.Realization>();
        }

        private void cbLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTutors();
        }

        private void cbLanguageLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTutors();
        }

        private void tbDuration_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAvailableTutors();
        }

        private void dpStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTutors();
        }

        private void lbDays_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAvailableTutors();
        }
    }
}
