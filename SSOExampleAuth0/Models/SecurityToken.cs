﻿namespace SampleMvcApp.Models
{
    public class SecToken
    {

        public SecToken(string name, string token)
        {
            Name = name;
            Token = token;
        }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}
