
using CourseProvider.Interface;
using CourseProvider.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CourseProvider.Data;

namespace CourseSpreader.Repository 
{
    public class MaterialRepository : IMaterialRepository 
    {
        private readonly CourseProviderContext _context;

        public MaterialRepository(CourseProviderContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Material>> GetAll()
        {
            return await _context.Materials.ToListAsync();
        }

        public Material GetById(int id)
        {
            return _context.Materials.Where(m => m.Id == id).FirstOrDefault();
        }

        public async Task<Material> GetByIdAsync(int id)
        {
            return await _context.Materials.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task Add(Material material)
        {
            try
            {
                if (material != null)
                {
                    Material mat = new Material
                    {
                        Name = material.Name,
                        DateOfPublication = material.DateOfPublication,
                        DataCarrier = material.DataCarrier,
                        Duration = material.Duration,
                        Quality = material.Quality,
                        Author = material.Author,
                        NumberOfPages = material.NumberOfPages,
                        YearOfPublication = material.YearOfPublication,
                        Format = material.Format,
                    };

                    //await IdentityHelpers.SetIdentityInsert<Material>(_context, false);
                    await _context.Materials.AddAsync(mat);
                    //await IdentityHelpers.SaveChangesWithIdentityInsert<Material>(_context);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Added to database");
                }

                else
                {
                    throw new Exception("The provided item in null");
                }
            }

            catch (Exception exception) 
            {
                Console.WriteLine(exception);
            }
        }

        public async Task Delete(Material material)
        {
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync(); 
        }

        public async Task<List<Material>> GetOnlineArticles()
        {
            List<Material> onlineArticles = new List<Material>();

            foreach (var item in _context.Materials)
            {
                if (item.Name != null && item.DateOfPublication != null && item.DataCarrier != null)
                {
                    onlineArticles.Add(item);
                }
            }

            return onlineArticles;
        }

        public async Task<List<Material>> GetVideoMaterials()
        {
            List<Material> videoMaterials = new List<Material>();

            foreach (var item in _context.Materials)
            {
                if (item.Name != null && item.Duration != null && item.Quality != null)
                {
                    videoMaterials.Add(item);
                }
            }

            return videoMaterials;
        }

        public async Task<List<Material>> GetElectronicCopies()
        {
           List<Material> electronicCopies = new List<Material>();

            foreach (var item in _context.Materials)
            {
                if (item.Name != null && item.Author != null && item.NumberOfPages > 0
                    && item.YearOfPublication != null && item.Format != null)
                {
                    electronicCopies.Add(item);
                }
            }

            return electronicCopies;
        }

        public async Task Update(Material MaterialToChange, Material MaterialWithChanges)
        {
            MaterialToChange.Duration = MaterialWithChanges.Duration;
            MaterialToChange.DataCarrier = MaterialWithChanges.DataCarrier;
            MaterialToChange.Format = MaterialWithChanges.Format;
            MaterialToChange.DateOfPublication = MaterialWithChanges.DateOfPublication;
            MaterialToChange.Author = MaterialWithChanges.Author;
            MaterialToChange.Name = MaterialWithChanges.Name;
            MaterialToChange.NumberOfPages = MaterialWithChanges.NumberOfPages;
            MaterialToChange.Quality = MaterialWithChanges.Quality;

            //await _context.Materials.AddAsync(MaterialToChange);
            await _context.SaveChangesAsync();
        }
    }


    public static class IdentityHelpers {
        public static Task EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: true);
        public static Task DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: false);

        public static Task SetIdentityInsert<T>(DbContext context, bool enable)
        {
            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            return context.Database.ExecuteSqlRawAsync(
                $"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}");
        }

        public static async Task SaveChangesWithIdentityInsert<T>(this DbContext context)
        {
            using var transaction = context.Database.BeginTransaction();
            await context.EnableIdentityInsert<T>();
            await context.SaveChangesAsync();
            await context.DisableIdentityInsert<T>();
            await transaction.CommitAsync();
        }

    }
}
