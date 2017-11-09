using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TenLi.Api.DataAccess.Mongo
{
    public interface IMongoRepository<T>
    {
        T Insert(T instance);
        void Save(T instance);
        void Delete(ObjectId id);
        T GetById(ObjectId id);
        T Single(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes);
        List<T> GetList(Expression<Func<T, bool>> condition = null, Func<T, string> order = null);
        List<T> GetList(FilterDefinition<T> filterDefinition, SortDefinition<T> sortDefinition, int? limit, int? skip);
        List<T> GetList(FilterDefinition<T> filterDefinition);
        int Count(Expression<Func<T, bool>> predicate = null);
        bool Exists(Expression<Func<T, bool>> predicate);
        void Update(ObjectId id, UpdateDefinition<T> updateDefinition);
        void InsertBulk(List<T> list);
        void UpdateBulk(List<T> list, UpdateDefinition<T> updateDefinition);
        void UpdateBulk(Expression<Func<T, bool>> predicate, UpdateDefinition<T> updateDefinition);
        void DeleteBulk(List<T> list);
    }

    public class MongoRepository<T> : IMongoRepository<T> where T : Entity
    {
        private readonly IMongoCollection<T> _collection;
        private readonly IMongoDatabaseProvider _mongoDatabaseProvider;

        public MongoRepository(IMongoDatabaseProvider mongoDatabaseProvider)
        {
            _mongoDatabaseProvider = mongoDatabaseProvider;
            _collection = _mongoDatabaseProvider.Create().GetCollection<T>(TableName());
        }

        private IQueryable<T> CreateSet()
        {
            return _collection.AsQueryable();
        }

        private static string TableName()
        {
            return Inflector.Pluralize(typeof(T).Name);
        }

        public virtual T Insert(T instance)
        {
            instance._id = ObjectId.GenerateNewId();
            _collection.InsertOneAsync(instance).Wait();

            return instance;
        }

        public virtual void InsertBulk(List<T> list)
        {
            if (list.Count > 0)
            {
                _collection.InsertManyAsync(list).Wait();
            }
        }

        public virtual void Save(T instance)
        {
            _collection.ReplaceOneAsync(x => x._id.Equals(instance._id), instance, new UpdateOptions
            {
                IsUpsert = true
            });
        }

        public virtual void Delete(ObjectId id)
        {
            _collection.DeleteOneAsync(x => x._id == id).Wait();
        }

        public virtual void DeleteBulk(List<T> list)
        {
            if (list.Count > 0)
            {
                var filter = Builders<T>.Filter.In("_id", list.Select(x => x._id));
                _collection.DeleteManyAsync(filter).Wait();
            }
        }

        public virtual T GetById(ObjectId id)
        {
            return Single(o => o._id == id);
        }

        public virtual T Single(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includes)
        {
            var set = CreateSet();
            var query = (predicate == null ? set : set.Where(predicate));

            return query.SingleOrDefault();
        }

        public virtual List<T> GetList(Expression<Func<T, bool>> condition = null, Func<T, string> order = null)
        {
            var set = CreateSet();

            if (condition != null)
            {
                set = set.Where(condition);
            }

            if (order != null)
            {
                return set.OrderBy(order).ToList();
            }

            return set.ToList();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate = null)
        {
            var set = CreateSet();

            return (predicate == null ? set.Count() : set.Count(predicate));
        }

        public virtual long Count(FilterDefinition<T> filter)
        {
            return _collection.Count(filter);
        }

        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            var set = CreateSet();

            return set.Any(predicate);
        }

        public void Update(ObjectId id, UpdateDefinition<T> updateDefinition)
        {
            _collection.UpdateOneAsync(x => x._id == id, updateDefinition).Wait();
        }

        public void UpdateBulk(List<T> list, UpdateDefinition<T> updateDefinition)
        {
            if (list.Count > 0)
            {
                var filter = Builders<T>.Filter.In("_id", list.Select(x => x._id));
                _collection.UpdateManyAsync(filter, updateDefinition).Wait();
            }
        }

        public void UpdateBulk(Expression<Func<T, bool>> predicate, UpdateDefinition<T> updateDefinition)
        {
            _collection.UpdateManyAsync(predicate, updateDefinition).Wait();
        }

        public List<T> GetList(FilterDefinition<T> filterDefinition)
        {
            return _collection.Find(filterDefinition).ToList();
        }

        public List<T> GetList(FilterDefinition<T> filterDefinition, SortDefinition<T> sortDefinition, int? limit, int? skip)
        {
            return _collection.Find(filterDefinition).Sort(sortDefinition).Skip(skip).Limit(limit).ToList();
        }
    }
}
