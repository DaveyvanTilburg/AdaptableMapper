namespace AdaptableMapper.Traversals
{
    public interface GetTemplateTraversal
    {
        Template GetTemplate(object target, MappingCaches mappingCache);
    }
}