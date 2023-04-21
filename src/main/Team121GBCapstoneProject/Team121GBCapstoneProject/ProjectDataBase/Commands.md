### Create Model classes and the DBContext subclass
```
dotnet ef dbcontext scaffold Name=GPConnection Microsoft.EntityFrameworkCore.SqlServer --context GPDbContext --context-dir Models --output-dir Models --verbose --force  --data-annotations
```

### To Enable Lazy Loading, add this to the OnConfiguring Method. 
- Every time you create models from the database you have to re-add this to the context for lazy loading and in memory testing to work.
```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=GPConnection");
        }
        optionsBuilder.UseLazyLoadingProxies();
    }
```