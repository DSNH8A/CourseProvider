using System.ComponentModel.DataAnnotations;

namespace CourseProvider.Models {
    public class Material : Entity
    {
        //----Online Article----

        //public int Id { get; set; }

        [StringLength(50, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string? Name { get; set; }

        public DateOnly? DateOfPublication { get; set; }

        public string? DataCarrier { get; set; }

        //----Video Material----

        public int? Duration { get; set; }

        public string? Quality { get; set; }

        //----Electronic Copies----

        public string? Author { get; set; }

        public int? NumberOfPages { get; set; }

        public DateOnly? YearOfPublication { get; set; }

        public string? Format { get; set; }

        public List<int> Courses { get; set; } = new List<int>();

        public enum Types
        {
            ONLINE_ARTICLE = 1,

            VIDEO_MATERIAL = 2,

            ELECTRONI_CPOPIES = 3,
        }
    }
}
