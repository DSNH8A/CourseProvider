using CourseProvider.Interface;
using CourseProvider.Data;
using CourseProvider.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseSpreader.Repository 
{
    public class UserRepository : IUserRepository {
      
        private readonly CourseProviderContext _context;

        public UserRepository(CourseProviderContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public void Add(User user) 
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User GetById(int id)
        {
            return _context.Users.Where(u => u.Id == id).FirstOrDefault();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _context.Users.Where(u => u.UserName == name).FirstOrDefaultAsync();
        }

        public async Task AddCourse(int courseId, User user)
        {
            user.Courses.Add(courseId);
            await _context.SaveChangesAsync();
        }

        public async Task AddSkill(int skillId, User user)
        {
            user.Skills.Add(skillId);
            await _context.SaveChangesAsync();
        }

        public bool Login(User user)
        {
            //passwordCheck

            foreach (var item in _context.Users)
            {
                if (item.Password == user.Password)
                {
                    //password already exists
                    return true;
                }

                else 
                {
                    //checked free to proceed
                    return false;
                }
            }

            return false;
        }

        public async void Delete(User user)
        {
           _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
