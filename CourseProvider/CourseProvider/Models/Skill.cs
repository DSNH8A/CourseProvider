namespace CourseProvider.Models {
    public class Skill : Entity
    {
        //public int Id { get; set; }

        public string? Name { get; set; }

        public int SkillLevel { get; set; }

        public List<int> Courses { get; set; } = new List<int>();

        public User ?User { get; set; }
    }
}
