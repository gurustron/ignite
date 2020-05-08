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
    /// Ignite Thin Client transactions facade.
    /// </summary>
    internal class ClientTransactions : IClientTransactions
    {
        /** <inheritDoc /> */
        public IClientTransaction TxStart()
        {
            throw new NotImplementedException();
        }

        /** <inheritDoc /> */
        public IClientTransaction TxStart(TransactionConcurrency concurrency, TransactionIsolation isolation)
        {
            throw new NotImplementedException();
        }

        /** <inheritDoc /> */
        public IClientTransaction TxStart(TransactionConcurrency concurrency, TransactionIsolation isolation, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        /** <inheritDoc /> */
        public IClientTransactions WithLabel(string label)
        {
            throw new NotImplementedException();
        }
    }
}