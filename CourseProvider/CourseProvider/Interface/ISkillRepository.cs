using CourseProvider.Models;

namespace CourseProvider.Interface {
    public interface ISkillRepository 
    {
        public Task<IEnumerable<Skill>> GetAll();

        public Task<List<Skill>> GetAllList();

        public void Add(Skill skill);

        public Skill GetById(int id);

        public Task<Skill> GetByIdAsync(int id);

        public Task<List<User>> GetUsers();

        public void Update(Skill skillToChange, Skill skillWithChanges);

        public void Delete(Skill skill);
    }
}
