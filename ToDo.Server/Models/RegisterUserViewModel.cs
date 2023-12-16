namespace Todo.Server.Models;

public readonly record struct RegisterUserViewModel(string UserName, string Password, string ConfirmPassword);