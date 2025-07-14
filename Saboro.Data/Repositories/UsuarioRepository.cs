using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Data.Context;
using Saboro.Data.Repositories.Base;
using Saboro.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Saboro.Core.Enums;

namespace Saboro.Data.Repositories;

public class UsuarioRepository(ApplicationDbContext dbContext) : BaseRepository(dbContext), IUsuarioRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    public async Task<(IEnumerable<Usuario> Usuarios, int Total)> BuscarAsync(int? pagina = 1, int? tamanhoPagina = 7)
    {
        var query = _dbContext.Usuarios.AsSingleQuery();

        var total = await query.CountAsync();

        var usuarios = await query
            .Include(u => u.CategoriaFavorita)
            .Include(u => u.NivelCulinario)
            .OrderBy(u => u.NomeCompleto)
            .Skip((pagina.Value - 1) * tamanhoPagina.Value)
            .Take(tamanhoPagina.Value)
            .ToListAsync();

        return (usuarios, total);

    }

    public async Task<Usuario> BuscarPorEmailAsync(string email)
    {
        var query = _dbContext.Usuarios.AsQueryable();

        return await query
            .Where(u => u.Email.ToLower() == email.ToLower())
            .Include(u => u.CategoriaFavorita)
            .Include(u => u.NivelCulinario)
            .FirstOrDefaultAsync();
    }


    public async Task<Usuario> BuscarNomePeloIdAsync(int idUsuario)
    {
        if (idUsuario <= 0)
            return null;

        return await _dbContext.Usuarios
            .Where(u => u.Id == idUsuario)
            .Select(u => new Usuario
            {
                Id = u.Id,
                NomeCompleto = u.NomeCompleto
            }).FirstOrDefaultAsync();
    }

    public async Task AtualizarAsync(int id, object modifiedFields)
    {
        await _dbContext.UpdateEntryAsync<Usuario>(id, modifiedFields);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Usuario> BuscarAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _dbContext.Usuarios
            .Include(u => u.CategoriaFavorita)
            .Include(u => u.NivelCulinario)
            .Include(u => u.Receitas)
            .Select(u => new Usuario
            {
                Id = u.Id,
                IdCategoriaFavorita = u.IdCategoriaFavorita,
                IdNivelCulinario = u.IdNivelCulinario,
                NomeCompleto = u.NomeCompleto,
                Email = u.Email,
                Biografia = u.Biografia,
                UsuarioCadastro = u.UsuarioCadastro,
                UsuarioUltimaAlteracao = u.UsuarioUltimaAlteracao,
                DataCadastro = u.DataCadastro,
                DataUltimaAlteracao = u.DataUltimaAlteracao,

                Receitas = u.Receitas.ToList()
            }).FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AdicionarAsync(Usuario usuario)
    {
        await _dbContext.AddAsync(usuario);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoverAsync(Usuario usuario)
    {
        _dbContext.Remove(usuario);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> EmailJaExistenteAsync(string email, int? idUsuarioAtual = null)
    {
        var query = _dbContext.Usuarios.AsQueryable();

        if (idUsuarioAtual.HasValue)
            query = query.Where(u => u.Id != idUsuarioAtual.Value);

        return await query.AnyAsync(u => u.Email.ToLower().Equals(email.ToLower()));
    }

}
