using DSW_3P_PR03.Models;

namespace DSW_3P_PR03.Services;

public interface IPersonajeService
{
    IReadOnlyList<Personaje> ObtenerTodos();
    Personaje? ObtenerPorId(int id);
    Personaje Agregar(Personaje personaje);
    Personaje? Actualizar(int id, Personaje personaje);
    bool Eliminar(int id);
}
