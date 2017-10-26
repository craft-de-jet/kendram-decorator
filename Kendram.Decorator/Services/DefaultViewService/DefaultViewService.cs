/*
 * Copyright 2014, 2015 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Kendram.Decorator.Extensions;
using Kendram.Decorator.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kendram.Decorator.Services.Default
{
    /// <summary>
    /// Default view service.
    /// </summary>
    public class DefaultViewService : IViewService
    {
        /// <summary>
        /// The index view
        /// </summary>
        public const string IndexView = "index";

        static readonly Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };

        /// <summary>
        /// The configuration
        /// </summary>
        protected readonly DefaultViewServiceOptions config;
        
        /// <summary>
        /// The view loader
        /// </summary>
        protected readonly IViewLoader viewLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultViewService"/> class.
        /// </summary>
        public DefaultViewService(DefaultViewServiceOptions config, IViewLoader viewLoader)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (viewLoader == null) throw new ArgumentNullException("viewLoader");

            this.config = config;
            this.viewLoader = viewLoader;
        }

        /// <summary>
        /// Loads the HTML for the login page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        /// Stream for the HTML
        /// </returns>
        public virtual Task<Stream> Index(IndexViewModel model)
        {
            return Render(model, IndexView);
        }

        /// <summary>
        /// Renders the specified page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        protected virtual Task<Stream> Render(CommonViewModel model, string page)
        {
            return Render(model, page, config.Stylesheets, config.Scripts);
        }

        /// <summary>
        /// Renders the specified page.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="page">The page.</param>
        /// <param name="stylesheets">The stylesheets.</param>
        /// <param name="scripts">The scripts.</param>
        /// <returns></returns>
        protected virtual async Task<Stream> Render(CommonViewModel model, string page, IEnumerable<string> stylesheets, IEnumerable<string> scripts)
        {
            var data = BuildModelDictionary(model, page, stylesheets, scripts);

            string html = await LoadHtmlTemplate(page);
            if (html == null) return null;

            html = FormatHtmlTemplate(html, data);

            return html.ToStream();
        }

        /// <summary>
        /// Loads the HTML template.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        protected virtual Task<string> LoadHtmlTemplate(string page)
        {
            return this.viewLoader.LoadAsync(page);
        }

        /// <summary>
        /// Formats the specified HTML template.
        /// </summary>
        /// <param name="htmlTemplate">The HTML template.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected string FormatHtmlTemplate(string htmlTemplate, object model)
        {
            return AssetManager.Format(htmlTemplate, model);
        }

        /// <summary>
        /// Builds the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="page">The page.</param>
        /// <param name="stylesheets">The stylesheets.</param>
        /// <param name="scripts">The scripts.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// model
        /// or
        /// stylesheets
        /// or
        /// scripts
        /// </exception>
        protected object BuildModel(CommonViewModel model, string page, IEnumerable<string> stylesheets, IEnumerable<string> scripts)
        {
            return BuildModelDictionary(model, page, stylesheets, scripts);
        }

        Dictionary<string, object> BuildModelDictionary(CommonViewModel model, string page, IEnumerable<string> stylesheets, IEnumerable<string> scripts)
        {
            if (model == null) throw new ArgumentNullException("model");
            if (stylesheets == null) throw new ArgumentNullException("stylesheets");
            if (scripts == null) throw new ArgumentNullException("scripts");

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(model, Newtonsoft.Json.Formatting.None, settings);

            var data = new Dictionary<string, object>
            {
                { "siteName" , model.SiteName }, //Microsoft.Security.Application.Encoder.HtmlEncode(model.SiteName) },
                { "model", json }, //Microsoft.Security.Application.Encoder.HtmlEncode(json) },
                { "page", page },
            };

            //var applicationPath = new Uri(model.SiteUrl).AbsolutePath;
            //if (applicationPath.EndsWith("/")) applicationPath = applicationPath.Substring(0, applicationPath.Length - 1);

            //var additionalStylesheets = BuildTags("<link href='{0}' rel='stylesheet'>", applicationPath, stylesheets);
            //var additionalScripts = BuildTags("<script src='{0}'></script>", applicationPath, scripts);
            //    { "stylesheets", additionalStylesheets },
            //    { "scripts", additionalScripts }
            //};

            return data;
        }

        string BuildTags(string tagFormat, string basePath, IEnumerable<string> values)
        {
            if (values == null || !values.Any()) return String.Empty;

            var sb = new StringBuilder();
            foreach (var value in values)
            {
                var path = value;
                if (path.StartsWith("~/"))
                {
                    path = basePath + path.Substring(1);
                }
                sb.AppendFormat(tagFormat, path);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
