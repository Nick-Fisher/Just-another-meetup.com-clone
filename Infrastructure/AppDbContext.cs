using System;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public required DbSet<Meeting> Meetings { get; set; }

}
