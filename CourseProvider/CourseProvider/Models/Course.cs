namespace CourseProvider.Models {
    public class Course : Entity 
    {

        //public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public List<int> Skills { get; set; } = new List<int>();

        public List<int> Materails { get; set; } = new List<int>();
        
        public List<int> Users { get; set; } = new List<int>(); 
    }
}
