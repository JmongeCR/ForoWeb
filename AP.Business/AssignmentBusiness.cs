using System.Collections.Generic;
using AP.Data;
using AP.Models;

namespace AP.Business
{
    public class AssignmentBusiness
    {
        private readonly AssignmentRepository _repo = new AssignmentRepository();

        public List<Assignment> GetAssignments()
        {
            return _repo.GetAssignments();
        }

        public Assignment GetAssignmentById(int id)
        {
            return _repo.GetAssignmentById(id);
        }

        public void CreateAssignment(Assignment model)
        {
            _repo.CreateAssignment(model);
        }
    }
}
