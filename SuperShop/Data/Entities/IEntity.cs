namespace SuperShop.Data.Entities
{
    /// <summary>
    /// Interface genérica que define a estrutura base comum a todas as entidades do sistema.
    /// </summary>
    public interface IEntity    //Interface genérico para todas as entidades
    {
        int Id { get; set; }    //A chave primária

        // Pode ser adicionada futuramente para implementar "soft delete":
        // /// <summary>
        // /// Indica se a entidade foi logicamente eliminada (sem remoção física da base de dados).
        // /// </summary>
        // bool WasDeleted { get; set; }
    }
}
