/*
 * Copyright (c) 2016-2018 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * https://github.com/piranhacms/piranha.core
 * 
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Piranha.AspNetCore.Services;

namespace Piranha.AspNetCore
{
    /// <summary>
    /// Base class for middleware.
    /// </summary>
    public abstract class MiddlewareBase
    {
        private const string HandledByPiranhaKey = "piranha_handled";
        protected static PathString ManagerBasePath = new PathString("/manager/");

        /// <summary>
        /// The next middleware in the pipeline.
        /// </summary>
        protected readonly RequestDelegate _next;

        /// <summary>
        /// The optional logger.
        /// </summary>
        protected ILogger _logger;

        /// <summary>
        /// Creates a new middleware instance.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        protected MiddlewareBase(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Creates a new middleware instance.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline</param>
        /// <param name="factory">The logger factory</param>
        protected MiddlewareBase(RequestDelegate next, ILoggerFactory factory) : this(next)
        {
            if (factory != null)
            {
                _logger = factory.CreateLogger(this.GetType().FullName);
            }
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <param name="api">The current api</param>
        /// <returns>An async task</returns>
        public abstract Task Invoke(HttpContext context, IApi api, IApplicationService service);

        /// <summary>
        /// Checks if the request has already been handled by another
        /// Piranha middleware.
        /// </summary>
        /// <param name="context">The current http context</param>
        /// <returns>If the request has already been handled</returns>
        protected bool IsHandled(HttpContext context)
        {
            return context.Items.ContainsKey(HandledByPiranhaKey);
        }

        /// <summary>
        /// Marks a requests as already being handled by a Piranha middleware.
        /// </summary>
        /// <param name="context">The current http context</param>
        protected void SetIsHandled(HttpContext context)
        {
            context.Items[HandledByPiranhaKey] = true;
        }
    }
}
