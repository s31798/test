using TavernSystem.Models.Models;

namespace TavernSystem.Models.DTOs;

public class AdventurerByIdDTO
{
    public int id { get; set; }
    public string nickname { get; set; }
    public string race { get; set; }
    public string experienceLevel { get; set; }
    public PersonData PersonData { get; set; }
}