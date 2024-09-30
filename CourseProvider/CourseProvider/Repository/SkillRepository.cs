using CourseProvider.Interface;
using CourseProvider.Models;
using CourseProvider.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseSpreader.Repository 
{
    public class SkillRepository : ISkillRepository {

        private readonly CourseProviderContext _context;

        public SkillRepository(CourseProviderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Skill>> GetAll()
        {
            return await _context.Skills.ToListAsync();
        }

        public async Task<List<Skill>> GetAllList()
        {
            return await _context.Skills.ToListAsync();
        }

        public void Add(Skill skill)
        {
            _context.Skills.Add(skill);
            _context.SaveChanges();
        }

        public Skill GetById(int id)
        {
            return _context.Skills.Where(s => s.Id == id).FirstOrDefault();
        }

        public async Task<Skill> GetByIdAsync(int id)
        {
            return await _context.Skills.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public void Update(Skill skillToChange, Skill skillWithChanges) 
        {
            if (skillToChange != null && skillWithChanges != null)
            {
                skillToChange.Name = skillWithChanges.Name;
                _context.SaveChanges();
            }
        }

        public void Delete(Skill skill)
        {
            _context.Skills.Remove(skill);
            _context.SaveChanges();
        }
    }
}
