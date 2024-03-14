using System.ComponentModel.DataAnnotations;

namespace Todo.Server.Models;

public readonly record struct LoginViewModel(
    [Required]
    [MinLength(1)]
    string UserName,
    
    [Required]
    [MinLength(6)]
    string Password);