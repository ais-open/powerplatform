namespace PluginOperations.Mapper
{
    interface IMapper<TModel,TEntity>
    {
        TModel MapFromEntity(TEntity entity);

        TEntity MapToEntity(TModel model);
    }
}
