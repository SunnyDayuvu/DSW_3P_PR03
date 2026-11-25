using DSW_3P_PR03.Data;
using DSW_3P_PR03.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR03.Repositories;

public class PersonajeSqlRepository : IPersonajeRepository
{
    private readonly AppDbContext _context;

    public PersonajeSqlRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Personaje> ObtenerTodos()
    {
        return _context.Personajes.AsNoTracking().ToList();
    }

    public Personaje? ObtenerPorId(int id)
    {
        return _context.Personajes.AsNoTracking().FirstOrDefault(p => p.Id == id);
    }

    public Personaje Agregar(Personaje personaje)
    {
        _context.Personajes.Add(personaje);
        _context.SaveChanges();
        return personaje;
    }

    public bool Actualizar(int id, Personaje personaje)
    {
        var existente = _context.Personajes.FirstOrDefault(p => p.Id == id);
        if (existente is null)
        {
            return false;
        }

        existente.Nombre = personaje.Nombre;
        existente.Videojuego = personaje.Videojuego;
        existente.Clase = personaje.Clase;
        existente.Nivel = personaje.Nivel;
        existente.Poder = personaje.Poder;
        existente.Disponible = personaje.Disponible;
        existente.ImagenUrl = personaje.ImagenUrl;

        _context.SaveChanges();
        return true;
    }

    public bool Eliminar(int id)
    {
        var personaje = _context.Personajes.FirstOrDefault(p => p.Id == id);
        if (personaje is null)
        {
            return false;
        }

        _context.Personajes.Remove(personaje);
        _context.SaveChanges();
        return true;
    }
}