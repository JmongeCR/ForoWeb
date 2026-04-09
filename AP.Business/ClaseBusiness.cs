using AP.Data;
using AP.Models;
using System.Collections.Generic;

namespace AP.Business
{
    public class ClaseBusiness
    {
        private readonly ClaseRepository _repo = new ClaseRepository();
        private readonly UserRepository _userRepo = new UserRepository();

        public List<Clase> GetAll() => _repo.GetAll();

        public Clase GetById(int id) => _repo.GetById(id);

        public List<Clase> GetByProfessor(int professorId) => _repo.GetByProfessor(professorId);

        public List<Clase> GetByStudent(int studentId) => _repo.GetByStudent(studentId);

        public void Create(Clase c) => _repo.Create(c);

        public void Update(Clase c) => _repo.Update(c);

        public List<User> GetStudents(int claseId) => _repo.GetStudents(claseId);

        public List<User> GetAvailableStudents(int claseId) => _repo.GetAvailableStudents(claseId);

        public void AddStudent(int claseId, int studentId) => _repo.AddStudent(claseId, studentId);

        public void RemoveStudent(int claseId, int studentId) => _repo.RemoveStudent(claseId, studentId);

        public bool IsStudentInClase(int claseId, int studentId) => _repo.IsStudentInClase(claseId, studentId);

        public List<User> GetAllProfessors() =>
            _userRepo.GetUsers().FindAll(u => u.Role == "Profesor" && u.IsActive);
    }
}
