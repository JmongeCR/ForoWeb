using System;
using System.Collections.Generic;
using AP.Data;
using AP.Models;

namespace AP.Business
{
    public class ThreadBusiness
    {
        private readonly ThreadRepository _threadRepo = new ThreadRepository();
        private readonly ReplyRepository _replyRepo = new ReplyRepository();

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
    }
}