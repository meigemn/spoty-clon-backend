namespace spoty_clon_backend.Utils
{
    public enum ResponseCodes
    {
        /// <summary>
        ///		Valor de Ok
        /// </summary>
        Ok = 0,
        /// <summary>
        ///	Valor de ErrorToken 
        /// </summary>
        ErrorToken = 1,
        /// <summary>
        ///		Valor de OriginAccess 
        /// </summary>
		OriginAccess = 2,
        /// <summary>
        ///		Valor de InvalidUserName
        /// </summary>
		InvalidUserName = 3,
        /// <summary>
        ///		Valor de InvalidPassword
        /// </summary>
        InvalidPassword = 4,
        /// <summary>
        ///		Valor de BdConectionFailed
        /// </summary>
        BdConectionFailed = 5,
        /// <summary>
        ///		Valor de NoDataFound
        /// </summary>
        NoDataFound = 6,
        /// <summary>
        ///		Valor de SaveDataFailed
        /// </summary>
        SaveDataFailed = 7,
        /// <summary>
        ///		Valor de SinEmpresa
        /// </summary>
        SinEmpresa = 8,
        /// <summary>
        ///		Valor de AccesoNoAutorizado
        /// </summary>
        AccesoNoAutorizado = 9,
        /// <summary>
        ///		Valor de ErrorDatos
        /// </summary>
        DataError = 10,
        /// <summary>
        ///		Valor de OtherError
        /// </summary>
        OtherError = 11,
        /// <summary>
        ///		Valor de InvalidModel
        /// </summary>
        InvalidModel = 12,
        /// <summary>
        ///		Valor de InvalidAccessType
        /// </summary>
        InvalidAccessType = 13,
        /// <summary>
        ///		Valor de InvalidToken
        /// </summary>
        InvalidToken = 14
    }
}
