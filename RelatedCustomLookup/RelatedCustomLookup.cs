using System;
using System.Collections.Generic;
using System.Security;
using Microsoft.SharePoint.Security;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.SharePoint;
using System.Diagnostics;
using Microsoft.SharePoint.WebControls;
using System.Security.Permissions;
namespace RelatedCustomLookup
{
  [Serializable]
public class RelatedCustomLookup : SPFieldLookup
{
    // Fields
    private string fieldTitleLookup;
    private string fieldValueLookup;
    private string isFile;
    private string listNameLookup;
    private string queryLookup;
    private string relatedFields;
    private string typeFile;
    private static Dictionary<int, string> updatedFieldTitleLookup = new Dictionary<int, string>();
    private static Dictionary<int, string> updatedFieldValueLookup = new Dictionary<int, string>();
    private static Dictionary<int, string> updatedIsFile = new Dictionary<int, string>();
    private static Dictionary<int, string> updatedListNameLookup = new Dictionary<int, string>();
    private static Dictionary<int, string> updatedQueryLookup = new Dictionary<int, string>();
    private static Dictionary<int, string> updatedRelatedFields = new Dictionary<int, string>();
    private static Dictionary<int, string> updatedTypeFile = new Dictionary<int, string>();
    private static Dictionary<int, string> updatedVolumeFile = new Dictionary<int, string>();
    private string volumeFile;

    // Methods
    public RelatedCustomLookup(SPFieldCollection fields, string fieldName) : base(fields, fieldName)
    {
        this.Init();
    }

    public RelatedCustomLookup(SPFieldCollection fields, string typeName, string displayName) : base(fields, typeName, displayName)
    {
        this.Init();
    }

    public string GetProperty(string propertyName)
    {
        Trace.TraceInformation("MultiColumnLookupField.GetProperty: " + propertyName);
        return base.GetProperty(propertyName);
    }

    public override string GetValidatedString(object value)
    {
        if (base.Required)
        {
            if ((value == null) || (value.ToString() == ""))
            {
                throw new SPFieldValidationException("Please fill in this mandatory field");
            }
            return value.ToString();
        }
        if (value != null)
        {
            return value.ToString();
        }
        return null;
    }

    private void Init()
    {
        this.ListNameLookup = this.GetCustomProperty("ListNameLookup") + "";
        this.FieldTitleLookup = this.GetCustomProperty("FieldTitleLookup")+"";
        this.FieldValueLookup = this.GetCustomProperty("FieldValueLookup")+"";
        this.IsFile = this.GetCustomProperty("IsFile")+"";
        this.VolumeFile = this.GetCustomProperty("VolumeFile")+"";
        this.TypeFile = this.GetCustomProperty("TypeFile")+"";
        this.RelatedFields =this.GetCustomProperty("RelatedFields")+"";
        this.QueryLookup = this.GetCustomProperty("QueryLookup")+"";
    }

    public override void OnAdded(SPAddFieldOptions op)
    {
        base.OnAdded(op);
        this.Update();
    }

    public override void Update()
    {
        base.SetCustomProperty("ListNameLookup", this.ListNameLookup);
        base.SetCustomProperty("FieldTitleLookup", this.FieldTitleLookup);
        base.SetCustomProperty("FieldValueLookup", this.FieldValueLookup);
        base.SetCustomProperty("RelatedFields", this.RelatedFields);
        base.SetCustomProperty("QueryLookup", this.QueryLookup);
        base.SetCustomProperty("IsFile", this.IsFile);
        base.SetCustomProperty("VolumeFile", this.VolumeFile);
        base.SetCustomProperty("TypeFile", this.TypeFile);
        base.LookupList = this.ListNameLookup.ToString();
        base.LookupField = "Title";
        base.Update();
        if (updatedListNameLookup.ContainsKey(this.ContextId))
        {
            updatedListNameLookup.Remove(this.ContextId);
        }
        if (updatedFieldTitleLookup.ContainsKey(this.ContextId))
        {
            updatedFieldTitleLookup.Remove(this.ContextId);
        }
        if (updatedFieldValueLookup.ContainsKey(this.ContextId))
        {
            updatedFieldValueLookup.Remove(this.ContextId);
        }
        if (updatedRelatedFields.ContainsKey(this.ContextId))
        {
            updatedRelatedFields.Remove(this.ContextId);
        }
        if (updatedQueryLookup.ContainsKey(this.ContextId))
        {
            updatedQueryLookup.Remove(this.ContextId);
        }
        if (updatedIsFile.ContainsKey(this.ContextId))
        {
            updatedIsFile.Remove(this.ContextId);
        }
        if (updatedVolumeFile.ContainsKey(this.ContextId))
        {
            updatedVolumeFile.Remove(this.ContextId);
        }
        if (updatedTypeFile.ContainsKey(this.ContextId))
        {
            updatedTypeFile.Remove(this.ContextId);
        }
    }

    public void UpdateFieldTitleLookup(string value)
    {
        updatedFieldTitleLookup[this.ContextId] = value;
    }

    public void UpdateFieldValueLookup(string value)
    {
        updatedFieldValueLookup[this.ContextId] = value;
    }

    public void UpdateIsFile(string value)
    {
        updatedIsFile[this.ContextId] = value;
    }

    public void UpdateListNameLookup(string value)
    {
        updatedListNameLookup[this.ContextId] = value;
    }

    public void UpdateQueryLookup(string value)
    {
        updatedQueryLookup[this.ContextId] = value;
    }

    public void UpdateRelatedFields(string value)
    {
        updatedRelatedFields[this.ContextId] = value;
    }

    public void UpdateTypeFile(string value)
    {
        updatedTypeFile[this.ContextId] = value;
    }

    public void UpdateVolumeFile(string value)
    {
        updatedVolumeFile[this.ContextId] = value;
    }

    // Properties
    public int ContextId
    {
        get
        {
            return SPContext.Current.GetHashCode();
        }
    }

    public override BaseFieldControl FieldRenderingControl
    {
        [SharePointPermission(SecurityAction.LinkDemand, ObjectModel=true)]
        get
        {
            BaseFieldControl control = null;
            if (this.AllowMultipleValues)
            {
                control = new RelatedCustomLookupControl();
            }
            else
            {
                control = new RelatedCustomLookupControl();
            }
            control.FieldName = base.InternalName;
            return control;
        }
    }

    public string FieldTitleLookup
    {
        get
        {
            return (updatedFieldTitleLookup.ContainsKey(this.ContextId) ? updatedFieldTitleLookup[this.ContextId] : this.fieldTitleLookup);
        }
        set
        {
            this.fieldTitleLookup = value;
        }
    }

    public string FieldValueLookup
    {
        get
        {
            return (updatedFieldValueLookup.ContainsKey(this.ContextId) ? updatedFieldValueLookup[this.ContextId] : this.fieldValueLookup);
        }
        set
        {
            this.fieldValueLookup = value;
        }
    }

    public override Type FieldValueType
    {
        get
        {
            return typeof(string);
        }
    }

    public string IsFile
    {
        get
        {
            return (updatedIsFile.ContainsKey(this.ContextId) ? updatedIsFile[this.ContextId] : this.isFile);
        }
        set
        {
            this.isFile = value;
        }
    }

    public string ListNameLookup
    {
        get
        {
            return (updatedListNameLookup.ContainsKey(this.ContextId) ? updatedListNameLookup[this.ContextId] : this.listNameLookup);
        }
        set
        {
            this.listNameLookup = value;
        }
    }

    public string QueryLookup
    {
        get
        {
            return (updatedQueryLookup.ContainsKey(this.ContextId) ? updatedQueryLookup[this.ContextId] : this.queryLookup);
        }
        set
        {
            this.queryLookup = value;
        }
    }

    public string RelatedFields
    {
        get
        {
            return (updatedRelatedFields.ContainsKey(this.ContextId) ? updatedRelatedFields[this.ContextId] : this.relatedFields);
        }
        set
        {
            this.relatedFields = value;
        }
    }

    public string TypeFile
    {
        get
        {
            return (updatedTypeFile.ContainsKey(this.ContextId) ? updatedTypeFile[this.ContextId] : this.typeFile);
        }
        set
        {
            this.typeFile = value;
        }
    }

    public Guid ValueColumnId
    {
        get
        {
            return SPBuiltInFieldId.ID;
        }
    }

    public string VolumeFile
    {
        get
        {
            return (updatedVolumeFile.ContainsKey(this.ContextId) ? updatedVolumeFile[this.ContextId] : this.volumeFile);
        }
        set
        {
            this.volumeFile = value;
        }
    }
}

 

 



}
