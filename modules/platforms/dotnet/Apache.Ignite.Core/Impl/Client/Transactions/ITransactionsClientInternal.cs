/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace Apache.Ignite.Core.Impl.Client.Transactions
{
    using System;
    using Apache.Ignite.Core.Client.Transactions;
    using Apache.Ignite.Core.Transactions;

    /// <summary>
    /// Internal Ignite Thin Client transactions facade.
    /// </summary>
    internal interface ITransactionsClientInternal : ITransactionsClient
    {
        /// <summary>
        /// Current thread transaction
        /// </summary>
        ITransactionClientInternal CurrentTx { get; }

        /// <summary>
        /// If ambient transaction is present, starts an Ignite transaction and enlists it.
        /// </summary>
        void StartTxIfNeeded();

        /// <summary>
        /// Default transaction concurrency.
        /// </summary>
        TransactionConcurrency DefaultTxConcurrency {get;}

        /// <summary>
        /// Default transaction isolation.
        /// </summary>
        TransactionIsolation DefaultTxIsolation { get; }

        /// <summary>
        /// Default transaction timeout.
        /// </summary>
        TimeSpan DefaultTimeout { get; }
    }
}
