using System.ComponentModel.DataAnnotations;

namespace Todo.Server.Models;

public record SearchViewModel(
    DateTime StartDate,
    DateTime EndDate);