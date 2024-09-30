using Microsoft.AspNetCore.Mvc;
using CourseProvider.Interface;
using CourseProvider.Data;
using CourseProvider.Models;
using CourseProvider.Wrapper;

namespace CourseProvider.Controllers 
{
    public class UserController  : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly ISkillRepository _skillRepository;
        private MySession _session;

        public UserController(CourseProviderContext _context, IUserRepository userRepository, 
            ICourseRepository courseRepository, ISkillRepository skillRepository) 
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
            _skillRepository = skillRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<User> users;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_userRepository != null)
                {
                    users = await _userRepository.GetAll();

                    if (users != null)
                    {
                        return View("Index");
                    }

                    else 
                    {
                        throw new Exception("Cannot fetch data.");
                    }
                }

                else 
                {
                    throw new Exception("Respository is null.");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (user != null)
                {
                    return View("Detail", user);
                }

                else 
                {
                    throw new Exception("Provided data is insufficient");
                }
            }
            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return RedirectToAction("Index");   
            }
        }

        [HttpPost]
        public async Task<IActionResult> Detail(int id)
        {
            User user;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (_userRepository != null && id > 0)
                {
                    user = await _userRepository.GetByIdAsync(id);

                    if (user != null)
                    {
                        return RedirectToAction("Detail", user);
                    }

                    else
                    {
                        throw new Exception("User is null");
                    }
                }

                else
                {
                    throw new Exception("Respository is null");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }

            _session = new MySession(HttpContext.Session);
            User user;

            if (_session != null)
            {
                user = _session.GetUser();
                return RedirectToAction("Detail", user);
            }

            return BadRequest(); 
        }

        [HttpPost]
        public async Task<IActionResult> Add(User user)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("modelstate nem vakid");
                return RedirectToAction("Index", "Home");
            }

            try
            {
                if (_userRepository != null)
                {
                    if (user != null)
                    {
                        user.DateOfJoining = DateTime.Now;  
                        _userRepository.Add(user);
                        return RedirectToAction("Detail", user);
                    }

                    else 
                    {
                        throw new Exception("Data was null.");
                    }
                }

                else 
                {
                    throw new Exception("Repository is null or id is invalid");
                }

            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserDTO user)
        {
            _session = new MySession(HttpContext.Session);

            if (!ModelState.IsValid)
            {
                Console.WriteLine("modelstate nem valid");
                return RedirectToAction("Index", "Home");
            }

            try
            {
                if (user.UserName != null)
                {
                    User newUser = await _userRepository.GetByNameAsync(user.UserName);

                    if (newUser != null)
                    {
                        var passwordCheck = _userRepository.Login(newUser);

                        if (passwordCheck == false)
                        {
                            if (newUser.IsActive == false || newUser.IsActive == null)
                            {
                                newUser.IsActive = true;

                                _session.SetUser(newUser);
                                return RedirectToAction("Login");
                            }
                        }

                        else
                        {
                            throw new Exception("Password is already used.");
                            return RedirectToAction("Index");
                        }
                    }
                }

                else
                {
                    throw new Exception("Provided data is insuficient");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return BadRequest();
            }

            Console.WriteLine("Saniyka");
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            _session = new MySession(HttpContext.Session);

            if (!ModelState.IsValid) 
            {
                Console.WriteLine("Modelstate nwm valid");
                return RedirectToAction("Error");
            }

            try
            {
                if (_session.GetUser() != null)
                {
                    return View("Detail", _session.GetUser());
                }

                else 
                {
                    throw new Exception("Session user is null");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return RedirectToAction("Error");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddCourse(int id)
        {
            _session = new MySession(HttpContext.Session); 

            if (!ModelState.IsValid)
            {
                Console.Write("sanyika");
                return BadRequest();    
            }

            try
            {
                if (_userRepository != null && id > 0)
                {
                    User user = await _userRepository.GetByIdAsync(_session.GetUser().Id);
                    Course course = await _courseRepository.GetByIdAsync(id);
                    if (user != null && course != null)
                    {
                        await _courseRepository.AddCourse(user, course);
                        await _userRepository.AddCourse(course.Id, user);

                        return RedirectToAction("Detail", user);
                    }

                    else 
                    {
                        throw new Exception("User or course is null");
                    }
                }

                else
                {
                    throw new Exception("Repository is null or id is not valid.");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddSkill(int id)
        {
            Skill skill;

            MySession session = new MySession(HttpContext.Session);

            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (_userRepository != null && id > 0)
                {
                    skill = await _skillRepository.GetByIdAsync(id);
                    User user = session.GetUser();

                    if (skill != null && user != null)
                    {
                        await _userRepository.AddSkill(skill.Id, user);
                        return RedirectToAction("Detail", user);
                    }

                    else 
                    {
                        throw new Exception("Skill is not null.");
                    }
                }

                else
                {
                    throw new Exception("Respository is null or id is null.");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return BadRequest();
            }
        }
    }
}
