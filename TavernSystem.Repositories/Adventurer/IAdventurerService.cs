using TavernSystem.Models.DTOs;

namespace WebApplication1;

public interface IAdventurerService
{
    public Task<List<GetAdventurerDTO>> GetAllAdventurersAsync(CancellationToken cancellationToken);
    public Task<AdventurerByIdDTO> GetAdventurerAsync(int id, CancellationToken cancellationToken);
    public void InsertAdventurer(InsertAdventurerDTO adventurer ,CancellationToken cancellationToken);
}