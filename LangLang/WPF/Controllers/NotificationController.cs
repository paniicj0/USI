using LangLang.Model;
using LangLang.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Controllers
{
    public class NotificationController
    {
        private NotificationService notificationService;

        public NotificationController()
        {
            notificationService = NotificationService.GetInstance();
        }

        public List<Notification> GetAll()
        {
            return notificationService.GetAll();
        }

        public Notification GetById(int id)
        {
            return notificationService.GetById(id);
        }

        public Notification Create(Notification notification)
        {
            return notificationService.Create(notification);
        }

        public void Update(Notification notification)
        {
            notificationService.Update(notification);
        }

        public void Delete(int id)
        {
            notificationService.Delete(id);
        }

        public List<Notification> LoadFromFile()
        {
            return notificationService.LoadFromFile();
        }

        public List<Notification> GetByCourseId(int courseId)
        {
            return notificationService.GetByCourseId(courseId);
        }

        public int CountUnreadMessages(int studentId)
        {
            return notificationService.CountUnreadMessages(studentId);
        }
    }
}
