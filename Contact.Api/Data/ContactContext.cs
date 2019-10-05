﻿using Contact.Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Data
{
    public class ContactContext
    {
        private IMongoDatabase _database;
        private IMongoCollection<ContactBook> _collection;
        private AppSettings _appSettings;
        public ContactContext(IOptionsSnapshot<AppSettings> settings)
        {
            _appSettings = settings.Value;
            var client = new MongoClient(_appSettings.MongoContactConnectionString);
            if (client != null)
            {
                _database = client.GetDatabase(_appSettings.MongoContactDatabase);
            }
        }
        /// <summary>
        /// 判断Mongo是否存在Collection不存在就创建
        /// </summary>
        /// <param name="collectionname"></param>
        private void ChekAndCreateCollection(string collectionname)
        {
            var collectionList = _database.ListCollections().ToList();
            var collectionnames = new List<string>();
            collectionList.ForEach(q => collectionnames.Add(q["name"].AsString));
            if (!collectionnames.Contains(collectionname))
            {
                _database.CreateCollection(collectionname);
            }
        }
        /// <summary>
        /// 用户通讯录
        /// </summary>
        public IMongoCollection<ContactBook> ContactBooks
        {
            get
            {
                ChekAndCreateCollection("ContactBook");
                return _database.GetCollection<ContactBook>("ContactBook");
            }
        }
        /// <summary>
        /// 好友申请请求记录
        /// </summary>
        public IMongoCollection<ContactApplyRequest> ContactApplyRequests
        {
            get
            {
                ChekAndCreateCollection("ContactApplyRequest");
                return _database.GetCollection<ContactApplyRequest>("ContactApplyRequest");
            }
        }
    }
}
