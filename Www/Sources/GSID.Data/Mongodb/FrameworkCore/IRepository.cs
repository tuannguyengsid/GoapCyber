using GSID.Data.Mongodb.MongoCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.FrameworkCore
{
    /// <summary>
    /// This is a generic contract that defines most generic behavior 
    /// Eddynguyen@g-sid.com
    /// </summary>
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Get one document by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns>T</returns>
        T GetOne<T>(string id) where T : IGSIDEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        T GetOne<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        List<T> All<T>() where T : IGSIDEntity;

        /// <summary>
        /// Get many documents
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="predicate">predicate</param>        
        /// <param name="take">"-1" here would actually default ot 256 records</param>
        /// <param name="skip">skip count</param>
        /// <returns>IList<T></returns>
        List<T> GetMany<T>(Expression<Func<T, bool>> predicate = null, int take = -1, int skip = 0) where T : IGSIDEntity;

        string Insert<T>(T entity) where T : IGSIDEntity;

        /// <summary>
        /// Batch Insert; suitable for a batch of 100 or less docs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns>List of IDs of the generated documents</returns>
        List<string> Insert<T>(List<T> entities) where T : IGSIDEntity;

        /// <summary>
        /// Bulk insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <returns>List of IDs of the generated documents</returns>
        List<string> BulkInsert<T>(List<T> entities) where T : IGSIDEntity;

        
        /// <summary>
        /// Updating a document. Please note that the version number is mandatory to be passed.
        /// Check the implementation in MXMongoRepository.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        UpdateResult Update<T>(T entity) where T : IGSIDEntity;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<UpdateResult> UpdateAsync<T>(T entity, CancellationToken cancellationToken = new CancellationToken()) where T : IGSIDEntity;

        /// <summary>
        /// Delete by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Document Id</param>
        /// <returns></returns>
        DeleteResult Delete<T>(string id) where T : IGSIDEntity;
        
        /// <summary>
        /// Delete by predicate for a smaller batch size; 100 or so.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        bool Delete<T>(Expression<Func<T, bool>> predicate = null) where T : IGSIDEntity;
  

        /// <summary>
        /// Returns the count of records in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">Optional value is null.</param>
        /// <returns></returns>
        long GetCount<T>(Expression<Func<T, bool>> predicate = null) where T : IGSIDEntity;

        /// <summary>
        /// Returns a single pair of DenormalizedReference type.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDenormalizedReference"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        TDenormalizedReference GetOptionById<TEntity, TDenormalizedReference>(string Id) 
            where TEntity : IGSIDEntity
            where TDenormalizedReference : IDenormalizedReference, new();

        /// <summary>
        /// Returns a list of DenormalizedReference types
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDenormalizedReference"></typeparam>
        /// <param name="predicate"></param>
        /// <param name="take">"-1" here would actually default ot 256 records</param>
        /// <returns></returns>
        List<TDenormalizedReference> GetOptionSet<TEntity, TDenormalizedReference>(Expression<Func<TEntity, bool>> predicate = null, int take = -1, int skip = 0)
            where TEntity : IGSIDEntity
            where TDenormalizedReference : IDenormalizedReference, new();

        /// <summary>
        /// drops the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        bool DropDatabase(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///  drops asyns the database
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        bool DropDatabaseAsync(CancellationToken cancellationToken = new CancellationToken());
        
        /// <summary> Drops the collection. This same concept is there in other databases such as arango, raven etc. 
        /// Whereas couchbase and couchdb doesn't have explicit collections, but the same concept of clearing all documents of a particular types applies good.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool DropCollection<T>() where T : IGSIDEntity;
    }
}
