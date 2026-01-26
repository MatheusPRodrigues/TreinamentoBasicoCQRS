namespace CQRS.Infraestructure.Context
{
    public interface IAbstractFactory<TConnection>
    {
        public TConnection CreateConnection();
    }
}
