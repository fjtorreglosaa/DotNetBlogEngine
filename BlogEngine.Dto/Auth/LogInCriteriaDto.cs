﻿namespace BlogEngine.App.Dto.Auth
{
    public class LogInCriteriaDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
