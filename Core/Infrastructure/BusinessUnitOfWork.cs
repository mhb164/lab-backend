namespace Laboratory.Backend;

public sealed class BusinessUnitOfWork : UnitOfWork<BusinessDbContext>, IBusinessUnitOfWork
{
    public BusinessUnitOfWork(BusinessDbContext context) : base(context)
    {
    }
}