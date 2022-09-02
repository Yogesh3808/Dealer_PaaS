<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Toolbar.ascx.cs" Inherits="MANART.WebParts.Toolbar" %>
<!-- Old CSS FILE-->
<link href="../Content/style.css" rel="stylesheet" />
<script src="../Scripts/jsValidationFunction.js"></script>
<link href="../Content/bootstrap.css" rel="stylesheet" />
<div class="table-responsive">
<table id ="Toolbar" style="height: 13px; width: 100%">
     <tr >
            <td  >                                
                <table id="ToolbarContainer" runat="server" width="100%" border="1"  class="">
                    <tr >
                        <td class="ToolbarImgColumn" >
                              <asp:ImageButton ID="ToolbarButton1"   
                                 ImageUrl="~/images/ToolbarImage/New.ico" 
                                onmouseover ="return ShowToolBarMessage(this);" onmouseout="RemoveToolBarName(this);" 
                                title="New" 
                                runat="server"  Height="16px" Width="16px" 
                                onclick="ImageButton_Click" />    
                        </td>
                        <td class="ToolbarImgColumn" >                            
                            <asp:ImageButton ID="ToolbarButton2"   
                                ImageUrl="~/images/ToolbarImage/Save.ico" 
                                onmouseover ="return CheckValidDataBeforeSave(this);" onmouseout="RemoveToolBarName(this);" 
                                title="Save" 
                                runat="server"  Height="16px" Width="16px" 
                                onclick="ImageButton_Click" />                                                  
                        </td>
                        <td class="ToolbarImgColumn" >
                            <asp:ImageButton ID="ToolbarButton3"  
                                ImageUrl="~/images/ToolbarImage/Confirm.png"  
                                onmouseover ="return ShowToolBarMessage(this);" onmouseout="RemoveToolBarName(this);"
                                title="Confirm"
                                runat="server" onclick="ImageButton_Click" Height="16px" Width="16px" />                            
                        </td>
                        <td class="ToolbarImgColumn" >
                            <asp:ImageButton ID="ToolbarButton4" 
                                ImageUrl="~/images/ToolbarImage/Cancel.png" 
                                onmouseover ="return ShowToolBarMessage(this);" onmouseout="RemoveToolBarName(this);"
                                OnClientClick="return CheckForCancel();"
                                title="Cancel" onclick="ImageButton_Click"
                                runat="server" Height="16px" 
                                Width="16px" />                            
                        </td>
                        <td class="ToolbarImgColumn" >
                            <asp:ImageButton ID="ToolbarButton5" 
                                title="Print"
                                ImageUrl="~/images/ToolbarImage/Print.ico" 
                                onmouseover ="return ShowToolBarMessage(this);" onmouseout="RemoveToolBarName(this);"
                                
                                 runat="server" Height="16px" onclick="ImageButton_Click"
                                Width="18px" />
                        </td>
                        <td class="ToolbarImgColumn" >
                            <img src="~/images/ToolbarImage/Help.png" runat ="server" id="ToolbarButton6" 
                                onmouseover ="return ShowToolBarMessage(this);" onmouseout="RemoveToolBarName(this);"
                             title="Help"                                  style="height: 15px; width: 20px" />                            
                            
                        </td>
                        <td >                        
                        <asp:TextBox ID="txtShowToolBarName" runat="server" Text="" Font-Bold="true" ForeColor="DarkBlue"  ReadOnly="true" Width="80%" CssClass="TextBoxForString"></asp:TextBox>                            
                        </td>
                        <td>            
                              
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
</table>
</div>
