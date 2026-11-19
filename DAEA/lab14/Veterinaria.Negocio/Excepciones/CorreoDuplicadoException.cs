namespace Veterinaria.Negocio.Excepciones;

public class CorreoDuplicadoException : ReglaNegocioException
{
    public CorreoDuplicadoException(string correo)
        : base($"El correo {correo} ya esta registrado")
    {
    }
}
