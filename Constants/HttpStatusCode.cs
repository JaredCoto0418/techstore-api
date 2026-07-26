namespace ApiTienda.Constants
{
    /// <summary>
    /// Define constantes para códigos de estado HTTP comunes.
    /// </summary>
    public static class CodigosDeEstadoHttp
    {
        public const int OK = 200;
        public const int CREADO = 201;
        public const int SIN_CONTENIDO = 204;
        public const int SOLICITUD_INCORRECTA = 400;
        public const int NO_AUTORIZADO = 401;
        public const int PROHIBIDO = 403;
        public const int NO_ENCONTRADO = 404;
        public const int ERROR_INTERNO_DEL_SERVIDOR = 500;
    }
} 