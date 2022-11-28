using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using GSID.Data.Mongodb.MongoCore;
using System.Threading;
using System.Configuration;
using GSID.Data.Mongodb.Helpers;
using MongoDB.Bson.Serialization;

namespace GSID.Data.Mongodb.FrameworkCore
{
    /// <summary>
    /// Generic repository class for Mongo operations. This is intentionlly marked abstract so that it cannot be instantiated 
    /// without a concrete context(connectionUrl and databaseName).
    /// Please take a look at the "MXBusinessMongoRepository" class for having a separate context per database.
    /// </summary>
    public class GSIDMongoRepository :  IGSIDMongoRepository
    {
        #region "Initialization and attributes"

        protected readonly int takeCount = 999999999;
        protected System.Lazy<string> connectionUrl;
        protected System.Lazy<string> databaseName;
        MongoClientSettings _mongoClientSettings;

        public GSIDMongoRepository(){
            _dbContext = new Lazy<IMongoDatabase>(getSession);

            connectionUrl = new Lazy<string>(() => ConfigurationManager.AppSettings["mongoConnUrl"].ToString());
            databaseName = new Lazy<string>(() => ConfigurationManager.AppSettings["GSIDConfigurationDatabaseName"].ToString());
            MongoLazyLoad.Initialize();
        }


        /// <summary>
        /// Lazy instantiation of "MongoDB.Driver.MongoDatabase" object.
        /// </summary>
       System.Lazy<IMongoDatabase> _dbContext;

        /// <summary>
        /// Supporting the automatic failover.
        /// Setting up the server address only once during the lifetime of an application
        /// </summary>
        private MongoClientSettings MongoClientSettings
        {
            get
            {
                var nodes = new List<MongoServerAddress>();

                foreach (var address in connectionUrl.Value.Split(',')) //give replicaset as "machine1:27017,machine2:27018,machine3:27017"
                    nodes.Add(new MongoServerAddress(address.Split(':')[0], int.Parse(address.Split(':')[1])));

                _mongoClientSettings = new MongoClientSettings();
                _mongoClientSettings.Servers = nodes;

                return _mongoClientSettings;
            }
        }

        public IMongoDatabase DbContext
        {
            get { return _dbContext.Value; }
        }

        IMongoDatabase getSession()
        {
            return new MongoClient(MongoClientSettings).GetDatabase(databaseName.Value);
        }

        //public string CurrentUser
        //{
        //    get { return UserProfileHelper.CurrentUser; }
        //}

        public DateTime CurrentDate
        {
            get { return DateTime.Now; }
        }

        public bool IsProcessedByQueue { get; set; }

        #endregion

        #region "Interface implementaions; generic CRUD repository"

        #region "Get"

        /// <summary>
        /// Find one by Id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetOne<T>(string id) where T : IGSIDEntity
        {
            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(id));
            return collection.Find(filter).FirstOrDefault();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual T GetOne<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : class, new()
        {
            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            return collection.Find(expression).FirstOrDefault();
        }
     

        /// <summary>
        /// Load records based on predicates
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">Use the GSIDPredicate object to build predicates</param>
        /// <param name="bIsActive"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public virtual List<T> GetMany<T>(Expression<Func<T, bool>> predicate = null, int take = -1, int skip = 0) where T : IGSIDEntity
        {
            if (take == -1) take = takeCount;

            var collection = DbContext.GetCollection<T>(typeof(T).Name);

            if (predicate == null)
                return collection.AsQueryable().Skip(skip).Take(take).ToList();
            else
                return collection.AsQueryable().Where(predicate).Skip(skip).Take(take).ToList();
        }
        #endregion
        
        #region "Insert"

        public virtual string Insert<T>(T entity) where T : IGSIDEntity
        {
            if (!IsProcessedByQueue) SetDocumentDefaults(entity);

            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            entity.AddedByDate = DateTime.Now;

            //The default WriteConcern here is "Acknowledged". Go ahead and override this method in particular repositories if you need other ways of writing to
            //a mongo collection.            
            //collection.Insert(entity, WriteConcern.Acknowledged);
            collection.InsertOneAsync(entity);

            //Insert into history collection
            Task.Run(() =>
                    InsertOneIntoHistory<T>(entity)
                );


            return entity.Id;
        }

        /// <summary>
        /// Batch insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>        
        /// <returns>List of IDs of the generated documents</returns>
        public virtual List<string> Insert<T>(List<T> entities) where T : IGSIDEntity
        {
            if (!IsProcessedByQueue) SetDocumentDefaults(entities);

            var collection = DbContext.GetCollection<T>(typeof(T).Name);

            //var result = collection.InsertBatch(entities, WriteConcern.Acknowledged);
            var result = collection.InsertManyAsync(entities);

            //Insert into history collection
            Task.Run(() =>
                    InsertManyIntoHistory<T>(entities)
                );

            return entities.Select(c => c.Id).ToList();
        }

        /// <summary>
        /// Bulk insert, very useful in cases of data migration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>        
        /// <returns>List of IDs of the generated documents</returns>
        public virtual List<string> BulkInsert<T>(List<T> entities) where T : IGSIDEntity
        {
            List<InsertOneModel<T>> insertList = new List<InsertOneModel<T>>();
            if (!IsProcessedByQueue) SetDocumentDefaults(entities);

            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            foreach (T entity in entities) { 
                InsertOneModel<T> model = new InsertOneModel<T>(entity);
                insertList.Add(model);
            }

            var bulk = collection.BulkWrite(insertList);
            //Insert into history collection
            Task.Run(() =>
                    InsertManyIntoHistory<T>(entities)
                );

            return entities.Select(c => c.Id).ToList();
        }
        #endregion

        #region "Update"
        /// <summary>
        /// Update for complete object graph; when all fields are supplied. Else, do it the other way mentioned in "ClientRepository.Update() method"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>        
        /// <returns></returns>
        public virtual UpdateResult Update<T>(T entity) where T : IGSIDEntity            
        {
            UpdateResult result = null;
            var collectionName = typeof(T).Name;

            var collection = DbContext.GetCollection<T>(collectionName);
            entity.EditedByDate = DateTime.Now;

            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(entity.Id));
            var currentVersion = collection.Find(filter).FirstOrDefault().Version;

            if (entity.Version == currentVersion) //handling concurrency; the request having the correct document version would win.
            {
                if (!IsProcessedByQueue) SetDocumentDefaults(entity);

                //var result = collection.Save<T>(entity, WriteConcern.Acknowledged);
                //result = collection.UpdateOne<T>(x => x.Id.Equals(entity.Id), new BsonDocumentUpdateDefinition<T>(entity.ToBsonDocument()));
                
                collection.ReplaceOneAsync(
                     item => item.Id == entity.Id,
                     entity,
                     new UpdateOptions { IsUpsert = true });
                Task.Run(() =>
                    InsertOneIntoHistory<T>(entity)
                );
            }

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<UpdateResult> UpdateAsync<T>(T entity, CancellationToken cancellationToken = new CancellationToken()) where T : IGSIDEntity
        {
            Task<UpdateResult> result = null;
            var collectionName = typeof(T).Name;

            var collection = DbContext.GetCollection<T>(collectionName);
            
            var filter = Builders<T>.Filter.Eq("_id", new ObjectId(entity.Id));
            var currentVersion = collection.Find(filter).FirstOrDefault().Version;

            if (entity.Version == currentVersion) //handling concurrency; the request having the correct document version would win.
            {
                if (!IsProcessedByQueue) SetDocumentDefaults(entity);

                //var result = collection.Save<T>(entity, WriteConcern.Acknowledged);
                result = collection.UpdateOneAsync(filter, new BsonDocumentUpdateDefinition<T>(entity.ToBsonDocument()), cancellationToken: cancellationToken);

                Task.Run(() =>
                    InsertOneIntoHistory<T>(entity)
                );
            }

            return result;
        }
        

        /// <summary>
        /// Bulk update based on MongoQuery and MongoUpdate(low level querying patterns).
        /// To process by a queue please set IMongoQuery and IMongoUpdate queries yourself, as in all other Insert and update operations 
        /// you set the particular Entity. Though I've never seen people updating in bulk in most business scenarios.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="update"></param>        
        /// <returns></returns>
        //public virtual long BulkUpdate<T>(IMongoQuery query, IMongoUpdate update) where T : IGSIDEntity
        //{
        //    var collectionName = typeof(T).Name;

        //    var collection = DbContext.GetCollection<T>(collectionName);

        //    var bulk = collection.BulkWrite();

        //    var updateBuilder = (UpdateBuilder<T>)update;

        //    if (!IsProcessedByQueue)
        //    {
        //        //set defaults
        //        updateBuilder.Inc(c => c.Version, 1);
        //        updateBuilder.Set(c => c.CreatedBy, CurrentUser);
        //        updateBuilder.Set(c => c.CreatedDate, CurrentDate);
        //    }

        //    bulk.Find(query).Update(updateBuilder);

        //    var modifiedCount = bulk.Execute(WriteConcern.Acknowledged).ModifiedCount;

        //    var docs = collection.FindAs<T>(query).ToList();

        //    //Insert into history
        //    Task.Run(() =>
        //        InsertManyIntoHistory<T>(docs)
        //    );

        //    return modifiedCount;
        //}

        #endregion

        #region "Delete"

        /// <summary>
        /// Delete by Id. This is a permanent delete from the collection, but a document is inserted into history.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">Document Id</param>        
        /// <returns></returns>        
        public virtual DeleteResult Delete<T>(string id) where T : IGSIDEntity
        {   
            var collection = DbContext.GetCollection<T>(typeof(T).Name);

            var doc = GetOne<T>(id);

            if (!IsProcessedByQueue && doc!= null)
            {
                doc.Version = -1; // -1 means deleted here.
                //doc.CreatedBy = CurrentUser;
                doc.AddedByDate = CurrentDate;
            }

            //Insert an entry into history first
            Task.Run(() =>
                    InsertOneIntoHistory<T>(doc)
                );


            var result = collection.DeleteOne(Builders<T>.Filter.Eq("_id", new ObjectId(id)));

            return result;
        }

        /// <summary>
        /// Delete by predicate for a smaller batch size; 500 or so.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual bool Delete<T>(Expression<Func<T, bool>> predicate = null) where T : IGSIDEntity
        {
            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            
            List<T> docs;
            
            if (predicate == null)
                docs = collection.AsQueryable().ToList();
            else
                docs = collection.AsQueryable().Where(predicate).ToList();

            if (!IsProcessedByQueue) {
                foreach (var doc in docs) {
                    doc.Version = -1;
                    //doc.CreatedBy = CurrentUser;
                    doc.AddedByDate = CurrentDate;
                }
            }

            Task.Run(() =>
                   InsertManyIntoHistory<T>(docs)
               );
            if (predicate == null)
               DbContext.DropCollection(typeof(T).Name);
            else
                collection.DeleteMany(predicate);
            
            return true;
        }

        /// <summary>
        /// Bulk delete
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">MongoQuery: an example could be something like this; Query<T>.In<string>(e => e.Id, ids). 
        /// To delete all documents, set Query as Query.Null</param>        
        /// <returns></returns>
        //public virtual long BulkDelete<T>(IMongoQuery query) where T : IGSIDEntity
        //{
        //    var collection = DbContext.GetCollection<T>(typeof(T).Name);

        //    var bulk = collection.InitializeOrderedBulkOperation();

        //    var docs = collection.FindAs<T>(query).ToList();

        //    if (!IsProcessedByQueue)
        //    {
        //        foreach (var doc in docs)
        //        {
        //            doc.Version = -1;
        //            doc.CreatedBy = CurrentUser;
        //            doc.CreatedDate = CurrentDate;
        //        }
        //    }

        //    Task.Run(() =>
        //           InsertManyIntoHistory<T>(docs)
        //       );

        //    bulk.Find(query).Remove();

        //    return bulk.Execute(WriteConcern.Acknowledged).ModifiedCount;
        //}


        #region drop

        private bool DropDatabase(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            DbContext.Client.DropDatabase(name, cancellationToken);

            return true;
        }

        public virtual bool DropDatabase(CancellationToken cancellationToken = new CancellationToken())
        {
            return DropDatabase(cancellationToken);
        }

        private bool DropDatabaseAsync(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            DbContext.Client.DropDatabaseAsync(name, cancellationToken);

            return true;
        }

        public virtual bool DropDatabaseAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return DropDatabaseAsync(cancellationToken);
        }

        public virtual bool DropCollection<T>() where T : IGSIDEntity
        {
            var database = DbContext.Client.GetDatabase(databaseName.Value);
            var collectionName = typeof(T).Name.ToLower();

            if (DbContext.GetCollection<T>(collectionName, null) != null)
            {
                database.DropCollection(collectionName);
                return true;
            }
            return false;
        }
        #endregion

        #endregion

        #endregion
        
        public virtual List<T> All<T>() where T : IGSIDEntity
        {
            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            return collection.AsQueryable().ToList();
        }


        #region "Full text search"

        /// <summary>
        /// Equivalent to a term query in lucene; a great feature. Wild card searches are not supported at the moment with a text index.
        /// Also do not forget to create a text index on the collection referred; eg. db.Author.ensureIndex({nm : "text"})
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="term"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public virtual List<T> GetManyByTextSearch<T>(string term,  int take = -1, int skip = 0) where T : IGSIDEntity
        {
            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            
            var filter = Builders<T>.Filter.Text(term);

            if(take == -1)
                return collection.Find<T>(filter).Skip(skip).ToList();
            else
                return collection.Find<T>(filter).Skip(skip).Limit(take).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="term"></param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual Task<List<T>> GetManyByTextSearch<T>(string term, int take = -1, int skip = 0, CancellationToken cancellationToken = new CancellationToken()) where T : IGSIDEntity
        {
            var collection = DbContext.GetCollection<T>(typeof(T).Name);

            var filter = Builders<T>.Filter.Text(term);

            if (take == -1)
                return collection.Find<T>(filter).Skip(skip).ToListAsync(cancellationToken);
            else
                return collection.Find<T>(filter).Skip(skip).Limit(take).ToListAsync(cancellationToken);
        }


        #endregion

        #region "Other methods; GetCount(), GetOptionSets()"
        
        /// <summary>
        /// This is a default implementation. Returns a single pair of DenormalizedReference type. 
        /// This can be overridden for specific repositories for different databases.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDenormalizedReference"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual TDenormalizedReference GetOptionById<TEntity, TDenormalizedReference>(string Id) 
            where TEntity : IGSIDEntity
            where TDenormalizedReference : IDenormalizedReference, new()
        {
            var collection = DbContext.GetCollection<TEntity>(typeof(TEntity).Name);
                        
            //return collection.AsQueryable().Where(c => c.Id == Id).Select(c => new TDenormalizedReference { DenormalizedId = c.Id, DenormalizedName = c.Name }).SingleOrDefault();
            return collection.AsQueryable().Where(c => c.Id == Id).Select(c => new TDenormalizedReference { DenormalizedId = c.Id, DenormalizedName = "" }).SingleOrDefault();
        }

        /// <summary>
        /// Returns a list of DenormalizedReference types; Get optionSets based on a predicate.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TDenormalizedReference"></typeparam>
        /// <param name="predicate">the default predicate considered is the IsActive field to be true; (p => p.IsActive == true)</param>
        /// <param name="take"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public virtual List<TDenormalizedReference> GetOptionSet<TEntity, TDenormalizedReference>(Expression<Func<TEntity, bool>> predicate = null, int take = -1, int skip = 0)
            where TEntity : IGSIDEntity
            where TDenormalizedReference : IDenormalizedReference, new()
        {
            if (take == -1) take = takeCount;

            var collection = DbContext.GetCollection<TEntity>(typeof(TEntity).Name);

            if (predicate == null)
            {   
                    return collection.AsQueryable()
                                  //.Select(c => new TDenormalizedReference { DenormalizedId = c.Id, DenormalizedName = c.Name })
                        .Select(c => new TDenormalizedReference { DenormalizedId = c.Id, DenormalizedName = "" })
                        .OrderBy(c => c.DenormalizedName)
                        .Skip(skip).Take(take)                        
                        .ToList();
            }
            else
            {                
                    return collection.AsQueryable().Where(predicate)
                        //.Select(c => new TDenormalizedReference { DenormalizedId = c.Id, DenormalizedName = c.Name })
                        .Select(c => new TDenormalizedReference { DenormalizedId = c.Id, DenormalizedName = "" })
                        .OrderBy(c => c.DenormalizedName).Skip(skip).Take(take)
                        .ToList();
            }
        }

        /// <summary>
        /// Returns the count of records in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">Optional value is null. If predicate is null, it counts only the active records</param>
        /// <returns></returns>
        public virtual long GetCount<T>(Expression<Func<T, bool>> predicate = null) where T : IGSIDEntity
        {            
            var collection = DbContext.GetCollection<T>(typeof(T).Name);
                        
            if (predicate == null)
                return collection.AsQueryable().Count();
            else                            
                return collection.AsQueryable().Where(predicate).Count();            
        }

        #endregion

        #region "History and defaults"

        public List<T> GetHistory<T>(string id, int take = -1, int skip = 0) where T : IGSIDEntity
        {
            if (take == -1) take = takeCount;

            var collectionGSIDX = DbContext.GetCollection<GSIDMongoEntityX<T>>(typeof(T).Name + "GSIDX");

            return collectionGSIDX.AsQueryable()
                .Where(c => c.XDocument.Id == id)
                .OrderByDescending(c => c.XDocument.Version)
                .Skip(skip).Take(take)
                .Select(c => c.XDocument).ToList();
        }

        public void InsertOneIntoHistory<T>(T entity) where T : IGSIDEntity
        {            
            GSIDMongoEntityX<T> xDoc = new GSIDMongoEntityX<T>
            {
                XDocument = entity,
            };

            var collectionGSIDX = DbContext.GetCollection<GSIDMongoEntityX<T>>(typeof(T).Name + "GSIDX");

            collectionGSIDX.InsertOneAsync(xDoc);
        }

        public void InsertManyIntoHistory<T>(List<T> entities) where T : IGSIDEntity
        {
            List<InsertOneModel<GSIDMongoEntityX<T>>> xDocs = new List<InsertOneModel<GSIDMongoEntityX<T>>>();

            foreach (var doc in entities)
            {
                InsertOneModel<GSIDMongoEntityX<T>> model = new InsertOneModel<GSIDMongoEntityX<T>>(new GSIDMongoEntityX<T>() {
                    XDocument = doc
                });

                xDocs.Add(model);
            }
            var collectionGSIDX = DbContext.GetCollection<GSIDMongoEntityX<T>>(typeof(T).Name + "GSIDX");
            //var bulk = collectionGSIDX.BulkWrite(xDocs);
        }
        
        public void SetDocumentDefaults(IGSIDEntity entity)
        {
            entity.Version = entity.Version + 1;
            //entity.CreatedBy = CurrentUser;
            entity.AddedByDate = CurrentDate;
        }

        public void SetDocumentDefaults<T>(List<T> entities) where T : IGSIDEntity
        {
            foreach (var entity in entities)
            {
                entity.Version = entity.Version + 1;
                //entity.CreatedBy = CurrentUser;
                entity.AddedByDate = CurrentDate;
            }
        }

        public void Dispose()
        {
            //DbContext.Dispose();
        }
        #endregion

        protected Task<IAsyncCursor<T>> GetEqAsyncCursor<T>(string collectionName,
            IDictionary<string, string> fieldEqValue = null,
            IDictionary<string, string> fieldContainsValue = null,
            IDictionary<string, IEnumerable<string>> fieldEqValues = null,
            IDictionary<string, IEnumerable<string>> fieldElemMatchInValues = null,
            IEnumerable<ObjectId> ids = null)
        {
            var collection = DbContext.GetCollection<T>(typeof(T).Name);
            var builder = Builders<T>.Filter;

            IList<FilterDefinition<T>> filters = new List<FilterDefinition<T>>();

            if (fieldEqValue != null &&
                fieldEqValue.Any())
            {
                filters.Add(fieldEqValue
                            .Select(p => builder.Eq(p.Key, p.Value))
                            .Aggregate((p1, p2) => p1 | p2));
            }

            if (fieldContainsValue != null &&
                fieldContainsValue.Any())
            {
                filters.Add(fieldContainsValue
                            .Select(p => builder.Regex(p.Key, new BsonRegularExpression($".*{p.Value}.*", "i")))
                            .Aggregate((p1, p2) => p1 | p2));
            }

            if (fieldEqValues != null &&
                fieldEqValues.Any())
            {
                foreach (var pair in fieldEqValues)
                {
                    foreach (var value in pair.Value)
                    {
                        filters.Add(builder.Eq(pair.Key, value));
                    }
                }
            }

            if (fieldElemMatchInValues != null &&
                fieldElemMatchInValues.Any())
            {
                var baseQuery = "{ \"%key%\": { $elemMatch: { $in: [%values%] } } }";
                foreach (var item in fieldElemMatchInValues)
                {
                    var replaceKeyQuery = baseQuery.Replace("%key%", item.Key);
                    var bsonQuery = replaceKeyQuery.Replace("%values%",
                                item.Value
                                    .Select(p => $"\"{p}\"")
                                    .Aggregate((value1, value2) => $"{value1},{ value2}"));
                    var filter = BsonSerializer.Deserialize<BsonDocument>(bsonQuery);
                    filters.Add(filter);
                }
            }

            if (ids != null &&
                ids.Any())
            {
                filters.Add(ids
                        .Select(p => builder.Eq("_id", p))
                        .Aggregate((p1, p2) => p1 | p2));
            }

            var filterConcat = builder.Or(filters);

            // Here's how you can debug the generated query
            //var documentSerializer = BsonSerializer.SerializerRegistry.GetSerializer<T>();
            //var renderedFilter = filterConcat.Render(documentSerializer, BsonSerializer.SerializerRegistry).ToString();

            return collection.FindAsync(filterConcat);
        }
    }//End of class CRUDBase
}
