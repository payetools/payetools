﻿// Copyright (c) 2023 Paytools Foundation.
//
// Paytools Foundation licenses this file to you under one of the following licenses:
//
//  * GNU Affero General Public License, see https://www.gnu.org/licenses/agpl-3.0.html
//  * Paytools Commercial Use license [TBA]
//
// For further information on licensing options, see https://paytools.dev/licensing-paytools.html

using System.Runtime.CompilerServices;

namespace Paytools.Testing.Utils
{
    /// <summary>
    /// Provides support for asynchronous lazy initialization. This type is fully threadsafe.
    /// </summary>
    /// <typeparam name="T">The type of object that is being asynchronously initialized.</typeparam>
    /// <remarks>With acknowledgement to Stephen Cleary - see
    /// <see href="https://blog.stephencleary.com/2012/08/asynchronous-lazy-initialization.html"/>.</remarks>
    public sealed class AsyncLazy<T>
    {
        /// <summary>
        /// The underlying lazy task.
        /// </summary>
        private readonly Lazy<Task<T>> instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
        /// </summary>
        /// <param name="factory">The delegate that is invoked on a background thread to produce the value when it is needed.</param>
        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLazy{T}"/> class.
        /// </summary>
        /// <param name="factory">The asynchronous delegate that is invoked on a background thread to produce the value when it is needed.</param>
        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        /// <summary>
        /// Asynchronous infrastructure support. This method permits instances of <see cref="AsyncLazy{T}"/> to be await'ed.
        /// </summary>
        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }

        /// <summary>
        /// Starts the asynchronous initialization, if it has not already started.
        /// </summary>
        public void Start()
        {
            var unused = instance.Value;
        }
    }
}
