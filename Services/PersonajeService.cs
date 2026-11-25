using DSW_3P_PR03.Models;
using DSW_3P_PR03.Repositories;

namespace DSW_3P_PR03.Services;

public class PersonajeService : IPersonajeService
{
    private readonly IPersonajeRepository _repository;

    public PersonajeService(IPersonajeRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Personaje> ObtenerTodos()
    {
        return _repository.ObtenerTodos();
    }

    public Personaje? ObtenerPorId(int id)
    {
        return _repository.ObtenerPorId(id);
    }

    public Personaje Agregar(Personaje personaje)
    {
        ArgumentNullException.ThrowIfNull(personaje);
        Validar(personaje);

        personaje.Disponible = true;
        return _repository.Agregar(personaje);
    }

    public Personaje? Actualizar(int id, Personaje personaje)
    {
        ArgumentNullException.ThrowIfNull(personaje);
        Validar(personaje);

        personaje.Id = id;
        return _repository.Actualizar(id, personaje) ? personaje : null;
    }

    public bool Eliminar(int id)
    {
        return _repository.Eliminar(id);
    }

    private static void Validar(Personaje personaje)
    {
        if (string.IsNullOrWhiteSpace(personaje.Nombre))
        {
            throw new ArgumentException("El nombre es obligatorio.", nameof(personaje));
        }

        if (string.IsNullOrWhiteSpace(personaje.Videojuego))
        {
            throw new ArgumentException("El videojuego es obligatorio.", nameof(personaje));
        }

        if (string.IsNullOrWhiteSpace(personaje.Clase))
        {
            throw new ArgumentException("La clase es obligatoria.", nameof(personaje));
        }

        if (personaje.Nivel is < 1 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(personaje.Nivel), "El nivel debe estar entre 1 y 100.");
        }

        if (personaje.Poder <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(personaje.Poder), "El poder debe ser mayor que cero.");
        }

        if (!Uri.TryCreate(personaje.ImagenUrl, UriKind.Absolute, out var url)
            || (url.Scheme != Uri.UriSchemeHttp && url.Scheme != Uri.UriSchemeHttps)
            || string.IsNullOrWhiteSpace(url.Host))
        {
            throw new ArgumentException(
                "ImagenUrl debe ser una URL absoluta válida mediante HTTP o HTTPS.",
                nameof(personaje));
        }

    }

}
