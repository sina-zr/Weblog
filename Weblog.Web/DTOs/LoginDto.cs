﻿namespace Weblog.Web.DTOs
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? RememberMe { get; set; } = true;
    }
}
