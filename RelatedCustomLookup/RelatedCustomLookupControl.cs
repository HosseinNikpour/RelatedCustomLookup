using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Diagnostics;
using Microsoft.SharePoint.Security;
using System.Security.Permissions;
namespace RelatedCustomLookup
{
    [System.Web.AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [System.Web.AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [Microsoft.SharePoint.Security.SharePointPermission(System.Security.Permissions.SecurityAction.LinkDemand, ObjectModel = true)]
    [Microsoft.SharePoint.Security.SharePointPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, ObjectModel = true)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [SharePointPermission(SecurityAction.LinkDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal), SharePointPermission(SecurityAction.InheritanceDemand, ObjectModel = true), AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]

    [CLSCompliant(false)]

public class RelatedCustomLookupControl : LookupField
{
    // Fields
    private List<ListItem> _availableItems = null;
    private SPFieldLookupValue _fieldVal;

    // Methods
    private string ConcatAvailableItems(string delimiter)
    {
        string str = string.Empty;
        if (!Util.ListIsNullOrEmpty(this._availableItems))
        {
            if (!this.Field.Required)
            {
                str = str + string.Format(CultureInfo.InvariantCulture, "{0}{1}{0}0", new object[] { delimiter, "(None)" });
            }
            foreach (ListItem item in this._availableItems)
            {
                str = str + string.Format("{0}{1}{0}{2}", delimiter, item.Text, item.Value);
            }
            return str.Trim().Substring(1);
        }
        return str;
    }

    protected override void CreateChildControls()
    {
        if (((base.Field != null) && (base.ControlMode != SPControlMode.Display)) && !base.ChildControlsCreated)
        {
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl("<span dir=\"none\">"));
            RelatedCustomLookup field = base.Field as RelatedCustomLookup;
            if (((this._availableItems != null) && (this._availableItems.Count > 0x13)) && this.IsExplorerOnWin())
            {
                this.CreateCustomSelect();
            }
            else
            {
                this.CreateStandardSelect();
            }
            this.Controls.Add(new LiteralControl("<br /></span>"));
        }
    }

    private void CreateCustomSelect()
    {
        HtmlInputHidden child = new HtmlInputHidden {
            ID = string.Format(CultureInfo.InvariantCulture, "{0}_Hidden", new object[] { this.Field.InternalName })
        };
        this.Controls.Add(child);
        this.Controls.Add(new LiteralControl("<span style=\"vertical-align: middle\">"));
        HtmlInputText text = new HtmlInputText {
            ID = "Txtbx"
        };
        text.Attributes.Add("class", "ms-lookuptypeintextbox");
        text.Attributes.Add("onfocusout", "HandleLoseFocus()");
        text.Attributes.Add("opt", "_Select");
        text.Attributes.Add("title", string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { this.Field.InternalName }));
        text.Attributes.Add("optHid", child.ClientID);
        text.Attributes.Add("onkeypress", "HandleChar()");
        text.Attributes.Add("onkeydown", "HandleKey()");
        text.Attributes.Add("match", "");
        text.Attributes.Add("choices", this.ConcatAvailableItems("|"));
        text.Attributes.Add("onchange", "HandleChange()");
        this.Controls.Add(text);
        this.Controls.Add(new LiteralControl("<img alt=\"Display lookup values\" onclick=\"ShowDropdown('" + text.ClientID + "');\" src=\"/_layouts/images/dropdown.gif\" style=\"border-width: 0px; vertical-align: middle;\" />"));
        this.Controls.Add(new LiteralControl("</span>"));
    }

    private void CreateStandardSelect()
    {
        DropDownList child = new DropDownList {
            ID = "Lookup",
            ToolTip = string.Format(CultureInfo.InvariantCulture, "{0}", new object[] { this.Field.InternalName })
        };
        if (!Util.ListIsNullOrEmpty(this._availableItems))
        {
            child.Items.Clear();
            child.Items.AddRange(this._availableItems.ToArray());
        }
        if (!this.Field.Required)
        {
            child.Items.Insert(0, new ListItem("(None)", "0"));
        }
        this.Controls.Add(child);
    }

    private void EnsureValueIsAvailable()
    {
        Predicate<ListItem> match = null;
        if ((this._fieldVal != null) && !string.IsNullOrEmpty(this._fieldVal.LookupValue))
        {
            if (match == null)
            {
                match = x => x.Value.ToLower() == this._fieldVal.LookupId.ToString().ToLower();
            }
            if (this._availableItems.Find(match) == null)
            {
                this._availableItems.Add(new ListItem(this._fieldVal.LookupValue, this._fieldVal.LookupId.ToString()));
            }
        }
    }

    private SPFieldLookupValue GetCustomSelectValue(HtmlInputText txtBox)
    {
        Predicate<ListItem> match = null;
        Control h = this.FindControl(string.Format(CultureInfo.InvariantCulture, "{0}_Hidden", new object[] { this.Field.InternalName }));
        if ((h != null) && !string.IsNullOrEmpty(((HtmlInputHidden) h).Value))
        {
            if (match == null)
            {
                match = x => x.Value.ToLower() == ((HtmlInputHidden) h).Value.ToLower();
            }
            ListItem item = this._availableItems.Find(match);
            if (((item != null) && (item.Value != "0")) && (item.Text.ToLower() == txtBox.Value.ToLower()))
            {
                return new SPFieldLookupValue(int.Parse(item.Value), item.Text);
            }
        }
        return new SPFieldLookupValue();
    }

    private Control GetRenderingWebControl()
    {
        foreach (Control control2 in this.Controls)
        {
            if ((control2.ID == "Lookup") && (control2.GetType().FullName == "System.Web.UI.WebControls.DropDownList"))
            {
                return control2;
            }
            if ((control2.ID == "Txtbx") && (control2.GetType().FullName == "System.Web.UI.HtmlControls.HtmlInputText"))
            {
                return control2;
            }
        }
        return null;
    }

    private void Initialize()
    {
        this._availableItems = Util.GetAvailableValues((RelatedCustomLookup) base.Field, this.Context);
        if (!Util.ListIsNullOrEmpty(this._availableItems))
        {
            this.EnsureValueIsAvailable();
        }
    }

    private bool IsExplorerOnWin()
    {
        HttpBrowserCapabilities browser = this.Page.Request.Browser;
        return (((browser.Browser.ToLower() == "ie") && (browser.Platform.ToLower() == "winnt")) && (browser.MajorVersion > 5));
    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        if ((base.ControlMode == SPControlMode.Edit) || (base.ControlMode == SPControlMode.Display))
        {
            if (base.ListItemFieldValue != null)
            {
                this._fieldVal = base.ListItemFieldValue as SPFieldLookupValue;
            }
            else
            {
                this._fieldVal = new SPFieldLookupValue();
            }
        }
        if (base.ControlMode == SPControlMode.New)
        {
            this._fieldVal = new SPFieldLookupValue();
            SPFormContext formContext = SPContext.Current.FormContext;
            formContext.OnSaveHandler = (EventHandler) Delegate.Combine(formContext.OnSaveHandler, new EventHandler(this.SaveHandler));
        }
        this.Initialize();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if ((base.ControlMode != SPControlMode.Display) && !this.Page.IsPostBack)
        {
            this.SetValue();
        }
    }

    protected void SaveHandler(object sender, EventArgs e)
    {
        Trace.TraceInformation("SaveHandler");
        this.ListItem[this.Field.InternalName] = this._fieldVal;
        this.ListItem.UpdateOverwriteVersion();
        Trace.TraceInformation("SaveHandler finished");
    }

    private void SetCustomSelectValue(HtmlInputText txtBox)
    {
        if ((this._fieldVal != null) && !string.IsNullOrEmpty(this._fieldVal.LookupValue))
        {
            txtBox.Value = this._fieldVal.LookupValue;
            Control control = this.FindControl(string.Format(CultureInfo.InvariantCulture, "{0}_Hidden", new object[] { this.Field.InternalName }));
            if (control != null)
            {
                ((HtmlInputHidden) control).Value = this._fieldVal.LookupId.ToString();
            }
        }
    }

    private void SetValue()
    {
        Control renderingWebControl = this.GetRenderingWebControl();
        if (!Util.ListIsNullOrEmpty(this._availableItems) && (renderingWebControl != null))
        {
            if (renderingWebControl.GetType().FullName == "System.Web.UI.WebControls.DropDownList")
            {
                DropDownList list = renderingWebControl as DropDownList;
                if ((this._fieldVal != null) && !string.IsNullOrEmpty(this._fieldVal.LookupValue))
                {
                    ListItem item = list.Items.FindByValue(this._fieldVal.LookupId.ToString());
                    if (item != null)
                    {
                        list.SelectedIndex = list.Items.IndexOf(item);
                        base.ItemIds.Add(this._fieldVal.LookupId);
                    }
                    else
                    {
                        list.SelectedIndex = 0;
                    }
                }
                else
                {
                    list.SelectedIndex = 0;
                }
            }
            else
            {
                this.SetCustomSelectValue((HtmlInputText) renderingWebControl);
            }
        }
    }

    // Properties
    protected override string DefaultTemplateName
    {
        get
        {
            return "RelatedCustomLookupControl";
        }
    }

    public override object Value
    {
        get
        {
            this.EnsureChildControls();
            Control renderingWebControl = this.GetRenderingWebControl();
            if (renderingWebControl != null)
            {
                if (renderingWebControl is DropDownList)
                {
                    DropDownList list = renderingWebControl as DropDownList;
                    if ((list.SelectedItem.Value != "0") && (list.SelectedItem.Text != "(None)"))
                    {
                        this._fieldVal = new SPFieldLookupValue(int.Parse(list.SelectedValue), list.SelectedItem.Text);
                        return new SPFieldLookupValue(int.Parse(list.SelectedItem.Value), list.SelectedItem.Text);
                    }
                }
                else if (renderingWebControl is HtmlInputText)
                {
                    return this.GetCustomSelectValue((HtmlInputText) renderingWebControl);
                }
            }
            return new SPFieldLookupValue();
        }
        set
        {
            this.EnsureChildControls();
            base.Value = value as SPFieldLookupValue;
        }
    }
}

 

 


}
