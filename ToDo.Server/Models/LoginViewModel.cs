using System.ComponentModel.DataAnnotations;

namespace Todo.Server.Models;

public record LoginViewModel(
    [Required]
    [MinLength(1)]
    string UserName,
    
    [Required]
    [MinLength(6)]
    string Password);