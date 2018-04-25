using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint.WebControls;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RelatedCustomLookup
{
   public class RelatedCustomLookupFieldEditor : UserControl, IFieldEditor
{
    // Fields
    protected CheckBox cbxAllowMultiple;
    protected CheckBox cbxFile;
    protected CheckBoxList ddlDependentFields;
    protected DropDownList ddlFieldTitleLookup;
    protected DropDownList ddlFieldValueLookup;
    protected DropDownList ddlListNameLookup;
    private string dependentValue = "";
    private string fieldTitle = "";
    private string fieldValue = "";
    private string internalName = "";
    private bool isFile = false;
    protected Label lblSelectedLookupList;
    private SPList list = null;
    private string listNameValue = "";
    private string queryValue = "";
    private const string TEXT_FIELD = "Value";
    protected TextBox txtQuery;
    protected TextBox txtTypeFile;
    protected TextBox txtVolumeFile;
    private string typeFile = "";
    private const string VALUE_FIELD = "Key";
    private string volumeFile = "";

    // Methods
    private void BindDisplayColumns(SPList list)
    {
        this.ddlFieldTitleLookup.Items.Clear();
        this.ddlFieldValueLookup.Items.Clear();
        foreach (SPField field in list.Fields)
        {
            if (((!field.Hidden && !field.FromBaseType) || (!field.Hidden && (field.InternalName == "Title"))) || (!field.Hidden && (field.InternalName == "ID")))
            {
                ListItem item = new ListItem {
                    Text = field.Title,
                    Value = field.InternalName.ToString()
                };
                this.ddlFieldTitleLookup.Items.Add(item);
                ListItem item2 = new ListItem {
                    Text = field.Title,
                    Value = field.InternalName.ToString()
                };
                this.ddlFieldValueLookup.Items.Add(item2);
            }
        }
    }

    protected override void CreateChildControls()
    {
        base.CreateChildControls();
        if (!base.IsPostBack)
        {
            using (SPWeb web = SPContext.Current.Site.OpenWeb())
            {
                foreach (SPList list in web.Lists)
                {
                    if (!list.Hidden)
                    {
                        this.ddlListNameLookup.Items.Add(new ListItem(list.Title, list.ID.ToString()));
                    }
                }
                foreach (SPField field in this.list.Fields)
                {
                    if (((!field.Hidden && !field.FromBaseType) && (field.InternalName != this.internalName)) || (!field.Hidden && (field.InternalName == "Title")))
                    {
                        ListItem item = new ListItem {
                            Text = field.Title,
                            Value = field.InternalName.ToString()
                        };
                        this.ddlDependentFields.Items.Add(item);
                    }
                }
                if (this.listNameValue != "")
                {
                    this.ddlListNameLookup.SelectedValue = this.listNameValue;
                    this.SetAsReadOnly(this.lblSelectedLookupList, this.ddlListNameLookup.SelectedItem.Text, this.ddlListNameLookup);
                }
                if (this.ddlListNameLookup.SelectedIndex != -1)
                {
                    SPList list2 = web.Lists[new Guid(this.ddlListNameLookup.SelectedValue)];
                    this.BindDisplayColumns(list2);
                }
            }
            if (this.isFile)
            {
                this.cbxFile.Enabled = false;
                this.txtTypeFile.Enabled = false;
                this.txtVolumeFile.Enabled = false;
            }
            if (this.fieldTitle != "")
            {
                ListItem item2 = this.ddlFieldTitleLookup.Items.FindByValue(this.fieldTitle);
                if (item2 != null)
                {
                    item2.Selected = true;
                }
            }
            if (this.fieldValue != "")
            {
                ListItem item3 = this.ddlFieldValueLookup.Items.FindByValue(this.fieldValue);
                if (item3 != null)
                {
                    item3.Selected = true;
                }
            }
            ListItem item4 = this.ddlListNameLookup.Items.FindByValue(this.listNameValue);
            if (item4 != null)
            {
                item4.Selected = true;
            }
            if (this.dependentValue != "")
            {
                string[] strArray = this.dependentValue.Split(new char[] { '|' });
                foreach (string str in strArray)
                {
                    this.ddlDependentFields.Items.FindByValue(str).Selected = true;
                }
            }
            if (this.queryValue != "")
            {
                this.txtQuery.Text = this.queryValue;
            }
            if (this.typeFile != "")
            {
                this.txtTypeFile.Text = this.typeFile;
            }
            if (this.volumeFile != "")
            {
                this.txtVolumeFile.Text = this.volumeFile;
            }
            this.cbxFile.Checked = this.isFile;
        }
    }

    protected void ddlListNameLookup_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList list = sender as DropDownList;
        if (list.SelectedItem != null)
        {
            using (SPWeb web = SPContext.Current.Site.OpenWeb())
            {
                Guid uniqueId = new Guid(list.SelectedItem.Value);
                SPList list2 = web.Lists.GetList(uniqueId, false);
                this.BindDisplayColumns(list2);
            }
            this.ddlFieldTitleLookup.Enabled = true;
            this.ddlFieldValueLookup.Enabled = true;
        }
        else
        {
            this.ddlFieldTitleLookup.Enabled = false;
            this.ddlFieldValueLookup.Enabled = false;
        }
    }

    public void InitializeWithField(SPField field)
    {
        if (!this.Page.IsPostBack)
        {
            RelatedCustomLookup lookup = field as RelatedCustomLookup;
            this.list = SPContext.Current.List;
            if (lookup != null)
            {
                this.listNameValue = lookup.ListNameLookup;
                this.fieldTitle = lookup.FieldTitleLookup;
                this.fieldValue = lookup.FieldValueLookup;
                this.dependentValue = lookup.RelatedFields;
                this.queryValue = lookup.QueryLookup;
                this.internalName = lookup.InternalName;
                if (lookup.IsFile == "بله")
                {
                    this.isFile = true;
                }
                else if (lookup.IsFile == "خیر")
                {
                    this.isFile = false;
                }
                else
                {
                    this.isFile = bool.Parse(lookup.IsFile);
                }
                this.volumeFile = lookup.VolumeFile;
                this.typeFile = lookup.TypeFile;
            }
        }
    }

    public void OnSaveChange(SPField field, bool bNewField)
    {
        string str = "";
        foreach (ListItem item in this.ddlDependentFields.Items)
        {
            if (item.Selected)
            {
                str = str + item.Value + "|";
            }
        }
        str = str.TrimEnd(new char[] { '|' });
        string selectedValue = this.ddlListNameLookup.SelectedValue;
        string str3 = this.ddlFieldTitleLookup.SelectedValue;
        string str4 = this.ddlFieldValueLookup.SelectedValue;
        string text = this.txtQuery.Text;
        string str6 = this.cbxFile.Checked.ToString();
        string str7 = this.txtVolumeFile.Text;
        string str8 = this.txtTypeFile.Text;
        RelatedCustomLookup lookup = field as RelatedCustomLookup;
        if (bNewField)
        {
            lookup.UpdateListNameLookup(selectedValue);
            lookup.UpdateFieldTitleLookup(str3);
            lookup.UpdateFieldValueLookup(str4);
            lookup.UpdateQueryLookup(text);
            lookup.UpdateRelatedFields(str);
            lookup.UpdateIsFile(str6.ToString());
            lookup.UpdateTypeFile(str8);
            lookup.UpdateVolumeFile(str7);
        }
        else
        {
            lookup.ListNameLookup = selectedValue;
            lookup.FieldTitleLookup = str3;
            lookup.FieldValueLookup = str4;
            lookup.QueryLookup = text;
            lookup.RelatedFields = str;
            lookup.IsFile = str6;
            lookup.TypeFile = str8;
            lookup.VolumeFile = str7;
        }
    }

    private void SetAsReadOnly(Label itemLabel, string itemText, DropDownList dropDownList)
    {
        itemLabel.Text = itemText;
        itemLabel.Visible = true;
        dropDownList.Visible = false;
    }

    // Properties
    public bool DisplayAsNewSection
    {
        get
        {
            return false;
        }
    }
}

 

 

 

 

}
