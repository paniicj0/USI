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
    /// Interaction logic for ManageExamForm.xaml
    /// </summary>
    public partial class ExamUpdateWindow : Window
    {
        private ExamController examController;
        // private Exam examForUpdate;
        private object callingForm;
        private Tutor tutor;

        public ExamUpdateWindow(ExamDisplayWindow examDisplayWindow, Tutor tutor)
        {
            InitializeComponent();
            this.callingForm = examDisplayWindow;
            this.tutor = tutor;
        }
        public ExamUpdateWindow(StartExamWindow startExamWindow, Tutor tutor)
        {
            InitializeComponent();
            this.callingForm = startExamWindow;
            this.tutor = tutor;
        }

        /*private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbLanguage.SelectedItem != null && cbLanguageLevel.SelectedItem != null && !string.IsNullOrWhiteSpace(tbNumOfStudents.Text) && dtpExam.SelectedDate.HasValue)
            //ManageExamForm manageExamForm = new ManageExamForm();
            {
                int id = manageExamForm.selectedId;
                //Console.WriteLine(id);
                LanguageEnum.Language language = (LanguageEnum.Language)cbLanguage.SelectedItem;
                LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)cbLanguageLevel.SelectedItem;
                int students = int.Parse(tbNumOfStudents.Text);
                DateTime? dateExam = dtpExam.SelectedDate;

                //i need to get applied students count and add it to the last paramether
                Exam exam = new Exam(id, language, languageLevel, students, dateExam.Value, 4, tutor.Id, -1);
                examController.Update(exam);
                //Dispose();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }*/
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (cbLanguage.SelectedItem != null && cbLanguageLevel.SelectedItem != null && !string.IsNullOrWhiteSpace(tbNumOfStudents.Text) && dtpExam.SelectedDate.HasValue)
            {
                // Proveravamo tip pozivajućeg prozora
                if (callingForm is ExamDisplayWindow examDisplayWindow)
                {
                    // Ako je pozvano iz ExamDisplayWindow
                    int id = examDisplayWindow.selectedId;
                    LanguageEnum.Language language = (LanguageEnum.Language)cbLanguage.SelectedItem;
                    LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)cbLanguageLevel.SelectedItem;
                    int students = int.Parse(tbNumOfStudents.Text);
                    DateTime? dateExam = dtpExam.SelectedDate;
                    TimeSpan? examTime = ParseExamTime();

                    Exam exam = new Exam(id, language, languageLevel, students, dateExam.Value, examTime.Value, 4, tutor.Id, -1);
                    examController.Update(exam);
                    this.Close();
                }
                else if (callingForm is StartExamWindow startExamWindow)
                {
                    int id = startExamWindow.selectedId;
                    LanguageEnum.Language language = (LanguageEnum.Language)cbLanguage.SelectedItem;
                    LanguageLevelEnum.LanguageLevel languageLevel = (LanguageLevelEnum.LanguageLevel)cbLanguageLevel.SelectedItem;
                    int students = int.Parse(tbNumOfStudents.Text);
                    DateTime? dateExam = dtpExam.SelectedDate;
                    TimeSpan? examTime = ParseExamTime();

                    Exam exam = new Exam(id, language, languageLevel, students, dateExam.Value, examTime.Value, 4, tutor.Id, -1);
                    examController.Update(exam);
                    this.Close();
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


        private void FillComboBoxWithLanguage()
        {
            Array language = Enum.GetValues(typeof(LanguageEnum.Language));

            foreach (var level in language)
            {
                cbLanguage.Items.Add(level);
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

       /* private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevels();
            //dtpExam.MinDate = DateTime.Today;
            dtpExam.DisplayDateStart = DateTime.Today;
            examController = new ExamController();
            //cbLanguage.SelectedItem=manageExamForm.sel;
            cbLanguage.SelectedItem = manageExamForm.selectedLanguage;
            cbLanguageLevel.SelectedItem = manageExamForm.selectedLanguageLevel;

            tbNumOfStudents.Text = manageExamForm.selectedNumOfStudents.ToString();
            dtpExam.SelectedDate = manageExamForm.selectedExamDate;
        }*/

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithLanguage();
            FillComboBoxWithLanguageLevels();
            dtpExam.DisplayDateStart = DateTime.Today;

            if (callingForm is ExamDisplayWindow examDisplayWindow)
            {
                cbLanguage.SelectedItem = examDisplayWindow.selectedLanguage;
                cbLanguageLevel.SelectedItem = examDisplayWindow.selectedLanguageLevel;
                tbNumOfStudents.Text = examDisplayWindow.selectedNumOfStudents.ToString();
                dtpExam.SelectedDate = examDisplayWindow.selectedExamDate;
            }
            else if (callingForm is StartExamWindow startExamWindow)
            {
                cbLanguage.SelectedItem = startExamWindow.selectedLanguage;
                cbLanguageLevel.SelectedItem = startExamWindow.selectedLanguageLevel;
                tbNumOfStudents.Text = startExamWindow.selectedNumOfStudents.ToString();
                dtpExam.SelectedDate = startExamWindow.selectedExamDate;
            }
        }

    }
}
