using LangLang.Controllers;
using LangLang.Model;
using LangLang.ModelEnum;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
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
using USIProject.View;
using static LangLang.ModelEnum.LanguageEnum;
using static LangLang.ModelEnum.LanguageLevelEnum;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for UpdateExamForm.xaml
    /// </summary>
    public partial class ExamDisplayWindow : Window
    {

        ExamController examController;
        private ExamRepository examRepository;
        public int selectedId;
        public int selectedNumOfStudents;
        public DateTime selectedExamDate;
        public Language selectedLanguage;
        public LanguageLevel selectedLanguageLevel;
        private int selectedExamDuration;
        private int selectedNumOfAppliedStudents;
        private int selectedTutorId;
        private Tutor tutor;
        public TutorWindow tutorWindow;
        private Student student;
        private StudentRepository studentRepository;
        private CourseRepository courseRepository;
        private TutorRepository tutorRepository;

        public ExamDisplayWindow()
        {
            InitializeComponent();
            examRepository = ExamRepository.GetInstance();
            studentRepository = StudentRepository.GetInstance();
            examRepository.ExamAdded += UpdateAddedExam;
            courseRepository = CourseRepository.GetInstance();
            tutorRepository = TutorRepository.GetInstance();
        }

        public ExamDisplayWindow(Tutor tutor) : this()
        {
            this.tutor = tutor;
            tutorWindow = new TutorWindow(tutor);
        }

        private void UpdateAddedExam(object sender, EventArgs e)
        {
            LoadDataFromCSV();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvExam.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvExam.SelectedItem;
                int examId = (int)selectedRow["Id"];
                TimeSpan timeUntilExam = selectedExamDate - DateTime.Today;
                if (timeUntilExam.TotalDays <= 14)
                {
                    MessageBox.Show("You cannot delete an exam if there are less than 14 days left until the exam starts.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                examController.Delete(examId);

                LoadDataFromCSV();
            }
            else
            {
                MessageBox.Show("First select the row that you want to delete.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ExamDisplayWindow_Load(object sender, RoutedEventArgs e)
        {
            ExamRepository examRepository = ExamRepository.GetInstance();
            //seting possibility to sort collumns 
            foreach (DataGridColumn column in dgvExam.Columns)
            {
                column.CanUserSort = true;
            }

            LoadDataFromCSV();
        }

        public void LoadDataFromCSV()
        {
            if (tutor == null)
            {
                return;
            }

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("LanguageLevel", typeof(string));
            dataTable.Columns.Add("NumOfStudents", typeof(int));
            dataTable.Columns.Add("ExamDate", typeof(DateTime));
            dataTable.Columns.Add("ExamDuration", typeof(int));
            dataTable.Columns.Add("TutorId", typeof(int));
            dataTable.Columns.Add("NumOfAppliedStudents", typeof(int));

            examController = new ExamController();
            List<Exam> exams = examController.GetAll();

            foreach (Exam exam in exams)
            {
                if (tutor.Id == exam?.TutorId) {
                    string language = exam.Language.ToString();
                    string languageLevel = exam.LanguageLevel.ToString();
                    dataTable.Rows.Add(
                        exam.Id,
                        language,
                        languageLevel,
                        exam.NumOfStudents,
                        exam.ExamDate,
                        exam.ExamDuration,
                        exam.TutorId,
                        exam.NumberOfAppliedStudents
                    );
                }
                
            }

            dgvExam.ItemsSource = dataTable.DefaultView;
            dgvExam.UnselectAllCells();
        }

        private void dgvExam_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvExam.SelectedItem == null || dgvExam.SelectedCells.Count == 0)
            {
                dgvExam.UnselectAll();
                return;
            }

            if (dgvExam.SelectedItem is DataRowView selectedRow)
            {
                selectedRow = (DataRowView)dgvExam.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                selectedLanguage = (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), selectedRow["Language"].ToString());
                selectedLanguageLevel = (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), selectedRow["LanguageLevel"].ToString());
                selectedNumOfStudents = (int)selectedRow["NumOfStudents"];
                selectedExamDate = (DateTime)selectedRow["ExamDate"];
                selectedExamDuration = (int)selectedRow["ExamDuration"];
                selectedTutorId = (int)selectedRow["TutorId"];
                selectedNumOfAppliedStudents = (int)selectedRow["NumOfAppliedStudents"];
            }
        }



        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgvExam.SelectedItem != null)
            {
                //send reference
                TimeSpan timeUntilExam = selectedExamDate - DateTime.Today;
                if (timeUntilExam.TotalDays <= 14)
                {
                    MessageBox.Show("You cannot update an exam if there are less than 14 days left until the exam starts.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                ExamUpdateWindow updateForm = new ExamUpdateWindow(this, tutor);
                updateForm.ShowDialog();
                LoadDataFromCSV();
            }
            else
            {
                MessageBox.Show("First select the row that you want to update.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = tbSearch.Text;

            ICollectionView view = CollectionViewSource.GetDefaultView(dgvExam.ItemsSource);
            if (view != null)
            {
                view.Filter = item =>
                {
                    DataRowView row = item as DataRowView;
                    if (row != null)
                    {
                        foreach (var cellValue in row.Row.ItemArray)
                        {
                            if (cellValue.ToString().Contains(searchTerm))
                                return true;
                        }
                    }
                    return false;
                };
            }
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = tbSearch.Text;

            if (dgvExam.ItemsSource is DataView dataView)
            {
                DataTable dt = dataView.Table;
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    string[] searchTerms = searchTerm.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    string filter = GetFilterExpression(dt, searchTerms);
                    dataView.RowFilter = filter;
                }
                else
                {
                    dataView.RowFilter = "";
                }
            }
        }

        private string GetFilterExpression(DataTable dt, string[] searchTerms)
        {
            List<string> filters = new List<string>();

            foreach (string term in searchTerms)
            {
                string termFilter = string.Join(" OR ", dt.Columns.Cast<DataColumn>()
                                                    .Select(c => $"CONVERT([{c.ColumnName}], 'System.String') LIKE '%{term}%'"));
                filters.Add(termFilter);
            }

            string combinedFilter = string.Join(" AND ", filters);

            return combinedFilter;
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (!e.Cancel && this.IsVisible)
            {
                e.Cancel = true;
                this.Hide();
                tutorWindow.Show();
            }
        }

        private void btnAppStudents_Click(object sender, RoutedEventArgs e)
        {
            if (dgvExam.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvExam.SelectedItem;
                LanguageEnum.Language language = (LanguageEnum.Language)Enum.Parse(typeof(LanguageEnum.Language), selectedRow["Language"].ToString());
                LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)Enum.Parse(typeof(LanguageLevelEnum.LanguageLevel), selectedRow["LanguageLevel"].ToString());
                List<Student> students = GetStudentsForCourseAndLevel(tutorRepository.GetById(tutor.Id), language, languageLevel);
                StudentAppliedOnExamWindow updateForm = new StudentAppliedOnExamWindow(tutor);
                updateForm.ShowDialog();
                LoadDataFromCSV();
            }
            else
            {
                MessageBox.Show("First select the row to see list of students.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private List<Student> GetStudentsForCourseAndLevel(Tutor tutor, LanguageEnum.Language language, LanguageLevelEnum.LanguageLevel languageLevel)
        {
            List<Student> studentsForCourseAndLevel = new List<Student>();
            foreach (Student student in studentRepository.GetAll())
            {
                foreach (Course course in courseRepository.GetAll())
                {
                    if (tutorRepository.GetById(tutor.Id) == tutorRepository.GetById(tutor.Id) && course.Language == language && course.LanguageLevel == languageLevel)
                    {
                        studentsForCourseAndLevel.Add(student);
                        break; 
                    }
                }
            }
            return studentsForCourseAndLevel;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            ExamCreateWindow createExamForm = new ExamCreateWindow(tutor);
            createExamForm.ShowDialog();
        }

        //pagination
    } 

}
