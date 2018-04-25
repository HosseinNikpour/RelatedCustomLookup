using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using System.Web;

namespace RelatedCustomLookup
{

    #region Util class
    internal sealed class Util
    {

        #region ListIsNullOrEmpty method
        /// <summary>
        /// Indicates whether the specified generic list object is null or empty
        /// </summary>
        /// <param name="list">A generic list</param>
        /// <returns>A value indicating whether the specified list object is null or empty</returns>
        internal static bool ListIsNullOrEmpty(List<ListItem> list)
        {
            return (list != null && list.Count > 0) ? false : true;
        }
        #endregion

        #region GetAvailableValues method
        internal static List<ListItem> GetAvailableValues(RelatedCustomLookup f, HttpContext ctx)
        {
            List<ListItem> _v = null;

            Guid fId = new Guid(f.FieldTitleLookup);

            using (SPSite s = SPControl.GetContextSite(ctx))
            {
                using (SPWeb lookupWeb = s.OpenWeb())
                {
                    SPList lookupList = lookupWeb.Lists[new Guid(f.ListNameLookup)];


                    SPQuery query = new SPQuery();
                    query.Query = string.Format(f.QueryLookup);
                    SPListItemCollection col = lookupList.GetItems(query);

                    _v = (from item in col.Cast<SPListItem>()
                          orderby item[new Guid(f.FieldTitleLookup)]
                          select new ListItem
                          {
                              Text = item[new Guid(f.FieldTitleLookup)].ToString(),
                              Value = item[new Guid(f.FieldValueLookup)].ToString()
                          }
                            ).ToList();




                }
            }
            return _v;
        }
        #endregion
    }
    #endregion

    #region Extensions class
    internal static class Extensions
    {
        // TO DO
        /// <summary>
        /// Indicates whether a field in a list is associated with a SPFolder content type
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        internal static bool AssociatedWithFolder(this SPField field)
        {
            // THIS IS WORK IN PROGRESS
            if (field != null)
            {
                SPList list = field.ParentList;
            }

            return false;
        }
    }
    #endregion
}
