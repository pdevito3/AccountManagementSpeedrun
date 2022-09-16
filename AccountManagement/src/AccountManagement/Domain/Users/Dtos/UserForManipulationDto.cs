namespace AccountManagement.Domain.Users.Dtos;

public abstract class UserForManipulationDto 
{
        public string Identifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

}
