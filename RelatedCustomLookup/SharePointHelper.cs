using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

namespace RelatedCustomLookup
{
    public static class SharePointHelper
    {
        #region Public Methods

        /// <summary>
        /// Get the list of all webs up to and including the web object provided.
        /// </summary>
        /// <param name="web">The web object to retrieve details of parent webs</param>
        /// <returns>Dictionary list of all webs up to and including the passed web object. Key is web Guid and Value is web title.</returns>
        /// <remarks>The order of dictionary will be from the root web (first item) down to the provided web.</remarks>
        public static Dictionary<Guid, string> GetListOfWebsFromWeb(SPWeb web)
        {
            Dictionary<Guid, string> webs = new Dictionary<Guid, string>();

            if (web == null)
                return webs;

            SPWeb workingWeb = web;
            while (!workingWeb.IsRootWeb)
            {
                webs.Add(workingWeb.ID, workingWeb.Title);
                workingWeb = workingWeb.ParentWeb;
            }
            webs.Add(workingWeb.ID, workingWeb.Title);

            return webs;
        }

        /// <summary>
        /// Get a list of all lists that match the given base type within the specified web.
        /// </summary>
        /// <param name="web">The web object to retrieve the lists from.</param>
        /// <param name="listType">The type of lists to retrieve.</param>
        /// <returns>A dictionary list of all lists for the given web that are of the specified list type, ordered by list title.</returns>
        public static Dictionary<Guid, string> GetListsForWeb(SPWeb web, SPBaseType listType)
        {
            Dictionary<Guid, string> lists = new Dictionary<Guid, string>();

            if (web == null)
                return lists;

            lists = (from list in web.Lists.Cast<SPList>()
                     where list.BaseType == listType
                     orderby list.Title
                     select list).ToDictionary(item => item.ID, item => item.Title);

            return lists;
        }

        /// <summary>
        /// Get a list of all fields that match the given field type within the specified list.
        /// </summary>
        /// <param name="list">The list object to retrieve the fields from.</param>
        /// <param name="fieldType">The type of fields to retrieve.</param>
        /// <returns>A dictionary list of all fields for the given list that are of the specified field type, ordered by title.</returns>
        public static Dictionary<Guid, string> GetFieldsForList(SPList list, SPFieldType fieldType)
        {
            Dictionary<Guid, string> columns = new Dictionary<Guid, string>();

            if (list == null)
                return columns;

            columns = (from field in list.Fields.Cast<SPField>()
                       where field.Type == fieldType
                       orderby field.Title
                       select field).ToDictionary(item => item.Id, item => item.Title);

            return columns;
        }

        /// <summary>
        /// Get a list of <see cref="T:LookupCustomField.Common.SharePointHelper.ExtendedLookupColumnValues"/> objects populated by the values
        /// of the displayColumnId and valueColumnId columns of sourceList.
        /// </summary>
        /// <param name="sourceList">The list to retrieve values from.</param>
        /// <param name="displayColumnId">The id of the display column.</param>
        /// <param name="valueColumnId">The id of the value column.</param>
        /// <returns>A list of <see cref="T:LookupCustomField.Common.SharePointHelper.ExtendedLookupColumnValues"/> objects.</returns>
        public static List<ExtendedLookupColumnValues> GetColumnValues(SPList sourceList, Guid displayColumnId, Guid valueColumnId, string queryStr)
        {
            List<ExtendedLookupColumnValues> items = new List<ExtendedLookupColumnValues>();

            if (sourceList == null)
                return items;
            if (!sourceList.Fields.Contains(displayColumnId))
            {
                return items;
            }
            if (!sourceList.Fields.Contains(valueColumnId))
            {
                return items;
            }
            SPQuery query = new SPQuery();
            query.Query = string.Format(queryStr);
            SPListItemCollection col = sourceList.GetItems(query);
            items = (from item in col.Cast<SPListItem>()
                     orderby item[displayColumnId]
                     select new ExtendedLookupColumnValues
                     {
                         Display = item[displayColumnId].ToString(),
                         Value = item[valueColumnId]
                     }
                    ).ToList();

            return items;
        }

        #endregion Public Methods

        #region Inner Class

        public class ExtendedLookupColumnValues
        {
            public string Display { get; set; }
            public object Value { get; set; }
        }

        #endregion Inner Class
    }
}