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
using System.Reflection;
using USIProject.View;
using static LangLang.ModelEnum.LanguageLevelEnum;
using static LangLang.ModelEnum.RealizationEnum;
using System.ComponentModel;

namespace LangLang.View
{
    /// <summary>
    /// Interaction logic for GivePenaltyPoint.xaml
    /// </summary>
    public partial class GivePenaltyPoint : Window
    {
        Student student;
        StudentController studentController = new StudentController();
        NotificationController notificationController = new NotificationController();
        int idCourse;
        public GivePenaltyPoint(Student student, int IdCourse)
        {
            InitializeComponent();
            this.student = student;
            this.idCourse = IdCourse;
        }

        private void GivePenaltyPointWindow_Loaded(object sender, RoutedEventArgs e)
        {
            FillComboBoxWithMessages();
        }

        private void FillComboBoxWithMessages()
        {
            Array messages = Enum.GetValues(typeof(PenaltyPointMessageEnum.PenaltyPointMessage));

            foreach (var message in messages)
            {
                cbMessage.Items.Add(message);
            }
        }

        private void btnGive_Click(object sender, RoutedEventArgs e)
        {
            student.PenaltyPoints += 1;
            studentController.Update(student);

            PenaltyPointMessageEnum.PenaltyPointMessage? message = cbMessage.SelectedItem as PenaltyPointMessageEnum.PenaltyPointMessage?;

            string messageString;

            if (message != null)
            {
                messageString = GetEnumDescription(message.Value);
            }
            else
            {
                messageString = "Unknown penalty point message.";
            }

            Notification notification = new Notification(-1, student.Id, idCourse, false, messageString);
            notificationController.Create(notification);

            this.Close();
        }

        private string GetEnumDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }

        }
}
