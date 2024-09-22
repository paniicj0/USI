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
using static LangLang.ModelEnum.LanguageEnum;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for TutorGradeStudentWindow.xaml
    /// </summary>
    public partial class TutorGradeStudentWindow : Window
    {
        private AppliedExam selectedAppliedExam;
        private AppliedExamController appliedExamController = new AppliedExamController();

        public TutorGradeStudentWindow(AppliedExam selectedAppliedExam, string name)
        {
            InitializeComponent();
            this.selectedAppliedExam = selectedAppliedExam;
            lblStudentName.Content = name;
        }
    

        private void TutorGradeStudentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithGrades();
            
        }

        private void FillComboBoxWithGrades()
        {
            int[] grades = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (int grade in grades)
            {
                cbGrade.Items.Add(grade);
                cbGradeActivity.Items.Add(grade);
            }
        }

        private void btnGrade_Click(object sender, RoutedEventArgs e)
        {
            int gradeKnowledge = 0;
            int gradeActivity = 0;
            int.TryParse(cbGrade.Text, out gradeKnowledge);

            int.TryParse(cbGradeActivity.Text, out gradeActivity); 

            if (gradeKnowledge == 0 || gradeActivity == 0)
            {
                MessageBox.Show("Please select a grade first.", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            selectedAppliedExam.Grade = gradeKnowledge;
            selectedAppliedExam.GradeActivity = gradeActivity;
            appliedExamController.Update(selectedAppliedExam);
            this.Close();
        }
    }
}
