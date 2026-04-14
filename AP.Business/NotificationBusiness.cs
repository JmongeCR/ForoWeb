using System.Collections.Generic;
using AP.Data;
using AP.Models;

namespace AP.Business
{
    // Capa intermedia entre el controller y el repositorio
    // siguiendo el mismo patron que el resto del proyecto
    public class NotificationBusiness
    {
        private readonly NotificationRepository _repo = new NotificationRepository();

        public List<Notification> GetByUser(int userId) => _repo.GetByUser(userId);

        public int GetUnreadCount(int userId) => _repo.GetUnreadCount(userId);

        public void MarkAllRead(int userId) => _repo.MarkAllRead(userId);

        public void Notify(int userId, string message, string link = null)
        {
            _repo.Create(new Notification
            {
                UserId  = userId,
                Message = message,
                Link    = link
            });
        }
    }
}
