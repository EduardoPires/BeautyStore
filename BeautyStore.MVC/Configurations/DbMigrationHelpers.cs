using BeautyStore.Domain.Entities;
using BeautyStore.Infra.Data.Contexts;
using BeautyStore.MVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeautyStore.MVC.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app) 
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData (WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope =serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<BeautyStoreDbContext>();
            var contextId = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            if (env.IsDevelopment())
            {
                await context.Database.MigrateAsync();
                await contextId.Database.MigrateAsync();

                await EnsureSeedProdutcts(context, contextId);
            }
        }

        public static async Task EnsureSeedProdutcts(BeautyStoreDbContext context, ApplicationDbContext contextId )
        {
            //CATEGORIAS
            if (context.Categorias.Any())
                return;

            var primeiraCategoriaId = Guid.NewGuid();

            await context.Categorias.AddAsync( new Categoria() 
            {
                Id = primeiraCategoriaId,
                Nome = "Cabelos",
                Descricao = "Produtos para os cabelos"
            });

            var segundaCategoriaId = Guid.NewGuid();

            await context.Categorias.AddAsync(new Categoria()
            {
                Id = segundaCategoriaId,
                Nome = "Mãos",
                Descricao = "Produtos para as mãos"
            });

            await context.SaveChangesAsync();


            //USUÁRIOS
            if (contextId.Users.Any())
                return;

            var primeiroUserId = Guid.NewGuid().ToString();
            var primeiroUserName = "tatimachado27@hotmail.com";

            await contextId.Users.AddAsync(new IdentityUser()
            { 
                Id = primeiroUserId,
                UserName = primeiroUserName,
                NormalizedUserName= primeiroUserName.ToUpper(),
                Email = primeiroUserName,
                NormalizedEmail = primeiroUserName.ToUpper(),
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEEdWhqiCwW/jZz0hEM7aNjok7IxniahnxKxxO5zsx2TvWs4ht1FUDnYofR8JKsA5UA==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            });


            var segundoUserId = Guid.NewGuid().ToString();
            var segundoUserName = "ccr.jeh@gmail.com";

            await contextId.Users.AddAsync(new IdentityUser()
            {
                Id = segundoUserId,
                UserName = segundoUserName,
                NormalizedUserName = segundoUserName.ToUpper(),
                Email = segundoUserName,
                NormalizedEmail = segundoUserName.ToUpper(),
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEEdWhqiCwW/jZz0hEM7aNjok7IxniahnxKxxO5zsx2TvWs4ht1FUDnYofR8JKsA5UA==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            var terceiroUserId = Guid.NewGuid().ToString();
            var terceiroUserName = "inara0723@gmail.com";

            await contextId.Users.AddAsync(new IdentityUser()
            {
                Id = terceiroUserId,
                UserName = terceiroUserName,
                NormalizedUserName = terceiroUserName.ToUpper(),
                Email = terceiroUserName,
                NormalizedEmail = terceiroUserName.ToUpper(),
                AccessFailedCount = 0,
                LockoutEnabled = false,
                PasswordHash = "AQAAAAIAAYagAAAAEEdWhqiCwW/jZz0hEM7aNjok7IxniahnxKxxO5zsx2TvWs4ht1FUDnYofR8JKsA5UA==",
                TwoFactorEnabled = false,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            await contextId.SaveChangesAsync();


            //VENDEDORES
            if (context.Vendedores.Any())
                return;

            await context.Vendedores.AddAsync(new Vendedor()
            {
                Id= Guid.Parse(primeiroUserId),
                Nome = primeiroUserName,
                IsProprietario = true
            });

            await context.Vendedores.AddAsync(new Vendedor()
            {
                Id = Guid.Parse(segundoUserId),
                Nome = segundoUserName,
                IsProprietario = true
            });

            await context.Vendedores.AddAsync(new Vendedor()
            {
                Id = Guid.Parse(terceiroUserId),
                Nome = terceiroUserName,
                IsProprietario = true
            });

            await context.SaveChangesAsync();


            //PRODUTOS
            if (context.Produtos.Any())
                return;

            await context.Produtos.AddAsync(new Produto()
            {
                Id = Guid.NewGuid(),
                Nome = "Condicionador Dove",
                Descricao = "Condicionador Dove",
                Imagem = "/uploads/condicionadorDove.jpg",
                Preco = 10,
                Estoque = 5,
                VendedorId = Guid.Parse(segundoUserId),
                CategoriaId = primeiraCategoriaId
            });

            await context.Produtos.AddAsync(new Produto()
            {
                Id = Guid.NewGuid(),
                Nome = "Condicionador Elseve",
                Descricao = "Condicionador Elseve",
                Imagem = "/uploads/condicionadorElseve.jpg",
                Preco = 22,
                Estoque = 5,
                VendedorId = Guid.Parse(primeiroUserId),
                CategoriaId = primeiraCategoriaId
            });

            await context.Produtos.AddAsync(new Produto()
            {
                Id = Guid.NewGuid(),
                Nome = "Esmalte Impala Azul",
                Descricao = "Esmalte Impala Azul",
                Imagem = "/uploads/esmalteAzul.jpg",
                Preco = 7,
                Estoque = 5,
                VendedorId = Guid.Parse(segundoUserId),
                CategoriaId = segundaCategoriaId
            });

            await context.Produtos.AddAsync(new Produto()
            {
                Id = Guid.NewGuid(),
                Nome = "Esmalte Colorama Verde",
                Descricao = "Esmalte Colorama Verde",
                Imagem = "/uploads/esmalteVerde.jpg",
                Preco = 9,
                Estoque = 2,
                VendedorId = Guid.Parse(terceiroUserId),
                CategoriaId = segundaCategoriaId
            });

            await context.Produtos.AddAsync(new Produto()
            {
                Id = Guid.NewGuid(),
                Nome = "Kit Finalizador SalonLine",
                Descricao = "Kit Finalizador SalonLine",
                Imagem = "/uploads/KitFinalizadorSalonLine.jpg",
                Preco = 99,
                Estoque = 2,
                VendedorId = Guid.Parse(terceiroUserId),
                CategoriaId = primeiraCategoriaId
            });

            await context.SaveChangesAsync();
        }
    }
}
