namespace Lab2.Models
{
    public enum ErrorCodes
    {
        /// <summary>
        /// Default value of the <see cref="ErrorCodes"/> enum.
        /// </summary>
        UnknownError,

        /// <summary>
        /// Empty result, no exact data was found.
        /// </summary>
        EmptyResult,

        /// <summary>
        /// Empty source data table.
        /// </summary>
        EmptyTable,

        /// <summary>
        /// Data already exists in the table.
        /// </summary>
        DataAlreadyExists,

        /// <summary>
        /// Not enought products in the storage.
        /// </summary>
        NotEnoughtCount,

        /// <summary>
        /// Wrong data format
        /// </summary>
        WrongDataFormat
    }
}
