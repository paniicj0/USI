using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using USIProject.View;
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for DirectorDisplayCoursesWindow.xaml
    /// </summary>
    public partial class DirectorDisplayCoursesWindow : Window
    {
        private CourseController courseController = new CourseController();
        private TutorController tutorController = new TutorController();
        private List<Course> courses;
        private List<Tutor> tutors;
        public List<DaysEnum.Days> selectedDays;

        private CourseRepository courseRepository;
        private Director director;
        public int selectedId;
        public int selectedDuration;
        public int selectedMaxNumberOfStudents;
        public DateTime selectedStart;
        public LanguageEnum.Language selectedLanguage;
        public LanguageLevelEnum.LanguageLevel selectedLanguageLevel;
        public RealizationEnum.Realization selectedRealization;
        public TutorWindow tutorWindow;
        public DirectorWindow directorWindow;

        public DirectorDisplayCoursesWindow(Director director)
        {
            InitializeComponent();
            this.director = director;
            tutors = tutorController.GetAll();
            courseRepository = CourseRepository.GetInstance();
            courseRepository.CourseAdded += UpdateAddedCourse;
            LoadCourses();
        }

        private void UpdateAddedCourse(object sender, EventArgs e)
        {
            LoadCourses();
            
        }

        private void LoadCourses()
        {
            courses = courseController.GetAll();
            dgvCourse.ItemsSource = courses;
        }

        private void DirectorDisplayCoursesWindow_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DataGridColumn column in dgvCourse.Columns)
            {
                column.CanUserSort = true;
            }
            LoadCourses();
        }

        private void dgvCourse_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvCourse.SelectedItem != null)
            {
                Course selectedCourse = (Course)dgvCourse.SelectedItem;
                selectedId = selectedCourse.Id;
                selectedLanguage = selectedCourse.Language;
                selectedLanguageLevel = selectedCourse.LanguageLevel;
                selectedDays = selectedCourse.Days;
                selectedStart = selectedCourse.Start;
                selectedDuration = selectedCourse.Duration;
                selectedRealization = selectedCourse.Realization;
                selectedMaxNumberOfStudents = selectedCourse.MaxStudents;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DirectorCreateCourseWindow directorCreateCourseWindow = new DirectorCreateCourseWindow(director);
            directorCreateCourseWindow.ShowDialog();
        }

        private void dgvCourse_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           var course = e.Row.Item as Course;
            if (course != null && (course.TutorId == -1 || !tutors.Any(t => t.Id == course.TutorId)))
            {
                e.Row.Background = new SolidColorBrush(Colors.LightCoral);
            }
            else
            {
                e.Row.Background = new SolidColorBrush(Colors.White);
            }
        }

        private bool IsTutorAvailable(Tutor tutor, Course course)
        {
            var endDate = course.Start.AddDays(course.Duration * 7);
            var tutorCourses = courses.Where(c => c.TutorId == tutor.Id);

            foreach (var c in tutorCourses)
            {
                var courseEndDate = c.Start.AddDays(c.Duration * 7);
                var overlap = c.Days.Any(day => course.Days.Contains(day)) &&
                              !(endDate <= c.Start || course.Start >= courseEndDate);

                if (overlap)
                {
                    return false;
                }
            }

            return tutor.Languages == course.Language && tutor.LanguageLevel >= course.LanguageLevel;
        }

        private void btnAutomaticallyAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dgvCourse.SelectedItem == null)
            {
                MessageBox.Show("You must select a course first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Course selectedCourse = dgvCourse.SelectedItem as Course;

            if (selectedCourse != null)
            {
                if (selectedCourse.TutorId == 0)
                {
                    List<Tutor> availableTutors = tutors.Where(t => IsTutorAvailable(t, selectedCourse)).ToList();

                    if (availableTutors.Any())
                    {
                        //to have random and not only first tutor
                        Random random = new Random();
                        Tutor selectedTutor = availableTutors[random.Next(0, availableTutors.Count)];
                        selectedCourse.TutorId = selectedTutor.Id;
                        courseController.Update(selectedCourse);
                        MessageBox.Show($"Tutor {selectedTutor.Name} {selectedTutor.Surname} is successfully added to course .", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("There are currently no tutors available for this course.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("You can only select a tutor with id=0 !", "Warning!");
                }
            }
            LoadCourses();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                directorWindow = new DirectorWindow(director);
                directorWindow.Show();
            }
        }
    }
}
