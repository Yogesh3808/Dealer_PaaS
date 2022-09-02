<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDtlsContentMgmt.aspx.cs" Title="MTI-Content Management"
    Theme="SkinFile" EnableViewState="true" EnableEventValidation="false" Inherits="MANART.Forms.Admin.frmDtlsContentMgmt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/WebParts/Location.ascx" TagName="Location" TagPrefix="uc2" %>
<%@ Register Src="~/WebParts/CurrentDate.ascx" TagName="CurrentDate" TagPrefix="uc3" %>
<%@ Register Src="~/WebParts/SearchGridView.ascx" TagName="SearchGridView" TagPrefix="uc4" %>
<%@ Register Src="~/WebParts/Toolbar.ascx" TagName="Toolbar" TagPrefix="uc5" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../CSS/Style.css" rel="Stylesheet" type="text/css" />
    

    <script type="text/javascript" src="../../JavaScripts/jsValidationFunction.js"></script>

    <script src="../../JavaScripts/jsGridFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsMessageFunction.js" type="text/javascript"></script>

    <script src="../../JavaScripts/jsToolbarFunction.js" type="text/javascript"></script>
   
    <script type ="text/javascript" >
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                //check if nxt sibling is not null & is an element node
                if (nxtSibling && nxtSibling.nodeType == 1) {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;
            if (parentNodeTable) {
                var checkUncheckSwitch;
                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) {
                    //check if the child node is an element node
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }


        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        function ShowInformationMessage(obj) {
            alert(obj)
            return false;
        }
        function Close() {
            window.close();
        }
    </script> 
</head>
<body>
    <form id="form1" runat="server" >
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div>
    <table id="PageTbl" border="1" width ="100%" >
     <%--<tr id="ToolbarPanel">
            <td style="width: 15%">
                <table id="ToolbarContainer" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                            <uc5:Toolbar ID="ToolbarC" runat="server"  OnImage_Click="ToolbarImg_Click" />
                        </td>
                    </tr>
                </table>
            </td>
    </tr>--%>
    <tr id="TrMsg" runat ="server" class="ToolbarTable">
    <td>
    
    <asp:Label ID="lblMsg" runat ="server" Text ="" Font-Size="Medium" Font-Bold ="true"  ></asp:Label>
    
    </td>
    </tr>
    <tr id="TblControl" class="ToolbarTable">
    <td>
    <asp:RadioButtonList ID="rdoType" runat ="server" 
            onselectedindexchanged="rdoType_SelectedIndexChanged" AutoPostBack="true"  >
    <asp:ListItem Selected="True" Value ="0">Common</asp:ListItem>
    <asp:ListItem Value ="D">Domestic</asp:ListItem>
     <asp:ListItem Value ="E">Export</asp:ListItem>
    </asp:RadioButtonList>
    
    </td>
    </tr>
    <tr id="trMenu" runat ="server" class="ToolbarTable">
            <td style="width: 100%">                  
                
           <div class="DivTree" style ="overflow:auto;" >
                                            <asp:TreeView ID="TreeDocument" runat="server" AutoGenerateDataBindings="False"
                                                Font-Names="Verdana" Font-Size="X-Small" ForeColor="#156FC4" Height="97px" 
                                                ShowCheckBoxes="All" Style="direction: ltr; border-top-style: none;
                                                border-right-style: none; border-left-style: none; text-align: left; border-bottom-style: none;
                                                font-family: verdana, sans-serif;cursor:auto" Width="244px"  
                                                onclick="OnTreeClick(event);" CssClass="tdLabel" ImageSet="Arrows">                                               
                                                <LeafNodeStyle Font-Bold="False" />
                                                <HoverNodeStyle Font-Underline="False" ForeColor="#5555DD" />
                                                <SelectedNodeStyle Font-Underline="False" ForeColor="#5555DD" 
                                                    HorizontalPadding="0px" VerticalPadding="0px" />
                                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                                                    HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                                               
                                            </asp:TreeView>
                                        </div>       
                    
            </td>
        </tr>
        <tr id="Tr1" align ="center" >
            <td style="width: 15%">
                <table id="Table1" runat="server" width="100%" border="1" class="ToolbarTable">
                    <tr>
                        <td>
                           <asp:Button ID="bSave" runat ="server" Text="Save" CssClass="CommandButton" 
                                onclick="bSave_Click"/>
                           <asp:Button ID="btnCancel" runat ="server" Text="Back" OnClientClick="Close()" CssClass="CommandButton" />
                        </td>
                    </tr>
                </table>
            </td>
    </tr>
    </table>
      </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
