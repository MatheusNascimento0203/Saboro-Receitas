using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Models;
using Saboro.Data.Context;
using Saboro.Data.Extensions;

namespace Saboro.Data.Repositories.Base;

public class ReceitaRepository(ApplicationDbContext dbContext) : BaseRepository(dbContext), IReceitaRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task AdicionarAsync(Receita receita)
    {
        await _dbContext.AddAsync(receita);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AtualizarAsync(int id, object receita)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var receitaModel = (Receita)receita;

            await AtualizarIngredientesAsync(id, receitaModel);
            await AtualizarModoPreparoAsync(id, receitaModel);

            await _dbContext.UpdateEntryAsync<Receita>(id, receita);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Erro ao atualizar receita.", ex);
        }
    }

    public async Task AtualizarIngredientesAsync(int id, Receita receita)
    {
        var receitaExistente = await BuscarReceitaPorIdAsync(id);

        if (receitaExistente == null)
            throw new Exception("Receita nao encontrada.");

        _dbContext.Ingredientes.RemoveRange(receitaExistente.Ingredientes);
        await _dbContext.SaveChangesAsync();

        var novosIngredientes = receita.Ingredientes.Select(i => new IngredienteReceita
        {
            Id = i.Id,
            IdReceita = id,
            DescricaoIngrediente = i.DescricaoIngrediente,
        });

        await _dbContext.AddRangeAsync(novosIngredientes);
    }

    public async Task AtualizarModoPreparoAsync(int id, Receita receita)
    {

        var entries = _dbContext.ChangeTracker.Entries<ModoPreparoReceita>();
        foreach (var entry in entries)
        {
            entry.State = EntityState.Detached;
        }

        var existentes = await _dbContext.ModosPreparoReceitas
            .Where(x => x.IdReceita == id)
            .ToListAsync();

        foreach (var md in receita.ModoPreparoReceitas)
        {
            var existente = existentes.FirstOrDefault(x => x.Id == md.Id);

            if (existente != null)
            {
                existente.Descricao = md.Descricao;
                existente.Ordem = md.Ordem;
                _dbContext.Update(existente);
            }
            else
            {
                _dbContext.Add(new ModoPreparoReceita
                {
                    IdReceita = id,
                    Ordem = md.Ordem,
                    Descricao = md.Descricao
                });
            }
        }

        var idsRecebidos = receita.ModoPreparoReceitas.Select(x => x.Id).ToList();
        var aRemover = existentes.Where(x => !idsRecebidos.Contains(x.Id));
        _dbContext.ModosPreparoReceitas.RemoveRange(aRemover);

        await _dbContext.SaveChangesAsync();
    }


    public async Task<IEnumerable<Receita>> BuscarReceitaAsync()
    {
        return await _dbContext.Receitas
            .Include(r => r.CategoriaFavorita)
            .Include(r => r.DificuldadeReceita)
            .Include(r => r.Ingredientes)
            .Include(r => r.ModoPreparoReceitas)
            .ToListAsync();
    }

    public async Task<Receita> BuscarReceitaPorIdAsync(int id)
    {
        return await _dbContext.Receitas
            .Include(r => r.CategoriaFavorita)
            .Include(r => r.DificuldadeReceita)
            .Include(r => r.Ingredientes)
            .Include(r => r.ModoPreparoReceitas)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Receita>> BuscarReceitaPorUsuarioAsync(int idUsuario)
    {
        return await _dbContext.Receitas
            .Include(r => r.CategoriaFavorita)
            .Include(r => r.DificuldadeReceita)
            .Include(r => r.Ingredientes)
            .Include(r => r.ModoPreparoReceitas)
            .Where(r => r.IdUsuario == idUsuario)
            .ToListAsync();
    }


    public async Task RemoverAsync(int id)
    {
        var receita = await BuscarReceitaPorIdAsync(id);

        if (receita == null)
            throw new Exception("Receita não encontrada");

        _dbContext.Remove(receita);
        await _dbContext.SaveChangesAsync();

    }

}
