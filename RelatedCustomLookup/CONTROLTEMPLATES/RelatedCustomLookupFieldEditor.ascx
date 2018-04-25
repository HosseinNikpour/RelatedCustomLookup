<%@ Control Language="C#" Inherits="RelatedCustomLookup.RelatedCustomLookupFieldEditor,RelatedCustomLookup, Version=1.0.0.0, Culture=neutral, PublicKeyToken=8749e53bcf25eebf"   AutoEventWireup="false" compilationMode="Always" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="~/_controltemplates/15/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="~/_controltemplates/15/InputFormSection.ascx" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
<script runat="server">

   
    
</script>


<wssuc:InputFormSection runat="server" id="MySections" Title="My Custom Section">
       <Template_InputFormControls>
             <wssuc:InputFormControl runat="server"
                    LabelText="Select List for Query">
                    <Template_Control>
                           <asp:DropDownList id="ddlListNameLookup" runat="server" OnSelectedIndexChanged="ddlListNameLookup_SelectedIndexChanged" AutoPostBack="true">
                           </asp:DropDownList>
                        <asp:Label runat="server" ID="lblSelectedLookupList" Visible="false"></asp:Label>
                    </Template_Control>


             </wssuc:InputFormControl>
           <wssuc:InputFormControl runat="server"
               LabelText="Check If File">
               <Template_Control>
                   <asp:CheckBox  ID="cbxFile" runat="server" Enabled="true"/>   
                            
               </Template_Control>
           
           </wssuc:InputFormControl>
           <wssuc:InputFormControl runat="server"
               LabelText="Volume of File">
               <Template_Control>
                   
                  <asp:TextBox ID="txtVolumeFile" runat="server" ></asp:TextBox> 
                   <asp:Label>KB</asp:Label>  
               
               </Template_Control>
           
           </wssuc:InputFormControl>
             <wssuc:InputFormControl runat="server"
               LabelText="Type Of File">
               <Template_Control>
                  
                  <asp:TextBox ID="txtTypeFile" runat="server" Width="100px" ></asp:TextBox>  
                      
               </Template_Control>
           
           </wssuc:InputFormControl>
         
           <wssuc:InputFormControl runat="server"
                    LabelText="Select Title Field for Query">
                    <Template_Control>
                           <asp:DropDownList id="ddlFieldTitleLookup" runat="server"  >
                           </asp:DropDownList>
                    </Template_Control>

             </wssuc:InputFormControl>

             <wssuc:InputFormControl runat="server"
                    LabelText="Select  Value Field for Query">
                    <Template_Control>
                           <asp:DropDownList id="ddlFieldValueLookup" runat="server"  >
                           </asp:DropDownList>
                    </Template_Control>


             </wssuc:InputFormControl>

             <wssuc:InputFormControl runat="server"
                    LabelText="Dependent Fields">
                    <Template_Control>
                           <asp:CheckBoxList id="ddlDependentFields" runat="server" >
                           </asp:CheckBoxList>
                    </Template_Control>


             </wssuc:InputFormControl>

             



            <wssuc:InputFormControl runat="server"
                    LabelText="Query">
                    <Template_Control>
                           <asp:TextBox ID="txtQuery" TextMode="MultiLine" runat="server"  Width="350px"></asp:TextBox>
                    </Template_Control>


             </wssuc:InputFormControl>

       </Template_InputFormControls>
</wssuc:InputFormSection>
 

