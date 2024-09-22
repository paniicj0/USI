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
    /// Interaction logic for StartExamWindow.xaml
    /// </summary>
    public partial class StartExamWindow : Window
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
        private Tutor tutor;
        public TutorWindow tutorWindow;
        List<Exam> exams;
        public StartExamWindow(Tutor tutor)
        {
            InitializeComponent();
            examRepository = ExamRepository.GetInstance();
            examRepository.ExamAdded += UpdateAddedExam;
            this.tutor = tutor;
            tutorWindow = new TutorWindow(tutor);
        }

        private void UpdateAddedExam(object sender, EventArgs e)
        {
            LoadDataFromCSV();
        }

        private void btnAdd_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExamCreateWindow createExamForm = new ExamCreateWindow(tutor);
            createExamForm.ShowDialog();
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


        private void StartExamWindow1_Loaded(object sender, RoutedEventArgs e)
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

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Language", typeof(string));
            dataTable.Columns.Add("LanguageLevel", typeof(string));
            dataTable.Columns.Add("NumOfStudents", typeof(int));
            dataTable.Columns.Add("ExamDate", typeof(DateTime));
            dataTable.Columns.Add("ExamDuration", typeof(int));
            dataTable.Columns.Add("NumOfAppliedStudents", typeof(int));

            examController = new ExamController();
            List<Exam> exams = examController.GetAll();

            DateTime currentDate = DateTime.Now;
            foreach (Exam exam in exams)
            {
                if (tutor.Id == exam.TutorId && !exam.Confirmed)
                {
                    if (exam.ExamDate >= currentDate && exam.ExamDate <= currentDate.AddDays(7))
                    {
                        string language = exam.Language.ToString();
                        string languageLevel = exam.LanguageLevel.ToString();
                        dataTable.Rows.Add(
                            exam.Id,
                            language,
                            languageLevel,
                            exam.NumOfStudents,
                            exam.ExamDate,

                            exam.ExamDuration,
                            exam.NumberOfAppliedStudents
                        );

                    }
                        
                }
                

            }

            dgvExam.ItemsSource = dataTable.DefaultView;
            dgvExam.UnselectAllCells();
        }
        

        private void dgvExam_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvExam.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)dgvExam.SelectedItem;
                selectedId = (int)selectedRow["Id"];
                selectedExamDate = (DateTime)selectedRow["ExamDate"];
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

        private void btnInsight_Click(object sender, RoutedEventArgs e)
        {
            if (dgvExam.SelectedItem != null)
            {
                //send reference
                TimeSpan timeUntilExam = selectedExamDate - DateTime.Today;
                if (timeUntilExam.TotalDays >= 7)
                {
                    MessageBox.Show("You cannot insight students request for this exam because there are more than 7 days left until the exam starts.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                AppliedExamWindow appliedExamWindow = new AppliedExamWindow(selectedId, tutor, this);
                appliedExamWindow.ShowDialog();
                LoadDataFromCSV();
            }
            else
            {
                MessageBox.Show("First select the row that you want to update.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            TutorWindow tutorWindow = new TutorWindow(tutor);
            tutorWindow.ShowDialog();
        }


    }
}
