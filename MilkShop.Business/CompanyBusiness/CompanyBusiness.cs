using MilkShop.Data.Models;
using MilkShop.Data;
using MilkShopBusiness.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.OrderDetailBusinesses;

namespace MilkShop.Business.CompanyBusiness
{
    public interface ICompanyBusiness
    {
        Task<IBusinessResult> GetAll(int page, int size);
        Task<IBusinessResult> GetAllCompany();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> FindId(int id);
        Task<IBusinessResult> UpdateAsync(Company company);
        Task<IBusinessResult> Save(Company company);
        Task<IBusinessResult> DeleteAsync(int id);
        Task<IBusinessResult> Search(string searchTerm, int page, int size);
    }

    public class CompanyBusiness : ICompanyBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        public CompanyBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> DeleteAsync(int id)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
                if (company != null)
                {
                    var result = await _unitOfWork.CompanyRepository.RemoveAsync(company);
                    if (result)
                        return new BusinessResult(1, "success");
                    else
                        return new BusinessResult(0, "error");
                }
                return new BusinessResult(0, "no content");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IBusinessResult> FindId(int id)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
                if (company != null)
                {
                    return new BusinessResult(1, "Get company successfully", company);
                }
                else
                {
                    return new BusinessResult(-1, "Get company fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll(int page, int size)
        {
            try
            {
                #region Business rule
                #endregion

                //var currencies = _DAO.GetAll();
                var products = await _unitOfWork.CompanyRepository.GetPagingListAsync(
                    selector: x => x,
                    page: page,
                    size: size
                    );
                if (products == null)
                {
                    return new BusinessResult(4, "No currency data");
                }
                else
                {
                    return new BusinessResult(1, "Get currency list success", products);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAllCompany()
        {
            try
            {
                #region Business rule
                #endregion

                //var currencies = _DAO.GetAll();
                var companies = await _unitOfWork.CompanyRepository.GetAllAsync();
                if (companies == null)
                {
                    return new BusinessResult(4, "No currency data");
                }
                else
                {
                    return new BusinessResult(1, "Get currency list success", companies);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                var company = await _unitOfWork.CompanyRepository.SingleOrDefaultAsync(
                    selector: x => x,
                    predicate: x => x.CompanyId == id
                    ); ;
                if (company != null)
                {
                    return new BusinessResult(1, "Get company successfully", company);
                }
                else
                {
                    return new BusinessResult(-1, "Get company fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(Company company)
        {
            try
            {
                int result = await _unitOfWork.CompanyRepository.CreateAsync(company);
                if (result > 0)
                {
                    return new BusinessResult(1, "success");
                }
                else
                {
                    return new BusinessResult(2, "fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Search(string searchTerm, int page, int size)
        {
            try
            {
                var companySearch = await _unitOfWork.CompanyRepository.GetPagingListAsync(
                    selector: x => x,
                    predicate: x => x.CompanyName.Contains(searchTerm),
                    page: page,
                    size: size
                    );

                if (companySearch != null)
                {
                    return new BusinessResult(1, "Create successfully", companySearch);
                }
                else
                {
                    return new BusinessResult(1, "Create fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateAsync(Company company)
        {
            try
            {
                int result = await _unitOfWork.CompanyRepository.UpdateAsync(company);
                if (result > 0)
                {
                    return new BusinessResult(1, "success");
                }
                else
                {
                    return new BusinessResult(2, "fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }
    }
}
