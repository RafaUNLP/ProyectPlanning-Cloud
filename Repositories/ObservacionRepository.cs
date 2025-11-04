using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class ObservacionRepository : BaseRepository<Observacion>
{
    public ObservacionRepository(AppDbContext context) : base(context) { }

}