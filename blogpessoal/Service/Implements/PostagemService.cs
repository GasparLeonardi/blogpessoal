﻿using blogpessoal.Data;
using blogpessoal.Model;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Service.Implements
{
    public class PostagemService : IPostagemService
    {

        private readonly AppDbContext _context;

        public PostagemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Postagem>> GetAll()
        {
            return await _context.Postagens
                .Include(p => p.Tema)
                .Include(u => u.Usuario)
                .ToListAsync();
        }

        public async Task<Postagem?> GetById(long id)
        {
            try
            {
                var Postagem = await _context.Postagens
                    .Include(p => p.Tema)
                    .Include(u => u.Usuario)
                    .FirstAsync(i => i.Id == id);
                return Postagem;
            }
            catch
            {
                return null;
            }
        }
        public async Task<IEnumerable<Postagem>> GetByTitulo(string titulo)
        {
            var Postagem = await _context.Postagens
                                .Where(p => p.Titulo.Contains(titulo))
                                .Include(p => p.Tema)
                                .Include(p => p.Usuario)
                                .ToListAsync();
            return Postagem;
        }
        public async Task<Postagem?> Create(Postagem postagem)
        {

            if (postagem.Tema is not null)
            {
                var BuscaTema = await _context.Temas.FirstOrDefaultAsync(t => t.Id == postagem.Tema.Id);

                if (BuscaTema is null)
                    return null;

                postagem.Tema = BuscaTema;

            }

            postagem.Usuario = postagem.Usuario is not null ? await _context.Users.FirstOrDefaultAsync(u => u.Id == postagem.Usuario.Id) : null;

            await _context.Postagens.AddAsync(postagem);
            await _context.SaveChangesAsync();

            return postagem;
        }
        public async Task<Postagem?> Update(Postagem postagem)
        {
            var PostagemUpdate = await _context.Postagens.FindAsync(postagem.Id);

            if (PostagemUpdate is null)
                return null;

            if (postagem.Tema is not null)
            {
                var BuscaTema = await _context.Temas.FirstOrDefaultAsync(t => t.Id == postagem.Tema.Id);

                if (BuscaTema is null)
                    return null;

                postagem.Tema = BuscaTema;

            }

            postagem.Usuario = postagem.Usuario is not null ? await _context.Users.FirstOrDefaultAsync(u => u.Id == postagem.Usuario.Id) : null;

            _context.Entry(PostagemUpdate).State = EntityState.Detached;
            _context.Entry(postagem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return postagem;

        }

        public async Task Delete(Postagem postagem)
        {
            _context.Remove(postagem);
            await _context.SaveChangesAsync();

        }
    }
}
