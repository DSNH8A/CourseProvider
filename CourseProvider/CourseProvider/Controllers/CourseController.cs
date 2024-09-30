using CourseProvider.Interface;
using CourseProvider.Models;
using CourseProvider.Data;
using Microsoft.AspNetCore.Mvc;
using CourseProvider.Services;
using CourseProvider.ViewModel;
using Microsoft.AspNetCore.ResponseCaching;

namespace CourseProvider.Controllers {
    public class CourseController : Controller {
        private readonly ICourseRepository _courseRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IphotoService _photoService;
        private List<Skill> skills = new List<Skill>();

        public CourseController(CourseProviderContext _context, ICourseRepository courseRepository, 
            ISkillRepository skillRepository, IphotoService photoService, IMaterialRepository materialRepository)
        {
            _courseRepository = courseRepository;
            _skillRepository = skillRepository;
            _photoService = photoService;
            _materialRepository = materialRepository;
            //_cookieHandler = cookieHandler;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses;

            if (!ModelState.IsValid)
            {
                return View("Error", "Shared");
            }

            try
            {
                if (_courseRepository != null)
                {
                    courses = await _courseRepository.GetAll();
                }

                else
                {
                    throw new Exception("Repository is null!!!");
                }

                if (courses != null)
                {
                    return View("Index", courses);
                }
            }

            catch (Exception exeption)
            {
                Console.WriteLine(exeption);
                return View("empty");
            }


            return View("Error");
        }

        [HttpGet]
        public IActionResult AddCourse()
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(CreateCourseViewModel course)
        {
            if (!ModelState.IsValid)
            {
                return View("index");
            }

            try
            {
                if (course != null)
                {
                    var result = await _photoService.AddPhotsAsync(course.Image);

                    Course newCourse = new Course
                    {
                        Name = course.Name,
                        Description = course.Description,
                        Image = result.Url.ToString(),

                    };

                    _courseRepository.Add(newCourse);
                }

                else
                {
                    throw new Exception("Course is null cannot create another one.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }


            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (course != null)
                {
                    return View("Detail", course);
                }

                else
                {
                    throw new Exception("Provided item is null.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Detail(int id)
        {
            Course course;
            Console.WriteLine("CourseId" + id);

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_courseRepository != null && id > 0)
                {
                    course = await _courseRepository.GetByIdAsync(id);
                }

                else
                {
                    throw new Exception("Repository is null");
                }

                if (course != null)
                {
                    return RedirectToAction("Detail", course);
                }

                else
                {
                    throw new Exception("Provided data is insufficient");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (course != null)
                {
                    return View("Index");
                }

                else 
                {
                    throw new Exception("Provided data was insufficient");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Course course;

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_courseRepository != null)
                {
                    course = await _courseRepository.GetByIdAsync(id);
                }

                else
                {
                    throw new Exception("Respository is null.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }

            return RedirectToAction("Delete");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Course course)
        {
            //if (!ModelState.IsValid) 
            //{
            //    Console.WriteLine("ModelState not valid");
            //    return View("Index");   
            //}

            try
            {
                if (course != null)
                {
                    return View("Edit", course);
                }

                else
                {
                    throw new Exception("Provided data is unsufficient");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            Course course;

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_courseRepository != null)
                {
                    course = await _courseRepository.GetByIdAsync(id);
                    //return RedirectToAction("Edit", course);
                }

                else
                {
                    throw new Exception("Repositotry is null");
                }

                if (course != null)
                {
                    return RedirectToAction("Edit", course);
                }

                else 
                {
                    throw new Exception("Data is null");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }
        }

        public async Task<IActionResult> Update(int id, CreateCourseViewModel courseView)
        {
            Course courseToChange;

            CookieHandler<Entity> handler = new CookieHandler<Entity>(Request.Cookies);
            Skill skill = _skillRepository.GetById(handler.GetModel("skill")); 
            Material material = _materialRepository.GetById(handler.GetModel("material"));

            Console.WriteLine("SkillName:" + skill.Name + "SkillId:" + skill.Id);
            Console.WriteLine("MaterialName :" + material.Name +  "MaterialId: " + material.Id);

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState nem Valid");
                return BadRequest();
            }

            try
            {
                if (courseView != null)
                {
                    courseToChange = await _courseRepository.GetByIdAsync(id);

                    var image = await _photoService.AddPhotsAsync(courseView.Image);
                    Course course = new Course
                    {
                        Name = courseView.Name,
                        Description
                        = courseView.Description,
                        Image = image.Url.ToString(),
                    };

                    if (skill.Id > 0)
                    {
                        await _courseRepository.AddSkill(skill, courseToChange);
                    }

                    if (material.Id > 0) 
                    {
                        Console.WriteLine("Material name: " + material.Name);
                        await _courseRepository.AddMaterial(material, courseToChange);
                    }

                    Console.WriteLine("CourseToChange");

                    if (courseToChange != null)
                    {
                        _courseRepository.Update(course, courseToChange);
                    }

                    else throw new Exception("Something is null");
                }

                else throw new Exception("Course is null cant change it");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }


            return RedirectToAction("Index");//("Detail", courseToChange);
        }

        public async Task<IActionResult> AddSkill(int id, int courseId)
        {
            Skill skill;
            Course course;
            //if (!ModelState.IsValid)
            //{
            //    return View("Index");
            //}

            try
            {
                if (_courseRepository != null && _skillRepository != null)
                {
                    skill = await _skillRepository.GetByIdAsync(id);
                    course = await _courseRepository.GetByIdAsync(courseId);


                    if (course == null)
                    {
                        throw new Exception("Selected Course is null");
                    }

                    if (skill == null)
                    {
                        throw new Exception("Selected Item is null.");
                    }
                }

                else
                {
                    throw new Exception("Repositors is null.");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }

            _courseRepository.AddSkill(skill, course);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddMaterial(int id, int courseId)
        {
            Material material;
            Course course;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (id > 0 && courseId > 0)
                {
                    material = await _materialRepository.GetByIdAsync(id);
                    course = await _courseRepository.GetByIdAsync(id);

                    if (material != null && course != null)
                    {
                        await _courseRepository.AddMaterial(material, course);
                        return RedirectToAction("Index");
                    }

                    else 
                    {
                        throw new Exception("Data is null");
                    }
                }

                else 
                {
                    throw new Exception("Insufficient data.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }
        }
            
        public async Task<IActionResult> AddList(int skill, int id)
        {
            Skill newSkill;
            Course course;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (skill > 0 && id > 0)
                {
                    newSkill = await _skillRepository.GetByIdAsync(skill);
                    course = await _courseRepository.GetByIdAsync(id);

                    if (newSkill != null)
                    {
                        CookieHandler<Skill> _cookieHandler = new CookieHandler<Skill>("skill", Response.Cookies);

                        _cookieHandler.SetModelId(newSkill);
                        ViewData["skill"] = newSkill;
                        ViewData["skillName"] = newSkill.Name.ToString();
                        Console.WriteLine("SANYIKA");
                    }

                    else 
                    {
                        throw new Exception("NewSkill is null.");
                    }


                    if (course != null)
                    {
                        Console.WriteLine("itt vagyok");
                        return View("Edit", course);
                    }

                    else 
                    {
                        throw new Exception("Course is null.");
                    }
                }

                else 
                {
                    throw new Exception("Insufficient data.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest();
            }
        }

        public async Task<IActionResult> ChooseMaterial(int material, int id)
        {
            Material newMaterial;
            Course course;
            Console.WriteLine("MaterialID: " + material);
            Console.WriteLine("CourseID: " + id);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (material > 0 && id > 0)
                {
                    newMaterial = await _materialRepository.GetByIdAsync(material);
                    course = await _courseRepository.GetByIdAsync(id);


                    if (newMaterial != null && course != null)
                    {
                        CookieHandler<Material> _cookieHandler = new CookieHandler<Material>("material", Response.Cookies);
                        _cookieHandler.SetModelId(newMaterial);
                        ViewData["material"] = newMaterial;
                        ViewData["materialName"] = newMaterial.Name.ToString();
                        Console.WriteLine(ViewData["material"].ToString());
                        return RedirectToAction("Edit", course);
                    }

                    else 
                    {
                        throw new Exception("Data is null.");
                    }
                }

                else 
                {
                    throw new Exception("Provided ids are not valid.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSkill(int course) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (course > 0)
                {
                    Course newCourse = await _courseRepository.GetByIdAsync(course);

                    if (newCourse != null)
                    {
                        return RedirectToAction("Detail", course);
                    }

                    else 
                    {
                        throw new Exception("Course Was null.");
                    }
                }

                else 
                {
                    throw new Exception("Id was invalid.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return RedirectToAction("Detail", course);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSkill(int skill, int course)
        {
            Course newCourse;

            Console.WriteLine("SkillId: " + skill);
            Console.WriteLine("CourseId: " + course);

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (skill > 0 && course > 0)
                {
                    newCourse = await _courseRepository.GetByIdAsync(course);

                    if (newCourse != null)
                    {
                        await _courseRepository.DeleteSkill(skill, newCourse);
                        return RedirectToAction("Detail", newCourse);
                    }

                    else 
                    {
                        throw new Exception("Course was null.");
                    }
                }

                else 
                {
                    throw new Exception("Id was invalid");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return RedirectToAction("Detail", course);
            }
        }

        public async Task<IActionResult> DeleteMaterial(int material, int course)
        {
            Course newCourse;
            Console.WriteLine("MaterialId: " + material);
            Console.WriteLine("CourseId: " + course);


            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (material > 0 && course > 0)
                {
                    newCourse = await _courseRepository.GetByIdAsync(course);

                    if (newCourse != null)
                    {
                        await _courseRepository.DeleteMaterial(material, newCourse);
                        return RedirectToAction("Detail", course);
                    }

                    else
                    {
                        throw new Exception("Course was null.");
                    }
                }

                else
                {
                    throw new Exception("Id was invalid");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return RedirectToAction("Detail", course);
            }
        }
    }
}
