namespace GameCheats
{
	using GameCheats.Models;
	using System;
	using System.Collections.Generic;
	using System.Data.Entity;
	using System.Linq;

	public class GCDbContext : DbContext
	{
		// Your context has been configured to use a 'Model' connection string from your application's 
		// configuration file (App.config or Web.config). By default, this connection string targets the 
		// 'GameCheats.Model' database on your LocalDb instance. 
		// 
		// If you wish to target a different database and/or database provider, modify the 'Model' 
		// connection string in the application configuration file.
		public GCDbContext() : base("name=GCDbContext")
		{}

		// Add a DbSet for each entity type that you want to include in your model. For more information 
		// on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

		public virtual DbSet<Game> Games { get; set; }
		public virtual DbSet<Configuration> Configurations { get; set; }
		public virtual DbSet<Plugin> Plugins { get; set; }
		public virtual DbSet<ConfigurationPlugin> ConfigurationsPlugins { get; set; }
	}
}