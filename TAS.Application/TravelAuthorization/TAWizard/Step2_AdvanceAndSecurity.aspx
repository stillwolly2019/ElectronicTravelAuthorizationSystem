<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Step2_AdvanceAndSecurity.aspx.cs" Inherits="TravelAuthorization_TAWizard_Step2_AdvanceAndSecurity" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/TravelAuthorization/TAWizard/WizardHeader.ascx" TagPrefix="uc1" TagName="WizardHeader" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>
<link href="~/css/Site.css" rel="stylesheet" />
<link href="~/css/bootstrap.css" rel="stylesheet" />
<!-- MetisMenu CSS -->
<link href="~/bower_components/metisMenu/dist/metisMenu.min.css" rel="stylesheet" />
<!-- Custom CSS -->
<link href="~/dist/css/sb-admin-2.css" rel="stylesheet" />
<!-- Custom Fonts -->
<link href="~/bower_components/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
<style type="text/css">
.auto-style1 {
vertical-align: middle;
background-color: transparent;
text-align: justify;
padding: 6px;
font-size: 12px;
}
    .auto-style3 {
        position: absolute;
        left: 0px;
        top: 0px;
        width: 100%;
        height: 147%;
        z-index: 99999;
    }
</style>
</head>
<body style="background-color: #fff">

<form id="form1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
<script src="<%= Page.ResolveClientUrl("~/js/jquery-1.11.3.min.js") %>" type="text/javascript"></script>

<%--<script type="text/javascript">
    window.open("foo.aspx");
</script>--%>

 

<script type="text/javascript">
    function RedirectToNewWindow() {
        var taid = $('#hftaid').val();
        var link= '../../ViewPDF.aspx?taid='+taid;
        window.open(link,'_blank');
        //window.parent.location.href =link;
    }
function ConfirmDelete() {
return confirm("Are you sure you want to delete?");
}
 function openModal() {
            $('#DivDuplicateTA').modal('show');
        }

function SaveValidation() {
var a = 0;
$(".Req").each(function () {

if ($(this).val() == "" || $(this).val() == null || $(this).val() == "-- Please Select --" ||
$(this).val() == "00000000-0000-0000-0000-000000000000") {
$(this).addClass("invalid");
a = a + 1;
} else {
$(this).removeClass("invalid");
}
});
if (a > 0) {
return false;
}
else {
return true;
}
}
</script>




<uc1:WizardHeader runat="server" ID="WizardHeader" />
<asp:UpdatePanel ID="UpdatePanelStep2" runat="server">
<ContentTemplate>
<div class="panel panel-default">
<div class="panel-heading">
<h4 class="panel-title">Step 2 - Advance Request and Security Requirements</h4>
    <asp:HiddenField ID="hftaid" runat="server" Value="" />
</div>
<div class="panel-body">

<div class="row" style="width: 99.9%;">

<div class="col-lg-12">
<asp:Panel ID="PanelMessage" runat="server" CssClass="alert alert-success alert-dismissable" Visible="False">
<asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
</asp:Panel>
<asp:Panel ID="pnlContent1" runat="server">
<table>
   <tr>
      <td class="tdLabel">Travel advance requested</td>
      <td class="tdField" colspan="2">
         <asp:RadioButton ID="rdYesAdv" AutoPostBack="true" OnCheckedChanged="rdYesAdv_CheckedChanged" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Adv" />
         <asp:RadioButton ID="rdNoAdv" AutoPostBack="true" OnCheckedChanged="rdNoAdv_CheckedChanged" runat="server" Text="No" CssClass="radio-inline" GroupName="Adv" />
      </td>
   </tr>
   <tr>
      <td class="tdLabel">Currency</td>
      <td class="tdField" colspan="2">
         <asp:DropDownList ID="DDLAdvCurr" Enabled="false" runat="server" CssClass="form-control" DataTextField="CurrencyName" DataValueField="CurrencyID"></asp:DropDownList>
      </td>
   </tr>
   <tr>
      <td class="tdLabel">Amount</td>
      <td class="tdField" colspan="2">
         <asp:TextBox ID="txtAdvAmnt" Enabled="false" runat="server" CssClass="form-control"></asp:TextBox>
      </td>
      <td>
         <asp:Label ID="lblAmountMsg" Visible="false" ForeColor="Red" Text="Only numbers" runat="server" />
      </td>
   </tr>
   <tr>
      <td class="tdLabel">Via</td>
      <td class="tdField" colspan="4">
         <asp:RadioButton ID="rdViaBank" AutoPostBack="true" OnCheckedChanged="rdViaBank_CheckedChanged" Enabled="false" runat="server" Text="Bank Transfer" CssClass="radio-inline" GroupName="Via" />
         <asp:RadioButton ID="rdViaCheck" AutoPostBack="true" OnCheckedChanged="rdViaCheck_CheckedChanged" Enabled="false" runat="server" Text="Check" CssClass="radio-inline" GroupName="Via" />
         <asp:RadioButton ID="rdViaCash" AutoPostBack="true" OnCheckedChanged="rdViaCash_CheckedChanged" Enabled="false" runat="server" Text="Cash" CssClass="radio-inline" GroupName="Via" />
      </td>
   </tr>
   <tr>
      <td colspan="5">
         <hr />
      </td>
   </tr>
   <tr>
      <td class="tdLabel">Visa(s) Obtained</td>
      <td class="tdField" colspan="2">
         <asp:RadioButton ID="rdVisYes" AutoPostBack="true" OnCheckedChanged="rdVisYes_CheckedChanged" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Visa" />
         <asp:RadioButton ID="rdVisaNo" AutoPostBack="true" OnCheckedChanged="rdVisaNo_CheckedChanged" runat="server" Text="No" CssClass="radio-inline" GroupName="Visa" />
         <asp:RadioButton ID="rdVisaNA" AutoPostBack="true" OnCheckedChanged="rdVisaNA_CheckedChanged" runat="server" Text="N/A" CssClass="radio-inline" GroupName="Visa" />
      </td>
   </tr>
   <tr>
      <td class="tdLabel"></td>
      <td class="tdField">
         <asp:TextBox ID="txtVisa" runat="server" Enabled="false" CssClass="form-control" placeholder="Visa issue date"></asp:TextBox>
         <asp:CalendarExtender ID="Calendar22" runat="server" TargetControlID="txtVisa" Format="dd-MMM-yyyy"></asp:CalendarExtender>
      </td>
   </tr>
   <tr>
      <td class="tdLabel" style="width: 250px">Health Briefings and vaccination obtained</td>
      <td class="tdField" colspan="2">
         <asp:RadioButton ID="rdHealthYes" AutoPostBack="true" OnCheckedChanged="rdHealthYes_CheckedChanged" runat="server" Text="Yes" CssClass="radio-inline" GroupName="Health" />
         <asp:RadioButton ID="rdHealthNo" AutoPostBack="true" OnCheckedChanged="rdHealthNo_CheckedChanged" runat="server" Text="No" CssClass="radio-inline" GroupName="Health" />
         <asp:RadioButton ID="rdHealthNA" AutoPostBack="true" OnCheckedChanged="rdHealthNA_CheckedChanged" runat="server" Text="N/A" CssClass="radio-inline" GroupName="Health" />
      </td>

      <td class="tdLabel">
         <asp:Label Text="" Visible="false" Font-Bold="false" ID="lblMedicalClearaneAttach" runat="server" />
      </td>
      <td class="tdField" colspan="2">
         <asp:Panel ID="pnlMedicalFiles" runat="server">
            <asp:LinkButton ID="ibViewMedical" runat="server" OnClick="ibViewMedical_Click">
               <li class="fa fa-search"></li>
               View File
            </asp:LinkButton>
            | 
            <asp:LinkButton ID="ibDeleteMedical" runat="server" OnClick="ibDeleteMedical_Click" OnClientClick="return ConfirmDelete();">
               <li class="fa fa-times"></li>
               Delete File
            </asp:LinkButton>
         </asp:Panel>
         <asp:AsyncFileUpload ID="fuMedicalAttachement" Width="100%" runat="server" CompleteBackColor="Green" OnClientUploadComplete="UploadCompleteMedical"
            OnClientUploadError="UploadErrorMedical" OnClientUploadStarted="StartUploadMedical" OnUploadedComplete="ProcessUploadMedical" ThrobberID="spanUploading" />
      </td>
      <td colspan="2">
         <img id="LoadingImgMed" alt="Loading" src="../../images/ajax_loader_blue_512.gif" style="display: none; width: 30px" />
         <asp:Label ID="lblUploadMessageMedical" runat="server"></asp:Label>
      </td>
   </tr>
   
   <tr>
      <td class="tdLabel" style="width: 250px">Upload any Other Supporting Documents obtained</td>
      <td class="tdField" colspan="2">
         <asp:RadioButton ID="rdOtherDocumentsYes" AutoPostBack="true" OnCheckedChanged="rdOtherDocumentsYes_CheckedChanged" runat="server" Text="Yes" CssClass="radio-inline" GroupName="OtherDocuments" />
         <asp:RadioButton ID="rdOtherDocumentsNo" AutoPostBack="true" OnCheckedChanged="rdOtherDocumentsNo_CheckedChanged" runat="server" Text="No" CssClass="radio-inline" GroupName="OtherDocuments" />
         <asp:RadioButton ID="rdOtherDocumentsNA" AutoPostBack="true" OnCheckedChanged="rdOtherDocumentsNA_CheckedChanged" runat="server" Text="N/A" CssClass="radio-inline" GroupName="OtherDocuments" />
      </td>
      <td class="tdLabel">
         <asp:Label Text="" Visible="false" Font-Bold="false" ID="lblOtherDocumentsClearaneAttach" runat="server" />
      </td>
      <td class="tdField" colspan="2">
         <asp:Panel ID="pnlOtherDocumentsFiles" runat="server">
            <asp:LinkButton ID="ibViewOtherDocuments" runat="server" OnClick="ibViewOtherDocuments_Click">
               <li class="fa fa-search"></li>
               View File
            </asp:LinkButton>
            | 
            <asp:LinkButton ID="ibDeleteOtherDocuments" runat="server" OnClick="ibDeleteOtherDocuments_Click" OnClientClick="return ConfirmDelete();">
               <li class="fa fa-times"></li>
               Delete File
            </asp:LinkButton>
         </asp:Panel>
         <asp:AsyncFileUpload ID="fuOtherDocumentsAttachement" Width="100%" runat="server" CompleteBackColor="Green" OnClientUploadComplete="UploadCompleteOtherDocuments"
            OnClientUploadError="UploadErrorOtherDocuments" OnClientUploadStarted="StartUploadOtherDocuments" OnUploadedComplete="ProcessUploadOtherDocuments" ThrobberID="spanUploading" />
      </td>
      <td colspan="2">
         <img id="LoadingImgOtherDocuments" alt="Loading" src="../../images/ajax_loader_blue_512.gif" style="display: none; width: 30px" />
         <asp:Label ID="lblUploadMessageOtherDocuments" runat="server"></asp:Label>
      </td>
   </tr>
 
   <tr>
      <td class="tdLabel">Security training completed</td>
      <td class="tdField">
         <asp:CheckBox ID="checkSecurityTraining" AutoPostBack="true" OnCheckedChanged="checkSecurityTraining_CheckedChanged" runat="server" CssClass="checkbox" Text="" />
      </td>
   </tr>
   <tr>
      <td class="tdLabel">If yes, requested by</td>
      <td class="tdField" colspan="2">
         <asp:RadioButton ID="rdMissionYes" Enabled="false" runat="server" AutoPostBack="true" OnCheckedChanged="rdMissionYes_CheckedChanged" Text="Mission" CssClass="radio-inline" GroupName="Mission" />
         <asp:RadioButton ID="rdMissionNo" Enabled="false" runat="server" AutoPostBack="true" OnCheckedChanged="rdMissionNo_CheckedChanged" Text="Headquarters" CssClass="radio-inline" GroupName="Mission" />
      </td>
   </tr>
   <tr>
      <td class="tdLabel">Security Clearance Needed</td>
      <td class="tdField">
         <asp:CheckBox ID="checkSecurityClearance" Checked="true" Enabled="false" runat="server" Text="" OnCheckedChanged="checkSecurityClearance_CheckedChanged" CssClass="checkbox" AutoPostBack="true" />
      </td>
   </tr>
   <tr id="rowDSANotApp" runat="server">
      <td class="tdLabel">DSA Not Applicable</td>
      <td class="tdField" colspan="2">
         <asp:CheckBox ID="checkNotForDSA" Visible="true" runat="server" Text="" CssClass="checkbox" />
      </td>
   </tr>
   <tr>
      <td colspan="5">
         <hr />
      </td>
   </tr>
</table>
</asp:Panel>
<table>
<tr>
    <td class="auto-style1" style="width: 250px">
        <asp:Label Text="Security Clearance (SCR) Attachment:" Font-Bold="false" ID="lblattch" runat="server" />
        <span style="color:red">*attach only the initial UNDSS approved PDF SCR. Office of Staff Security (OSS) shall then endorse TA and SCR documents at the end of the process.</span>
    &nbsp;</td>
    <td class="tdField">
        <asp:Panel ID="pnlFile" runat="server">
            <%--<asp:LinkButton ID="ibView" runat="server" OnClick="ibView_Click" ><li class="fa fa-search"></li> View File</asp:LinkButton>
           --%>
           <asp:LinkButton ID="ibView" runat="server" OnClientClick="return RedirectToNewWindow();" ><li class="fa fa-search"></li> View File</asp:LinkButton>

            





            | 
            <asp:LinkButton ID="ibDelete" runat="server" OnClick="ibDelete_Click" OnClientClick="return ConfirmDelete();"><li class="fa fa-times"></li> Delete File</asp:LinkButton>
        </asp:Panel>
        <asp:AsyncFileUpload ID="fuAttachment" Width="100%" ToolTip="This can be left blank" runat="server" CompleteBackColor="Green" OnClientUploadComplete="UploadComplete"
            OnClientUploadError="UploadError" OnClientUploadStarted="StartUpload" OnUploadedComplete="ProcessUpload" ThrobberID="spanUploading" />
    </td>
    <td>
        <img id="LoadingImg" alt="Loading" src="../../images/ajax_loader_blue_512.gif" style="display: none; width: 30px" />
        <asp:Label ID="lblUploadMsg" runat="server"></asp:Label>
         
  
    </td>
</tr>
                      
<%--<tr>
    <td colspan="3" style="width:570px">
        <hr />
    </td>
</tr>--%>
</table>



<asp:Panel ID="pnlContent2" runat="server">
<table>
    <tr>
        <td colspan="5">Please confirm that the Chief of Mission (COM) at destination has been informed of your arrival. If the function of COM at the duty 
        station of destination
        <br />
            does not exist or if the COM is absent,
        the Officer In Charge of that Mission or the Regional Director at the 
        appropriate Regional Office must be informed.&nbsp;&nbsp;&nbsp;&nbsp;
        <span style="color: red">*</span>
            <asp:CheckBox ID="checkConfirm" runat="server" Text="Yes" CssClass="checkbox-inline" />
        </td>
    </tr>
    <tr>
        <td colspan="4"></td>
        <td class="tdLabel" colspan="5" style="text-align: right; float: right">
            <asp:Button ID="brnSaveAdvance" OnClick="btnSave_Click" runat="server" CssClass="form-control btn-primary" OnClientClick="return SaveValidation();" Font-Size="14px" Text="Submit & Continue" Width="280px" />
        </td>
    </tr>
    <tr>
        <td colspan="4"></td>
        <td class="tdLabel" colspan="5" style="text-align: right; float: right">
            
        </td>
    </tr>
     <tr>
        <td colspan="4"></td>
        <td class="tdLabel" colspan="5" style="text-align: right; float: right">
            
        </td>
    </tr>

</table>
</asp:Panel>
</div>
</div>


</div>



</div>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="ibViewMedical" />
<asp:PostBackTrigger ControlID="ibView" />

<asp:PostBackTrigger ControlID="ibViewOtherDocuments" />


</Triggers>
</asp:UpdatePanel>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep2" DisplayAfter="100" DynamicLayout="False">
<ProgressTemplate>
<div runat="server" id="LoadingDiv" style="position: absolute; left: 0px; top: 0px; width: 100%; height: 100%; background-color: Gray; opacity: 0.5; filter: alpha(opacity=50); z-index: 99999; text-align: center; vertical-align: middle">
<div style="padding-top: 200px;">
<img alt="Loading" src="../../images/ajax_loader_blue_512.gif" width="200" />
</div>
</div>
</ProgressTemplate>
</asp:UpdateProgress>
</form>
<script src="<%= Page.ResolveClientUrl("~/TravelAuthorization/TAWizard/TAUpload.js") %>" type="text/javascript"></script>
<!-- jQuery -->
<script src="<%= Page.ResolveClientUrl("~/bower_components/jquery/dist/jquery.min.js") %>" type="text/javascript"></script>
<!-- Bootstrap Core JavaScript -->
<script src="<%= Page.ResolveClientUrl("~/bower_components/bootstrap/dist/js/bootstrap.min.js") %>" type="text/javascript"></script>
<!-- Metis Menu Plugin JavaScript -->
<script src="<%= Page.ResolveClientUrl("~/bower_components/metisMenu/dist/metisMenu.min.js") %>" type="text/javascript"></script>
<!-- Custom Theme JavaScript -->
<script src="<%= Page.ResolveClientUrl("~/dist/js/sb-admin-2.js") %>" type="text/javascript"></script>



</body>
</html>

