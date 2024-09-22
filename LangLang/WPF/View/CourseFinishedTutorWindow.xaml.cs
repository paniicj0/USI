using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Data;
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
using USIProject.View;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for CourseFinishedTutor.xaml
    /// </summary>
    public partial class CourseFinishedTutorWindow : Window
    {
        private CourseController courseController = new CourseController();
        List<Course> courses;
        public int selectedId;
        public Tutor tutor;
        public CourseRepository courseRepository;

        public CourseFinishedTutorWindow(Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
            CourseRepository courseRepository = CourseRepository.GetInstance();
        }
        private void CourseFinishedTutorWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevel();
            FillComboBoxWithRealization();
            foreach (DataGridColumn column in dgvCourse.Columns)
            {
                column.CanUserSort = true;
            }
            courses = courseController.GetAll();
            LoadDataFromCSV(courses);

        }

        private void FillComboBoxWithLanguage()
        {
            Array languages = Enum.GetValues(typeof(LanguageEnum.Language));

            foreach (var language in languages)
            {
                cbLanguage.Items.Add(language);
            }
        }

        private void FillComboBoxWithLanguageLevel()
        {
            Array languageLevels = Enum.GetValues(typeof(LanguageLevelEnum.LanguageLevel));

            foreach (var languageLevel in languageLevels)
            {
                cbLanguageLevel.Items.Add(languageLevel);
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

        private void UpdateAddedExam(object sender, EventArgs e)
        {
            LoadDataFromCSV(courses);
        }

        private void LoadDataFromCSV(List<Course> courses)
        {
            try
            {
                using (DataTable dataTable = new DataTable())
                {
                    dataTable.Columns.Clear();
                    dataTable.Columns.Add("Id", typeof(int));
                    dataTable.Columns.Add("Language", typeof(string));
                    dataTable.Columns.Add("Level", typeof(string));
                    dataTable.Columns.Add("Duration", typeof(int));
                    dataTable.Columns.Add("Days", typeof(string));
                    dataTable.Columns.Add("Start", typeof(DateTime));
                    dataTable.Columns.Add("Realization", typeof(string));
                    dataTable.Columns.Add("Max students", typeof(int));

                    foreach (Course course in courses)
                    {
                        if (tutor.Id == course.TutorId)
                        {
                            if (IsValidCourse(course))
                            {
                                DateTime endDate = course.Start.AddDays(course.Duration * 7); 
                                if (endDate <= DateTime.Today) 
                                {
                                    string daysString = string.Join(", ", course.Days.Select(day => day.ToString()));
                                    dataTable.Rows.Add(
                                        course.Id,
                                        course.Language.ToString(),
                                        course.LanguageLevel.ToString(),
                                        course.Duration,
                                        daysString,
                                        course.Start,
                                        course.Realization.ToString(),
                                        course.MaxStudents
                                    );
                                }
                            }
                        }
                    }

                    dgvCourse.ItemsSource = dataTable.DefaultView;
                    dgvCourse.UnselectAllCells();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidCourse(Course course)
        {

            return course != null;
        }

        private void dgvCourse_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvCourse.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvCourse.SelectedItem;
                selectedId = (int)selectedRow["Id"];
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cbLanguage.SelectedItem = null;
            cbLanguageLevel.SelectedItem = null;
            cbRealization.SelectedItem = null;
            tbDuration.Text = string.Empty;
            dpStart.SelectedDate = null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LanguageEnum.Language? language = cbLanguage.SelectedItem as LanguageEnum.Language?;
            LanguageLevelEnum.LanguageLevel? languageLevel = cbLanguageLevel.SelectedItem as LanguageLevelEnum.LanguageLevel?;
            DateTime? start = dpStart.SelectedDate;
            int duration = 0;
            int.TryParse(tbDuration.Text, out duration);
            RealizationEnum.Realization? realization = cbRealization.SelectedItem as RealizationEnum.Realization?;

            if (language == null && languageLevel == null && start == null && duration == 0 && realization == null)
            {
                lblNoSearch.Content = "No search parameters have been entered.";
                LoadDataFromCSV(courses);

            }
            else
            {
                List<Course> filteredCourses = FilterCourses(language, languageLevel, start, duration, realization);
                LoadDataFromCSV(filteredCourses);

            }

        }

        private List<Course> FilterCourses(LanguageEnum.Language? language, LanguageLevelEnum.LanguageLevel? languageLevel, DateTime? start, int duration, RealizationEnum.Realization? realization)
        {
            courseController = new CourseController();
            List<Course> coursesFilter = courseController.GetAll();

            return coursesFilter.Where(course =>
                (!language.HasValue || course.Language == language) &&
                (!languageLevel.HasValue || course.LanguageLevel == languageLevel) &&
                (!start.HasValue || course.Start == start.Value) &&
                (duration == 0 || course.Duration >= duration) &&
                (!realization.HasValue || course.Realization == realization || course.TutorId == tutor.Id)
            ).ToList();
        }

        private void btnGrade_Click(object sender, RoutedEventArgs e)
        {
            TutorGradingWindow window = new TutorGradingWindow(tutor, selectedId);
            window.Show();
            courses = courseController.GetAll();
            LoadDataFromCSV(courses);
        }
    }
}
