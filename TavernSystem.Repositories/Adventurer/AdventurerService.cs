using Microsoft.Data.SqlClient;
using TavernSystem.Models.DTOs;
using TavernSystem.Models.Models;

namespace WebApplication1;

public class AdventurerService : IAdventurerService
{
    private readonly string _connectionString;
    public AdventurerService(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<List<GetAdventurerDTO>> GetAllAdventurersAsync(CancellationToken cancellationToken)
    {
        List<GetAdventurerDTO> adventurers = [];
        int id;
        string nickname;
        string sql = "select a.Id, a.Nickname from Adventurer a";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(sql, connection))   
        {
            await connection.OpenAsync(cancellationToken);
            using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    id = (int)reader["Id"];
                    nickname = (string)reader["Nickname"];
                    var dto = new GetAdventurerDTO()
                    {
                        Id = id,
                        nickName = nickname
                    };
                    adventurers.Add(dto);
                }
            }
        }

        return adventurers;
    }

    public async Task<AdventurerByIdDTO> GetAdventurerAsync(int id, CancellationToken cancellationToken)
    {
        AdventurerByIdDTO adventurer = null;
        string sql = "select a.Nickname, r.Name, e.Name , pd.Id, pd.firstName,pd.middleName ,pd.lastName, pd.hasBounty from PersonData pd join Adventurer a on pd.Id=a.PersonId join Race r on r.Id=a.RaceId join ExperienceLevel e on e.Id = a.ExperienceId where a.Id = @Id";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(sql, connection))   
        {
            command.Parameters.AddWithValue("@Id", id);
            await connection.OpenAsync(cancellationToken);
            using (SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    string nickname = (string)reader["Nickname"];
                    string race = reader.GetString(1);
                    string experience = reader.GetString(2);
                    string persondataId = (string)reader["Id"];
                    string firstname = (string)reader["firstName"];
                    string middleName = (string)reader["middleName"];
                    string lastName = (string)reader["lastName"];
                    bool hasBounty = (bool)reader["hasBounty"];
                    
                    var personData = new PersonData()
                    {
                        id = persondataId,
                        firstName = firstname,
                        middleName = middleName,
                        lastName = lastName,
                        hasBountry = hasBounty
                    };
                    var adv = new AdventurerByIdDTO()
                    {
                        id = id,
                        nickname = nickname,
                        race = race,
                        experienceLevel = experience,
                        PersonData = personData
                    };
                    adventurer = adv;
                }
            }
        }
        return adventurer;
    }

    public async void InsertAdventurer(InsertAdventurerDTO adventurer, CancellationToken cancellationToken)
    {
        string sql = "Insert into Adveturer (Nickname, RaceId, ExperienceId, PersonID) values (@nickname, @race, @experience, @personid)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        using (SqlCommand command = new SqlCommand(sql, connection))
        using (SqlTransaction transaction = connection.BeginTransaction())
        {
            command.Parameters.AddWithValue("nickname",adventurer.nickname);
            command.Parameters.AddWithValue("race",adventurer.raceId);
            command.Parameters.AddWithValue("experience",adventurer.experienceLevelId);
            command.Parameters.AddWithValue("personid",adventurer.personDataId);
            try
            {
                await connection.OpenAsync(cancellationToken);
                await connection.BeginTransactionAsync(cancellationToken);
                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (Exception e)
            {
               transaction.Rollback();
            }
        }
    }
}