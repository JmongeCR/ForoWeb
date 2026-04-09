using AP.Data;
using AP.Models;
using System.Collections.Generic;

namespace AP.Business
{
    public class AssignmentBusiness
    {
        private readonly AssignmentRepository _assignRepo = new AssignmentRepository();
        private readonly SubmissionRepository _subRepo = new SubmissionRepository();
        private readonly ClaseRepository _claseRepo = new ClaseRepository();

        public List<Assignment> GetByProfessor(int professorId) =>
            _assignRepo.GetByProfessor(professorId);

        public List<Assignment> GetByStudent(int studentId) =>
            _assignRepo.GetByStudent(studentId);

        public Assignment GetById(int id) =>
            _assignRepo.GetById(id);

        public int Create(Assignment a) =>
            _assignRepo.Create(a);

        public List<User> GetAssignedStudents(int assignmentId) =>
            _assignRepo.GetAssignedStudents(assignmentId);

        public bool IsStudentAssigned(int assignmentId, int studentId) =>
            _assignRepo.IsStudentAssigned(assignmentId, studentId);

        public List<Clase> GetProfessorClases(int professorId) =>
            _claseRepo.GetByProfessor(professorId);

        public List<Submission> GetSubmissions(int assignmentId) =>
            _subRepo.GetByAssignment(assignmentId);

        public Submission GetMySubmission(int studentId, int assignmentId) =>
            _subRepo.GetByStudentAndAssignment(studentId, assignmentId);

        public void Submit(Submission s) =>
            _subRepo.Create(s);
    }
}
