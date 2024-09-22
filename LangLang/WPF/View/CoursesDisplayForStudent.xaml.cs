using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;
using static LangLang.ModelEnum.RealizationEnum;
using static LangLang.ModelEnum.StatusEnum;

namespace LangLang.View
{
    public partial class CoursesDisplayForStudent : Window
    {
        public Student student;
        public List<Course> courses;
        private Course selectedCourse;
        public ExamsDisplayForStudent examsDisplayForStudent;
        private CourseController courseController;
        private CourseRepository courseRepository;
        public StudentWindow studentWindow;

        private StudentCourseRequestController studentCourseRequestController = new StudentCourseRequestController();
        public List<StudentCourseRequest> requests;

        public CoursesDisplayForStudent(Student student)
        {
            InitializeComponent();
            courseRepository = CourseRepository.GetInstance();
            this.courses = courseRepository.GetAll();
            studentWindow = new StudentWindow(student);
            this.student = student;
            requests = studentCourseRequestController.GetAll();
        }

        public void CoursesDisplayForStudent_Load(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevel();
            FillComboBoxWithRealization();
            LoadDataFromCSV(courses);
        }

        private void LoadDataFromCSV(List<Course> courses)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("Level", typeof(string));
            dataTable.Columns.Add("Duration", typeof(int));
            dataTable.Columns.Add("Start", typeof(DateTime));
            dataTable.Columns.Add("Realization", typeof(string));
            dataTable.Columns.Add("MaxStudents", typeof(int));

            courseController = new CourseController();

            foreach (Course course in courses)
            {
                if (course.MaxStudents > course.NumberOfStudents)
                {
                    if ((course.Start - DateTime.Now).TotalDays >= 7)
                    {
                        string language = course.Language.ToString();
                        string languageLevel = course.LanguageLevel.ToString();
                        dataTable.Rows.Add(
                            course.Id,
                            language,
                            languageLevel,
                            course.Duration,
                            course.Start,
                            course.Realization,
                            course.MaxStudents
                        );
                    }
                }
            }

            dgCourse.ItemsSource = dataTable.DefaultView;
            dgCourse.UnselectAllCells();
            dgCourse.IsReadOnly = true;
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                studentWindow.Show();
            }
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

        private void tbDuration_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
            return courses.Where(course =>
                (!language.HasValue || course.Language == language) &&
                (!languageLevel.HasValue || course.LanguageLevel == languageLevel) &&
                (!start.HasValue || course.Start == start.Value) &&
                (duration == 0 || course.Duration >= duration) &&
                (!realization.HasValue || course.Realization == realization)
            ).ToList();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cbLanguage.SelectedItem = null;
            cbLanguageLevel.SelectedItem = null;
            cbRealization.SelectedItem = null;

            tbDuration.Text = string.Empty;

            dpStart.SelectedDate = null;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (dgCourse.SelectedItem == null)
            {
                MessageBox.Show("Please select a row before clicking on the 'Apply for a course' button.");
                return;
            }
            DataRowView row = (DataRowView)dgCourse.SelectedItem;
            int courseId = (int)row["ID"];

            requests = studentCourseRequestController.GetAll();

            bool alreadyApplied = studentCourseRequestController.IsAlreadyApplied(courseId, student.Id);

            if (student.AppliedForCourse)
            {
                MessageBox.Show("You can't apply for another course while attending another one!");
                return;
            }
            else if (alreadyApplied)
            {
                MessageBox.Show("You already applied for that course!");
                return;
            }
            StudentCourseRequest studentCourseRequest = new StudentCourseRequest(-1, courseId, student.Id, StatusEnum.Status.Pending, " ");
            studentCourseRequestController.Create(studentCourseRequest);
            MessageBox.Show("Successfully applied for the course!");
        }
    }
}
