using System.ComponentModel.DataAnnotations;

namespace Todo.Server.Models;

public readonly record struct RegisterUserViewModel(
    [Required]
    [MinLength(1)]
    string UserName,
    
    [Required]
    [MinLength(6)]
    string Password, 
    
    [Required]
    [MinLength(6)]
    string ConfirmPassword);