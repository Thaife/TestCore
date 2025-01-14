﻿using StackExchange.Redis;
using System;

namespace ApplicationCore.Interface.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// Get Data using key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetData<T>(string key);

        /// <summary>
        /// Set Data with Value and Expiration Time of Key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        bool SetData<T>(string key, T value, TimeSpan? expirationTime = null);

        /// <summary>
        /// Remove Data
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object RemoveData(string key);

        /// <summary>
        /// Create Transaction
        /// </summary>
        /// <param name="asyncState"></param>
        /// <returns></returns>
        ITransaction CreateTransaction(object? asyncState = null);

    }
}
