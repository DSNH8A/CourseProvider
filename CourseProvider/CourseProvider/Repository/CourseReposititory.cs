using CourseProvider.Interface;
using CourseProvider.Models;
using CourseProvider.Data;
using Microsoft.EntityFrameworkCore;

namespace CourseSpreader.Repository 
{
    public class CourseReposititory : ICourseRepository {
       
        private readonly CourseProviderContext _context;


        public CourseReposititory(CourseProviderContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Course>> GetAll()
        {
            return await _context.Courses.ToListAsync();
        }

        public List<Course> GetAllList() 
        {
            return _context.Courses.ToList();
        }

        public void Add(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public Course GetById(int id)
        {
            return _context.Courses.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICourseRepository GetRepository(int id) 
        {
            return _context.Courses.Where(c => c.Id == id).FirstOrDefault() as ICourseRepository;
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public void Delete(Course course)
        {
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }

        public void Update(Course courseWithChanges, Course courseToChange) 
        {

            Console.WriteLine("Update function");
            courseToChange.Name = courseWithChanges.Name;
            courseToChange.Description = courseWithChanges.Description;
            courseToChange.Image = courseWithChanges.Image; 

            //if (courseToChange.Skills != null &&courseWithChanges.Skills != null)
            //{
            //    courseToChange.Skills.Add(courseWithChanges.Skills.Last());
            //}

            //if (courseToChange.Users != null && courseWithChanges.Users != null)
            //{
            //    courseToChange.Users.Add(courseWithChanges.Users.Last());
            //}

            //if (courseToChange.Materails != null && courseWithChanges.Materails != null)
            //{
            //    courseToChange.Materails.Add(courseWithChanges.Materails.Last());
            //}

            Console.WriteLine("Changes Saved");
            _context.SaveChanges();
        }

        public async Task AddSkill(Skill skill, Course course)
        {
            if (skill == null || course == null)
            {
                Console.WriteLine("Valami null");
            }

            try
            {
                course.Skills.Add(skill.Id);
                skill.Courses.Add(course.Id);
                   await _context.SaveChangesAsync();

                if(course.Skills == null)
                {
                    throw new Exception("SkillsList is null");
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task AddMaterial(Material material, Course course)
        {
            try 
            {
                if (course != null && material != null)
                {
                    course.Materails.Add(material.Id);
                    material.Courses.Add(course.Id);
                    await _context.SaveChangesAsync();
                }

                else 
                {
                    throw new Exception("Data provided is null.");
                }

            }

            catch(Exception exception)
            { 
                Console.WriteLine(exception); 
            }
        }

        public async Task<List<Skill>> GetSkills()
        {
            return await _context.Skills.ToListAsync(); 
        }

        public async Task<List<Skill>> GetSkills(int Id) //courseId
        {
            return await _context.Skills.Where(s => s.Courses.Contains(Id)).ToListAsync();
        }

        public async Task DeleteSkill(int id, Course course)
        {
            course.Skills.Remove(course.Id);
            Console.WriteLine("Skill Removed");
             _context.SaveChangesAsync();
        }

        public async Task<List<Material>> GetMaterials(int id)
        {
            return await _context.Materials.Where(m => m.Courses.Contains(id)).ToListAsync();
        }

        public async Task DeleteMaterial(int id, Course course)
        {
            course.Materails.Remove(course.Id);
            await _context.SaveChangesAsync();
        }

        public async Task AddCourse(User user, Course course)
        {
            course.Users.Add(user.Id);
            await _context.SaveChangesAsync();
        }
    }
}
