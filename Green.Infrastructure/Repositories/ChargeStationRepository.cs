using Green.Domain.Abstractions.IRepositories;
using Green.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Green.Infrastructure.Repositories;

public class ChargeStationRepository : IChargeStationRepository
{
    private readonly GreenDbContext _dbContext;

    public ChargeStationRepository(GreenDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public void Add(ChargeStation chargeStation)
        => _dbContext.ChargeStations.Add(chargeStation);

    public async Task<List<ChargeStation>> GetAll()
        => await _dbContext.ChargeStations.ToListAsync();

    public async Task<List<ChargeStation>> GetByGroupId(Guid groupId)
        => await _dbContext.ChargeStations.Where(c => c.GroupId == groupId).ToListAsync();

    public async Task<ChargeStation> GetById(Guid stationId)
        => await _dbContext.ChargeStations.FirstOrDefaultAsync(c => c.Id == stationId);

    public async Task<bool> HasChargeStationInAnyGroupId(Guid groupId)
       => await _dbContext.ChargeStations.AnyAsync(c => c.GroupId == groupId);

    public void Remove(ChargeStation station)
        => _dbContext.ChargeStations.Remove(station);

    public void Update(ChargeStation chargeStation)
     => _dbContext.ChargeStations.Update(chargeStation);
}