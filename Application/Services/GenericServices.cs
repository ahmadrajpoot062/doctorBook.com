using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class GenericServices<TEntity>
	{
		private readonly IRepository<TEntity> _irepository;
		public GenericServices(IRepository<TEntity> repository)
		{
			_irepository = repository;
		}
		public async Task Add(TEntity entity)
		{
			await _irepository.Add(entity);
		}
		public async Task<TEntity> GetById(int id)
		{
			return await _irepository.GetById(id);
		}
		public async Task<List<TEntity>> GetAll()
		{
			return await _irepository.GetAll();
		}
		public async Task Update(TEntity entity)
		{
			await _irepository.Update(entity);
		}
		public async Task Delete(int id)
		{
			await _irepository.Delete(id);
		}
	}
}
