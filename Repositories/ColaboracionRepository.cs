using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class ColaboracionRepository : BaseRepository<Colaboracion>
{
    public ColaboracionRepository(AppDbContext context) : base(context) { }

}