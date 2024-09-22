using LangLang.Model;
using LangLang.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLang.Service
{
    internal class NotificationService
    {
        private static NotificationService instance = null;
        private NotificationRepository notificationRepository;
        private List<Notification> notifications;

        private NotificationService()
        {
            notificationRepository = NotificationRepository.GetInstance();
            this.notifications = notificationRepository.GetAll();
        }

        public static NotificationService GetInstance()
        {
            if (instance == null)
            {
                instance = new NotificationService();
            }
            return instance;
        }

        public Notification GetById(int id)
        {
            return notificationRepository.GetById(id);
        }

        public List<Notification> GetAll()
        {
            return notificationRepository.GetAll();
        }

        public Notification Create(Notification notification)
        {
            return notificationRepository.Create(notification);
        }

        public void Update(Notification notification)
        {
            notificationRepository.Update(notification);
        }

        public void Delete(int id)
        {
            notificationRepository.Delete(id);
        }

     
        public List<Notification> LoadFromFile()
        {
            return notificationRepository.LoadFromFile();
        }

        public List<Notification> GetByCourseId(int courseId)
        {
            List<Notification> selectedNotifications = new List<Notification>();
            foreach (Notification notification in notifications)
            {
                if (notification.IdCourse == courseId)
                {
                    selectedNotifications.Add(notification);
                }
            }
            return selectedNotifications;
        }

        public int CountUnreadMessages(int studentId)
        {
            notifications = notificationRepository.GetAll();
            int unreadMessages = 0;
            foreach (Notification notification in notifications)
            {
                if (notification.Whom == studentId && notification.Read == false)
                {
                    unreadMessages++;
                }
            }
            return unreadMessages;
        }
    }
}
