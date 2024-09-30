using CourseProvider.Models; 

namespace CourseProvider.Interface 
{
    public interface IUserRepository 
    {
        public Task<IEnumerable<User>> GetAll();

        public void Add(User user);

        public User GetById(int id);

        public Task<User> GetByIdAsync(int id);

        public Task<User> GetByNameAsync(string name);

        public Task AddCourse(int courseId, User user);

        public Task AddSkill(int skillId, User user);

        public bool Login(User user);

        public void Delete(User user);
    }
}
