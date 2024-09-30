using CourseProvider.Models;

namespace CourseProvider.Interface {
    public interface IMaterialRepository 
    {

        //faster but readonly 
        public Task<IEnumerable<Material>> GetAll();

        public Task Add(Material material);

        public Material GetById(int id);

        public Task<Material> GetByIdAsync(int id);

        public Task<List<Material>> GetOnlineArticles();

        public Task<List<Material>> GetVideoMaterials();

        public Task<List<Material>> GetElectronicCopies();

        public Task Update(Material MaterialToCahnge, Material MaterialWithChanges);
        public Task Delete(Material material);

    }
}
