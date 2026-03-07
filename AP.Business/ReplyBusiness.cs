using System;
using AP.Data;
using AP.Models;

namespace AP.Business
{
    public class ReplyBusiness
    {
        private readonly ReplyRepository _repo = new ReplyRepository();

        public void CreateReply(Reply r)
        {
            r.CreatedAt = DateTime.Now;
            _repo.CreateReply(r);
        }
    }
}