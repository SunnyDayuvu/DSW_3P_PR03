using DSW_3P_PR03.Models;
using System.Text.Json;

namespace DSW_3P_PR03.Repositories;

public class PersonajeJsonRepository : IPersonajeRepository
{
    private readonly string _ruta;
    private readonly object _bloqueo = new();
    private readonly JsonSerializerOptions _opciones = new()
    {
        WriteIndented = true
    };

    public PersonajeJsonRepository(IWebHostEnvironment environment)
    {
        _ruta = Path.Combine(environment.ContentRootPath, "Data", "personajes.json");

        Directory.CreateDirectory(Path.GetDirectoryName(_ruta)!);

        if (!File.Exists(_ruta))
        {
            File.WriteAllText(_ruta, "[]");
        }
    }

    private List<Personaje> Leer()
    {
        var json = File.ReadAllText(_ruta);
        return JsonSerializer.Deserialize<List<Personaje>>(json, _opciones)
            ?? throw new InvalidDataException("El archivo de personajes contiene un valor JSON nulo.");
    }

    private void Guardar(List<Personaje> personajes)
    {
        var json = JsonSerializer.Serialize(personajes, _opciones);
        var archivoTemporal = _ruta + ".tmp";
        File.WriteAllText(archivoTemporal, json);
        File.Move(archivoTemporal, _ruta, overwrite: true);
    }

    public List<Personaje> ObtenerTodos()
    {
        lock (_bloqueo)
        {
            return Leer();
        }
    }

    public Personaje? ObtenerPorId(int id)
    {
        lock (_bloqueo)
        {
            return Leer().FirstOrDefault(personaje => personaje.Id == id);
        }
    }

    public Personaje Agregar(Personaje personaje)
    {
        ArgumentNullException.ThrowIfNull(personaje);

        lock (_bloqueo)
        {
            var personajes = Leer();
            personaje.Id = personajes.Count == 0 ? 1 : checked(personajes.Max(registrado => registrado.Id) + 1);
            personajes.Add(personaje);
            Guardar(personajes);
            return personaje;
        }
    }

    public bool Actualizar(int id, Personaje personaje)
    {
        ArgumentNullException.ThrowIfNull(personaje);

        lock (_bloqueo)
        {
            var personajes = Leer();
            var existente = personajes.FirstOrDefault(registrado => registrado.Id == id);

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

            Guardar(personajes);
            return true;
        }
    }

    public bool Eliminar(int id)
    {
        lock (_bloqueo)
        {
            var personajes = Leer();
            var personaje = personajes.FirstOrDefault(registrado => registrado.Id == id);

            if (personaje is null)
            {
                return false;
            }

            personajes.Remove(personaje);
            Guardar(personajes);
            return true;
        }
    }
}
