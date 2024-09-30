using CourseSpreader.Repository;
using CourseProvider.Interface;
using Microsoft.AspNetCore.Mvc;
using CourseProvider.Data;
using CourseProvider.Models;
using System.Net;
using Microsoft.Net.Http.Headers;
using CourseProvider.ViewModel;

namespace CourseProvider.Controllers 
{
    public class MaterialController : Controller 
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialController(CourseProviderContext context, IMaterialRepository material)
        {
            _materialRepository = material;
        }

        [HttpGet]
        public async Task<IActionResult> Index(IEnumerable<Material> materials)
        {
            return View("Index", materials);
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Material> materials;

            if (!ModelState.IsValid)
            {
                return View("Error");
            }

            try
            {
                if (_materialRepository != null)
                {
                    materials = await _materialRepository.GetAll();
                }

                else
                {
                    throw new Exception("Repository is null.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Error");
            }

            return RedirectToAction("Index", materials);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(Material material)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (material != null)
                {
                    return View("Detail", material);
                }

                else
                {
                    throw (new Exception("Provided data was insufficient"));
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return View("Index");
        }

        public async Task<IActionResult> Detail(int id)
        {
            Material material;

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_materialRepository != null)
                {
                    material = await _materialRepository.GetByIdAsync(id);
                }

                else
                {
                    throw new Exception("Repositiry is null.");
                }

                if (material != null)
                {
                    return RedirectToAction("Detail", material);
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return View("Index");   
        }

        [HttpGet]
        public async Task<IActionResult> Add(int id)
        {
            Console.WriteLine("Number: " + id);

            if(!ModelState.IsValid)
            {
                Console.WriteLine("modelstate nem valid");
                return View("Index");
            }
            Material.Types[] types = (Material.Types[])Enum.GetValues(typeof(Material.Types));
            Material.Types type = types[id];
            Console.WriteLine("Type is : " + type);
            ViewData["type"] = type;
            return View("Add");
           
        }

        [HttpPost]
        public async Task<IActionResult> Add(Material material)
        {

            Console.WriteLine(material.Quality);

            if (!ModelState.IsValid) 
            {
                return View("Index");
            }

            try 
            {
                if (material != null)
                {
                    await _materialRepository.Add(material);
                }

                else 
                {
                    throw new Exception("provided data is null");
                }
            }

            catch(Exception exception)
            {
                Console.WriteLine(exception);
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {

            Material material;

            if(!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                if (_materialRepository != null && id > 0)
                {
                    material = await _materialRepository.GetByIdAsync(id);

                    if (material != null)
                    {
                        _materialRepository.Delete(material);
                        return RedirectToAction("Index");
                    }

                    else 
                    {
                        throw new Exception("Provided data is insufficient.");
                    }
                }

                else 
                {
                    throw new Exception("Id is not valid or Repository in null.");
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Material material)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest();
            }

            try
            {
                if (material != null)
                {
                    return View("Edit", material);
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

        [HttpPost]
        public async Task<IActionResult> Edit(int id)
        {
            Material material;

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (_materialRepository != null && id > 0)
                {
                    material = await _materialRepository.GetByIdAsync(id);

                    if (material != null)
                    {
                        return RedirectToAction("Edit", material);
                    }

                    else 
                    {
                        throw new Exception("Material i snull.");
                    }
                }

                else 
                {
                    throw new Exception("Respository is null or providedn id is null");  
                }
            }

            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return View("Index");
            }
        }

        public async Task<IActionResult> Update(int id, Material material)
        {
            Material newMaterial;
            
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                if (material != null && id > 0)
                {
                    newMaterial = await _materialRepository.GetByIdAsync(id);

                    if (newMaterial != null)
                    {
                       await _materialRepository.Update(newMaterial, material);
                        //return RedirectToAction("Edit", newMaterial);
                    }

                    else 
                    {
                        throw new Exception("Material is null.");
                    }
                }

                else 
                {
                    throw new Exception("Provided data is null.");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);  
                return RedirectToAction("Index");   
            }

            return RedirectToAction("Index");
        }
    }
}
