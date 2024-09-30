using MvcExtensions;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace CourseProvider.Models {
    public class User : Entity {

        //public int Id { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "The username must be minimum 6 characters long.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "A username is required.")]
        public string? UserName { get; set; }

        [StringLength(20, MinimumLength = 6, ErrorMessage = "The password must be minimum 6 characters long.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "A pasword is required.")]
        public string? Password { get; set; }

        [StringLength(30, MinimumLength = 8)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        public bool? IsActive { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfJoining { get; set; }

        public List<int> Courses { get; set; } = new List<int>();

        public List<int> Skills { get; set; } = new List<int>();

        public List<int> Materials { get; set; } = new List<int>();
    }

    public class UserDTO 
    {

        [StringLength(20, MinimumLength = 6, ErrorMessage = "The username must be minimum 6 characters long.")]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "A username is required.")]
        public string UserName { get; set; } = string.Empty;


        [StringLength(20, MinimumLength = 6, ErrorMessage = "The password must be minimum 6 characters long.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "A pasword is required.")]
        public string Password { get; set; } = string.Empty;
    }

    public class CookieHandler<T> 
    {
        private string _key = string.Empty;

        private IRequestCookieCollection requestCookies { get; set; } = null!;
        private IResponseCookies responseCookies { get; set; } = null!;

        public CookieHandler(string key, IRequestCookieCollection cookies) 
        {
            this._key = key;
            this.requestCookies = cookies;
        }

        public CookieHandler(string key,IResponseCookies cookies) 
        {
            this._key = key;
            this.responseCookies = cookies;
        }

        public CookieHandler(IRequestCookieCollection cookies)
        {
            this.requestCookies = cookies;
        }

        public CookieHandler(IResponseCookies cookies)
        {
            this.responseCookies = cookies;
        }

        public void SetModelId<T>(T model) where T : Entity
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30)
            };

            var sanyika = JsonSerializer.Serialize<Entity>(model);
            //Create cookie
            responseCookies.Append(_key, sanyika, options);
        }

        public int GetModel(string key)
        {
            string cookie = requestCookies[key] ?? String.Empty;

            var sanyika = JsonSerializer.Deserialize<Entity>(cookie);

            Console.WriteLine(cookie);
            return sanyika.Id;
        }

        public void RemoveCookie(string key)
        {
            responseCookies.Delete(key);
        }
    }

    public class UserForCookie
    {
        private const string UserKey = "UserKey";
        private const string Delimiter = "-";
        private IRequestCookieCollection requestCookies { get; set; } = null!;
        private IResponseCookies responseCookies { get; set; } = null!;

        public UserForCookie(IRequestCookieCollection cookies)
        {
            this.requestCookies = cookies;
        }

        public UserForCookie(IResponseCookies cookies)
        {
            this.responseCookies = cookies;
        }

        public void SetUserId(User user)
        {
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(30)
            };

            string idString = String.Join(Delimiter, user.Id.ToString());

            //Create cookie
            responseCookies.Append(UserKey, idString, options);
        }

        public string[] Getuser()
        {
            string cookie = requestCookies[UserKey] ?? String.Empty;

            if (string.IsNullOrEmpty(cookie))
            {
                return Array.Empty<string>();
            }
            else 
            {
                return cookie.Split(Delimiter);
            }
        }

        public void RemoveCookie()
        {
            responseCookies.Delete(UserKey);
        }
    }
}
