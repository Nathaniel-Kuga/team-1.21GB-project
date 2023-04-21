﻿using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Team121GBCapstoneProject.Data;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using SendGrid.Helpers.Mail;

namespace Team121GBNUnitTest;

public enum DbPersistence { OneDbPerTest, ReuseDb };
public class InMemoryDbHelper<TContext> : IDisposable where TContext : DbContext
{
    public DbPersistence PersistOption;
    private string _seedFilePath;
    private DbConnection _dbConnection;
    private DbContextOptions<TContext> _dbContextOptions;

    /// <summary>
    /// Create the helper that you can get a context from
    /// </summary>
    /// <param name="seedFilePath">Path to a seed .sql file, or null if you don't want to seed the db</param>
    /// <param name="persistOption">Choose your model: a new db per test, or a shared instance</param>
    public InMemoryDbHelper(string seedFilePath, DbPersistence persistOption)
    {
        PersistOption = persistOption;
        _seedFilePath = seedFilePath;
        Initialize();
    }

    private void Initialize()
    {
        // Establish connection with SQLite
        _dbConnection = new SqliteConnection("Filename=:memory:");
        _dbConnection.Open();
        // Create and save the options needed to create our DbContext
        _dbContextOptions = new DbContextOptionsBuilder<TContext>()
                                .UseSqlite(_dbConnection)
                                .UseLazyLoadingProxies()
                                .Options;

        // Create a context to use in populating the DB with tables defined in the context
        using TContext context = (TContext)Activator.CreateInstance(typeof(TContext), _dbContextOptions); // new TContext(_dbContextOptions);
        if (!context.Database.EnsureCreated())
        {
            Debug.WriteLine("Entity Framework could not ensure the db was created using Code First");
        }
        // Seed data
        if (_seedFilePath != null)
        {
            // No checking on this so will throw FileNotFoundException if it can't find it
            string seedText = System.IO.File.ReadAllText(_seedFilePath);
            using SqliteCommand cmd = new SqliteCommand(seedText, (SqliteConnection)_dbConnection);
            cmd.ExecuteNonQuery();
        }

        // The DB is now created and populated and will stick around until we dispose of the connection
    }

    public TContext GetContext()
    {
        if (PersistOption == DbPersistence.OneDbPerTest)
        {
            Dispose();
            Initialize();
        }
        return (TContext)Activator.CreateInstance(typeof(TContext), _dbContextOptions);
    }

    public void Dispose()
    {
        _dbConnection.Close();
        _dbConnection.Dispose();
    }
}