using Entities.Models;

namespace Contracts
{
    public interface ICompanyRepository
    {
        //The Create and Delete method signatures are left synchronous.
        //Because, in these methods, we are not making any changes in the database.
        Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);
        Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges);
        void CreateCompany(Company company);
        Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        void DeleteCompany(Company company);
    }
}
