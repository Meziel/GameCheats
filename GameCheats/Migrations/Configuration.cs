namespace GameCheats.Migrations
{
	using GameCheats.Models;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<GameCheats.GCDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "GameCheats.GCDbContext";
        }

        protected override void Seed(GameCheats.GCDbContext context)
        {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.

			Game game1 = new Game { Id = 0, Name = "Game 1"};
			Game game2 = new Game { Id = 1, Name = "Game 2"};

			context.Games.AddOrUpdate(game1);
			context.Games.AddOrUpdate(game2);

			context.Configurations.AddOrUpdate(new Models.Configuration { Id = 0, Name = "Config 1", GameId = 0 });
			context.Configurations.AddOrUpdate(new Models.Configuration { Id = 1, Name = "Config 2", GameId = 0 });
			context.Configurations.AddOrUpdate(new Models.Configuration { Id = 2, Name = "Config 3", GameId = 1 });
		}
    }
}
