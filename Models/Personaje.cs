using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR03.Models;

public class Personaje
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(200)]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "El videojuego es obligatorio.")]
    [StringLength(200)]
    public string Videojuego { get; set; } = "";

    [Required(ErrorMessage = "La clase es obligatoria.")]
    [StringLength(200)]
    public string Clase { get; set; } = "";

    [Range(1, 100, ErrorMessage = "El nivel debe estar entre 1 y 100.")]
    public int Nivel { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El poder debe ser mayor que cero.")]
    public int Poder { get; set; }

    public bool Disponible { get; set; } = true;

    [Required(ErrorMessage = "La URL de la imagen es obligatoria.")]
    [StringLength(2048)]
    public string ImagenUrl { get; set; } = "";
}

public class Personajes : Personaje
{
}