using backend.Data;
using backend.Models;

namespace backend.Repositories;

public class UsuarioRepository : BaseRepository<Usuario>
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

}