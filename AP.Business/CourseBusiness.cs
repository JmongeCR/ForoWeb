using AP.Data;
using AP.Models;
using System.Collections.Generic;

namespace AP.Business
{
    public class CourseBusiness
    {
        private readonly CourseRepository _repo = new CourseRepository();

        public List<Course> GetAll() => _repo.GetAll();

        public Course GetById(int id) => _repo.GetById(id);

        public void Create(Course c) => _repo.Create(c);

        public void Update(Course c) => _repo.Update(c);
    }
}
