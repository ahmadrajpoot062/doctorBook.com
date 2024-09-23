﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IRepository<TEntity>
	{
		Task Add(TEntity entity);
		Task<TEntity> GetById(int id);
		Task<List<TEntity>> GetAll();
		Task Update(TEntity entity);
		Task Delete(int id);
	}
}
