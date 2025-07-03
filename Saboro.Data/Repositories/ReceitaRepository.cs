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
            await AtualizarIngredientesAsync(id, (Receita)receita);
            await AtualizarModoPreparoAsync(id, (Receita)receita);
            await _dbContext.UpdateEntryAsync<Receita>(id, receita);
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
            IdReceita = id,
            DescricaoIngrediente = i.DescricaoIngrediente,
        });

        await _dbContext.AddRangeAsync(novosIngredientes);
    }

    public async Task AtualizarModoPreparoAsync(int id, Receita receita)
    {
        var receitaExistente = await BuscarReceitaPorIdAsync(id);

        if (receitaExistente == null)
            throw new Exception("Receita nao encontrada.");

        _dbContext.ModosPreparoReceitas.RemoveRange(receitaExistente.ModoPreparoReceitas);
        await _dbContext.SaveChangesAsync();

        var novosModosPreparo = receita.ModoPreparoReceitas.Select(i => new ModoPreparoReceita
        {
            IdReceita = id,
            Ordem = i.Ordem,
            Descricao = i.Descricao,
        });

        await _dbContext.AddRangeAsync(novosModosPreparo);
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
