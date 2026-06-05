using Microsoft.EntityFrameworkCore;
using Projekt_Rezervacija_terena.Models;
using System.Collections.Generic;

namespace Projekt_Rezervacija_terena.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Rezervacija> Rezervacije { get; set; }

        // DODAJ OVU LINIJU ISPOD:
        public DbSet<Korisnik> Korisnici { get; set; }
    }
}