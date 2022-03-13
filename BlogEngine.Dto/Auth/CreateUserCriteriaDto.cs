namespace BlogEngine.Dto.Auth
{
    public class CreateUserCriteriaDto
    {        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int AuthorId { get; set; }
    }
}
