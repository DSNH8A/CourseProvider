using CourseProvider.Interface;
using CourseProvider.Models;
using Microsoft.AspNetCore.Mvc;
using CourseProvider.Data;

namespace CourseProvider.Controllers {
    public class SkillController : Controller {
        private readonly ISkillRepository _skillRepository;

        public SkillController(CourseProviderContext _context, ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Skill> skills;

            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            try
            {
                if (_skillRepository != null)
                {
                    skills = await _skillRepository.GetAll();
                }

                else
                {
                    throw new Exception("There are no skills to show.");
                }

                if (skills != null)
                {
                    return View("Index", skills);
                }

                else 
                {
                    throw new Exception("Data is null.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("empty");
            }
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (skill != null)
                {
                    _skillRepository.Add(skill);
                    return RedirectToAction("Index");
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

        [HttpGet]
        public async Task<IActionResult> Detail(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (skill != null)
                {
                    return View(skill);
                }

                else 
                {
                    throw new Exception("Data is null.");
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
            Skill skill;

            if(!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_skillRepository != null)
                {
                    skill = await _skillRepository.GetByIdAsync(id);
                }

                else 
                {
                    throw new Exception("Respository is null.");
                }

                if (skill == null)
                {
                    throw new Exception("Cannot open item details");
                }
            }
            catch (Exception exception)
            { 
                Console.WriteLine(exception);
                return View("Index");
            }

            return RedirectToAction("Detail", skill);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Skill skill)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            try
            {
                if (skill != null)
                {
                    return View(skill);
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

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            Skill skill;

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_skillRepository != null)
                {
                    skill = await _skillRepository.GetByIdAsync(id);
                }

                else 
                {
                    throw new Exception("Respository is null.");
                }

                if(skill == null)
                {
                    throw new Exception("Cannot edit this item.");
                }
            }
            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return View("Index");
            }

            return RedirectToAction("Edit", skill);
        }

        public async Task<IActionResult> Update(int id, Skill skill)
        {
            Skill skillToChange;

            if (!ModelState.IsValid)
            {
                return View("Index");   
            }

            try
            {
                if (id > 0 && skill != null)
                {
                    skillToChange = await _skillRepository.GetByIdAsync(id);
                    _skillRepository.Update(skillToChange, skill);
                }

                else
                {
                    throw new Exception("Provided data is sufficient");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
                return View("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Skill skill;

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_skillRepository != null && id > 0)
                {
                    skill = await _skillRepository.GetByIdAsync(id);

                    if (skill != null)
                    {
                        _skillRepository.Delete(skill);
                    }

                    else 
                    {
                        throw new Exception("Cannot delete skill because it is null.");
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
                return View("Index");
            }

            return RedirectToAction("Index");
        }
    }
}
