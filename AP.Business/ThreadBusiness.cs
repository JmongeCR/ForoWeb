using System;
using System.Collections.Generic;
using AP.Data;
using AP.Models;

namespace AP.Business
{
    // DP: Facade - agrupa en un solo lugar las operaciones de hilos, respuestas y categorias
    // SOLID: DIP - los controllers dependen de esta clase y no de los repositorios directamente
    public class ThreadBusiness
    {
        private readonly ThreadRepository _threadRepo = new ThreadRepository();
        private readonly ReplyRepository _replyRepo = new ReplyRepository();
        private readonly CategoryRepository _categoryRepo = new CategoryRepository();

        public List<Thread> GetThreads() => _threadRepo.GetThreads();

        public Thread GetThread(int id) => _threadRepo.GetThread(id);

        public List<Reply> GetReplies(int threadId) => _replyRepo.GetRepliesByThread(threadId);

        public void CreateThread(Thread t)
        {
            t.CreatedAt = DateTime.Now;
            _threadRepo.CreateThread(t);
        }
        public List<Thread> GetThreadsByCategory(int categoryId)
        {
            return _threadRepo.GetThreadsByCategory(categoryId);
        }

        public void SetClosed(int threadId, bool closed) => _threadRepo.SetClosed(threadId, closed);

        public string GetCategoryName(int categoryId)
        {
            var cat = _categoryRepo.GetAll().Find(c => c.CategoryId == categoryId);
            return cat != null ? cat.Name : "Canal";
        }
    }
}