using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for TutorsCoursesWindow.xaml
    /// </summary>
    public partial class TutorsCoursesWindow : Window
    {
        private CourseController courseController = new CourseController();
        private Tutor tutor;
        List<Course> courses;

        private CourseRepository courseRepository;
        public int selectedId;
        public LanguageEnum.Language selectedLanguage;
        public LanguageLevelEnum.LanguageLevel selectedLanguageLevel;
        public int selectedDuration;
        public List<DaysEnum.Days> selectedDays;
        public RealizationEnum.Realization selectedRealization;
        public int selectedMaxNumberOfStudents;
        public DateTime selectedStart;
        public TutorWindow tutorWindow;

        public TutorsCoursesWindow(Tutor tutor)
        {
            InitializeComponent();
            this.tutor = tutor;
            courseRepository = CourseRepository.GetInstance();
            courseRepository.CourseAdded += UpdateAddedExam;
            tutorWindow = new TutorWindow(tutor);
        }

        private void TutorsCoursesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevel();
            FillComboBoxWithRealization();
            foreach (DataGridColumn column in dgvCourse.Columns)
            {
                column.CanUserSort = true;
            }
            courseController = new CourseController();
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
            courses = courseController.GetAll();
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

                    dataTable.Rows.Clear();

                    foreach (Course course in courses)
                    {
                        if (tutor.Id == course.TutorId) {
                            if (IsValidCourse(course))
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

            return course != null && course.Start != null;
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CourseCreateWindow createCourseWindow = new CourseCreateWindow(tutor);
            createCourseWindow.ShowDialog();
            courses = courseController.GetAll();
            LoadDataFromCSV(courses);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgvCourse.SelectedItem != null)
            {
                //send reference
                TimeSpan timeUntilCourse = selectedStart - DateTime.Today;
                if (timeUntilCourse.TotalDays <= 7)
                {
                    MessageBox.Show("You cannot update a course if there are less than 7 days left until the course starts.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                CourseUpdateWindow updateCourseWindow = new CourseUpdateWindow(this, tutor);
                updateCourseWindow.ShowDialog();
                courses = courseController.GetAll();
                LoadDataFromCSV(courses);
            }
            else
            {
                MessageBox.Show("First select the row that you want to update.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

        private void dgvCourse_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvCourse.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvCourse.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                selectedLanguage = (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), selectedRow["Language"].ToString());
                selectedLanguageLevel = (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), selectedRow["Level"].ToString());
                string[] dayTokens = selectedRow["Days"].ToString().Split(',');
                selectedDays = dayTokens.Select(token => (DaysEnum.Days)Enum.Parse(typeof(DaysEnum.Days), token.Trim())).ToList();
                selectedStart = (DateTime)selectedRow["Start"];

                selectedDuration = (int)selectedRow["Duration"];
                selectedRealization = (RealizationEnum.Realization)Enum.Parse(typeof(RealizationEnum.Realization), selectedRow["Realization"].ToString());
                selectedMaxNumberOfStudents = (int)selectedRow["Max students"];
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (dgvCourse.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvCourse.SelectedItem;
                int courseId = (int)selectedRow["Id"];
                TimeSpan timeUntilStart = selectedStart - DateTime.Today;
                if (timeUntilStart.TotalDays <= 7)
                {
                    MessageBox.Show("You cannot delete a course if there are less than 7 days left until the course starts.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                courseController.Delete(courseId);
                MessageBox.Show("You succesfully delete this course.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                courses=courseController.GetAll();
                LoadDataFromCSV(courses);
            }
            else
            {
                MessageBox.Show("First select the row that you want to delete.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
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
    }
}
