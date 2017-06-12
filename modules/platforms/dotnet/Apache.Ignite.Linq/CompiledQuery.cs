﻿/*
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

namespace Apache.Ignite.Linq
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Apache.Ignite.Core.Cache.Query;
    using Apache.Ignite.Core.Impl.Common;
    using Apache.Ignite.Linq.Impl;
    using Remotion.Linq.Parsing;

    /// <summary>
    /// Delegate for compiled query with arbitrary number of arguments.
    /// </summary>
    /// <typeparam name="T">Result type.</typeparam>
    /// <param name="args">The arguments.</param>
    /// <returns>Query cursor.</returns>
    public delegate IQueryCursor<T> CompiledQueryFunc<T>(params object[] args);

    /// <summary>
    /// Represents a compiled cache query.
    /// </summary>
    public static class CompiledQuery
    {
        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<IQueryCursor<T>> Compile<T>(Expression<Func<IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return () => compiledQuery(new object[0]);
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query with any number of arguments.
        /// <para />
        /// This method differs from other Compile methods in that it takes in <see cref="ICacheQueryable"/> directly,
        /// and returns a delegate that takes an array of parameters.
        /// It is up to the user to provide query arguments in correct order.
        /// <para />
        /// This method also imposes no restrictions on where the query comes from (in contrary to other methods).
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static CompiledQueryFunc<T> Compile<T>(IQueryable<T> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var cacheQueryable = query as ICacheQueryableInternal;

            if (cacheQueryable == null)
                throw GetInvalidQueryException(query);

            var compileQuery = cacheQueryable.CompileQuery<T>();

            // Special delegate is required to allow params[].
            return args => compileQuery(args);
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, IQueryCursor<T>> Compile<T, T1>(Expression<Func<T1, IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return x => compiledQuery(new object[] {x});
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, T2, IQueryCursor<T>> Compile<T, T1, T2>(Expression<Func<T1, T2, 
            IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return (x, y) => compiledQuery(new object[] {x, y});
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, T2, T3, IQueryCursor<T>> Compile<T, T1, T2, T3>(Expression<Func<T1, T2, T3,
            IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return (x, y, z) => compiledQuery(new object[] {x, y, z});
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, T2, T3, T4, IQueryCursor<T>> Compile<T, T1, T2, T3, T4>(Expression<Func<T1, T2, T3, T4,
            IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return (x, y, z, a) => compiledQuery(new object[] {x, y, z, a});
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, T2, T3, T4, T5, IQueryCursor<T>> Compile<T, T1, T2, T3, T4, T5>(
            Expression<Func<T1, T2, T3, T4, T5, IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return (x, y, z, a, b) => compiledQuery(new object[] {x, y, z, a, b});
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, T2, T3, T4, T5, T6, IQueryCursor<T>> Compile<T, T1, T2, T3, T4, T5, T6>(
            Expression<Func<T1, T2, T3, T4, T5, T6, IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return (x, y, z, a, b, c) => compiledQuery(new object[] {x, y, z, a, b, c});
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, T2, T3, T4, T5, T6, T7, IQueryCursor<T>> Compile<T, T1, T2, T3, T4, T5, T6, T7>(
            Expression<Func<T1, T2, T3, T4, T5, T6, T7, IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return (x, y, z, a, b, c, d) => compiledQuery(new object[] {x, y, z, a, b, c, d});
        }

        /// <summary>
        /// Creates a new delegate that represents the compiled cache query.
        /// </summary>
        /// <param name="query">The query to compile.</param>
        /// <returns>Delegate that represents the compiled cache query.</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", 
            Justification = "Invalid warning, validation is present.")]
        public static Func<T1, T2, T3, T4, T5, T6, T7, T8, IQueryCursor<T>> Compile<T, T1, T2, T3, T4, T5, T6, T7, T8>(
            Expression<Func<T1, T2, T3, T4, T5, T6, T7, T8, IQueryable<T>>> query)
        {
            IgniteArgumentCheck.NotNull(query, "query");

            var compiledQuery = GetCompiledQuery<T>(query);

            return (x, y, z, a, b, c, d, e) => compiledQuery(new object[] {x, y, z, a, b, c, d, e});
        }

        /// <summary>
        /// Gets the compiled query.
        /// </summary>
        private static Func<object[], IQueryCursor<T>> GetCompiledQuery<T>(LambdaExpression expression)
        {
            Debug.Assert(expression != null);
            
            // Get default parameter values.
            var paramValues = expression.Parameters
                .Select(x => x.Type)
                .Select(x => x.IsValueType ? Activator.CreateInstance(x) : null)
                .ToArray();

            var transformingxpressionVisitor = new JoinInnerSequenceParameterTransformingExpressionVisitor();
            var queryCaller = (LambdaExpression)transformingxpressionVisitor.Visit(expression);

            // Compile and invoke the delegate to obtain the cacheQueryable.
            var queryable = queryCaller.Compile().DynamicInvoke(paramValues);

            var cacheQueryable = queryable as ICacheQueryableInternal;

            if (cacheQueryable == null)
                throw GetInvalidQueryException(queryable);

            return cacheQueryable.CompileQuery<T>(expression);
        }

        /// <summary>
        /// Transforms JoinClause with parameterised inner sequence to .Join(innerSequence ?? new T[0] ... 
        /// </summary>
        class JoinInnerSequenceParameterTransformingExpressionVisitor : RelinqExpressionVisitor
        {
            /** */
            private static readonly MethodInfo[] JoinMethods = typeof(Queryable).GetMethods()
                .Where(info => info.Name == "Join")
                .ToArray();

            /** */
            private static readonly Type EnumerableType = typeof(IEnumerable);

            /** */
            private bool _inJoin;

            /** <inheritdoc /> */
            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.IsGenericMethod)
                {
                    var genericMethodDefinition = node.Method.GetGenericMethodDefinition();

                    if (JoinMethods.Any(mi => mi == genericMethodDefinition))
                    {
                        _inJoin = true;
                        //var args = node.Arguments
                        //    .ToArray();

                        //// Get inner sequence arg
                        //var innerSequenceArg = args[1];

                        //// Coalesce only if innerSequence is parameter expression
                        //if (innerSequenceArg.NodeType == ExpressionType.Parameter)
                        //{
                        //    var itemType = EnumerableHelper.GetIEnumerableItemType(innerSequenceArg.Type);

                        //    args[1] = Expression.Coalesce(innerSequenceArg, Expression.NewArrayBounds(itemType, Expression.Constant(0)));

                        //    var updatedNode = node.Update(node.Object, args);

                        //}
                    }
                }

                var result = base.VisitMethodCall(node);

                _inJoin = false;

                return result;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                if (_inJoin && EnumerableType.IsAssignableFrom(node.Type))
                {
                    var itemType = EnumerableHelper.GetIEnumerableItemType(node.Type);
                    return Expression.Coalesce(node, Expression.NewArrayBounds(itemType, Expression.Constant(0)));
                }

                return node;
            }
        }


        private static object GetValue(Type type)
        {
            if (type == typeof(string))
            {
                return null;
            }

            Type itemType = null;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                itemType = type.GetGenericArguments()[0];
            }
            else
            {
                var implementedIEnumerableType = type.GetInterfaces()
                    .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                if (implementedIEnumerableType == null)
                {
                    throw new NotSupportedException("Not supported collection type for Join with local collection: " + type.FullName);
                }

                itemType = implementedIEnumerableType.GetGenericArguments()[0];
            }

            if (itemType == null)
            {
                return null;
            }

            var emptyEnumerable = typeof(Enumerable).GetMethod("Empty").MakeGenericMethod(itemType).Invoke(null, null);
            return emptyEnumerable;
        }

        /// <summary>
        /// Gets the invalid query exception.
        /// </summary>
        private static ArgumentException GetInvalidQueryException(object queryable)
        {
            return new ArgumentException(
                string.Format("{0} can only compile cache queries produced by AsCacheQueryable method. " +
                              "Provided query is not valid: '{1}'", typeof(CompiledQuery).FullName, queryable));
        }
    }
}
