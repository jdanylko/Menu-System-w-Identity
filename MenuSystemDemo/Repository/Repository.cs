﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace MenuDemo.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext DbContext;
        public Repository(DbContext context)
        {
            DbContext = context;
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }
        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().AsNoTracking().Where(predicate);
        }
        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().Count(predicate);
        }
        public int Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            DbContext.Set<TEntity>().Add(entity);
            return DbContext.SaveChanges();
        }
        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }
        public int Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity Issue. It''s null.");
            DbContext.Entry(entity).State = EntityState.Deleted;
            return DbContext.SaveChanges();
        }
        public TEntity First(Expression<Func<TEntity, bool>> predicate)
        {
            return DbContext.Set<TEntity>().FirstOrDefault(predicate);
        }
        protected virtual T ExecuteReader<T>(Func<DbDataReader, T> mapEntities,
            string exec, params object[] parameters)
        {
            using (var conn = new SqlConnection(DbContext.Database.Connection.ConnectionString))
            {
                using (var command = new SqlCommand(exec, conn))
                {
                    conn.Open();
                    command.Parameters.AddRange(parameters);
                    command.CommandType = CommandType.StoredProcedure;
                    try
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            T data = mapEntities(reader);
                            return data;
                        }
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
    }
}