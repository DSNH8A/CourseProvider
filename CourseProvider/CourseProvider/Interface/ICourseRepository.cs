using CourseProvider.Models;

namespace CourseProvider.Interface 
{
    public interface ICourseRepository 
    {
        public Task<IEnumerable<Course>> GetAll();

        public List<Course> GetAllList();

        public void Add(Course course);

        public Course GetById(int id);

        public ICourseRepository GetRepository(int id);

        public Task<Course> GetByIdAsync(int id);

        public void Delete(Course course);

        public void Update(Course courseWithChanges, Course courseToChange);

        public Task AddSkill(Skill skill, Course course);

        public Task AddCourse(User user, Course course);

        public Task AddMaterial(Material material, Course course);

        public Task<List<Skill>> GetSkills(int Id);

        public Task<List<Skill>> GetSkills();

        public Task DeleteSkill(int id, Course course);

        public Task<List<Material>> GetMaterials(int id);

        public Task DeleteMaterial(int id, Course course);
    }
}
