﻿using System.ComponentModel.DataAnnotations;

namespace API.Entities;

public class AppUser
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] Password { get; set; }
    public byte[] PasswordHash { get; set; }
}
